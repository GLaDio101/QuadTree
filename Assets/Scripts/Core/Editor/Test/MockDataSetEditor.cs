using System.Collections.Generic;
using Core.Testing;
using Core.Utils;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Core.Editor.Test
{
    [CustomEditor(typeof(MockDataSet))]
    public class MockDataSetEditor : UnityEditor.Editor
    {
        private GUIStyle _titleStyle;
        TextAsset script;

        private string _tagName;
        private int selected;
        private bool _customKey;

        public override void OnInspectorGUI()
        {
            MockDataSet myTarget = (MockDataSet) target;
            var bdr = GUI.skin.button.border;
            _titleStyle = new GUIStyle();
            _titleStyle = EditorStyles.helpBox;
            _titleStyle.alignment = TextAnchor.UpperCenter;
            _titleStyle.fontSize = 12;
            _titleStyle.fontStyle = FontStyle.Bold;
            _titleStyle.normal.textColor = Color.black;
            _titleStyle.normal.background = MakeTex(10, 10, Color.grey);
            _titleStyle.border = bdr;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Active Custom Key");
            _customKey = myTarget.CustomKey;
            _customKey = EditorGUILayout.Toggle(_customKey);

            EditorGUILayout.EndHorizontal();

            myTarget.Key = EditorGUILayout.TextField("CustomKey", myTarget.Key);

            if (_customKey != myTarget.CustomKey)
            {
                myTarget.CustomKey = _customKey;
                EditorUtility.SetDirty(myTarget);
                EditorSceneManager.MarkAllScenesDirty();
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            if (myTarget.DataList == null)
            {
                myTarget.DataList = new List<MockDataVo>();
                EditorUtility.SetDirty(myTarget);
                EditorSceneManager.MarkAllScenesDirty();
            }

            if (myTarget.DataList.Count > 0)
            {
                GUILayout.Label("Test Data Vos", _titleStyle);

                foreach (MockDataVo dataVo in myTarget.DataList)
                {
                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                    GUIStyle _dataActiveStyle = new GUIStyle();
                    _dataActiveStyle.alignment = TextAnchor.MiddleCenter;
                    _dataActiveStyle.fontSize = 18;
                    _dataActiveStyle.fontStyle = FontStyle.Bold;
                    _dataActiveStyle.normal.textColor = dataVo.IsActive ? Color.green : Color.red;
                    _dataActiveStyle.normal.background = MakeTex(10, 10, Color.black)
                        ;
                    GUILayout.Label("Test Name : " + dataVo.Tag);

                    GUILayout.Label(dataVo.IsActive ? "Active" : "Pasive", _dataActiveStyle);

                    EditorGUILayout.Space();

                    if (!dataVo.IsActive)
                    {
                        if (GUILayout.Button("Active"))
                        {
                            foreach (MockDataVo otherVo in myTarget.DataList)
                            {
                                otherVo.IsActive = false;
                            }

                            dataVo.IsActive = true;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Deactive"))
                        {
                            dataVo.IsActive = false;
                        }
                    }

                    if (GUILayout.Button("Remove"))
                    {
                        myTarget.DataList.Remove(dataVo);
                        return;
                    }


                    if (GUILayout.Button("Sellect"))
                    {
                        Selection.activeObject = dataVo.TextData;
                        return;
                    }


                    EditorGUILayout.EndHorizontal();
                }


                EditorGUILayout.Space();
                if (GUILayout.Button("ClearAllData"))
                {
                    myTarget.DataList = new List<MockDataVo>();
                }

                EditorGUILayout.Space();
            }


            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Add New Data", _titleStyle);
            EditorGUILayout.Space();

            _tagName = EditorGUILayout.TextField("Tag", _tagName);
            EditorGUILayout.Space();
            script = EditorGUILayout.ObjectField(script, typeof(TextAsset), false) as TextAsset;
            if (GUILayout.Button("Add New"))
            {
                if (script != null)
                {
                    myTarget.DataList.Add(new MockDataVo()
                    {
                        IsActive = false,
                        TextData = script,
                        Tag = _tagName
                    });
                    _tagName = "";
                    script = null;
                    EditorUtility.SetDirty(myTarget);
                    EditorSceneManager.MarkAllScenesDirty();
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }
    }
}