/*
* Version: 1.0
* Author:  Yilmaz Kiymaz (@VoxelBoy)
* Purpose: To be able to change the pivot of Game Objects
* 			without needing to use a separate 3D application. 
* License: Free to use and distribute, in both free and commercial projects.
* 			Do not try to sell as your own work. Simply put, play nice :)
* Contact: VoxelBoy on Unity Forums
*/

/*
 * - Doesn't work properly with rotated objects.
 * - Can't compensate for the positioning of Mesh Colliders.
 * - Need to figure out if the "Instantiating mesh" error in Editor is a big issue, if not, how to supress it.
 * - Allowing the pivot to move outside the bounds of the mesh, ideally using the movement gizmo but only affecting the pivot.
 */


using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Tools
{
    public class SetPivot : EditorWindow
    {
        private Vector3 _p; //Pivot value -1..1, calculated from Mesh bounds
        private Vector3 _lastP; //Last used pivot

        private GameObject _obj; //Selected object in the Hierarchy
        private MeshFilter _meshFilter; //Mesh Filter of the selected object
        private Mesh _mesh; //Mesh of the selected object
        private Collider _col; //Collider of the selected object

        private bool _pivotUnchanged; //Flag to decide when to instantiate a copy of the _mesh

        [MenuItem("Tools/Game Object/Set Pivot")]
        [UsedImplicitly]
        //Place the Set Pivot menu item in the GameObject menu
        private static void Init()
        {
            SetPivot window = (SetPivot)EditorWindow.GetWindow(typeof(SetPivot));
            window.RecognizeSelectedObject(); //Initialize the variables by calling RecognizeSelectedObject on the class instance
            window.Show();
        }

        [UsedImplicitly]
        private void OnGUI()
        {
            if (_obj)
            {
                if (_mesh)
                {
                    _p.x = EditorGUILayout.Slider("X", _p.x, -1.0f, 1.0f);
                    _p.y = EditorGUILayout.Slider("Y", _p.y, -1.0f, 1.0f);
                    _p.z = EditorGUILayout.Slider("Z", _p.z, -1.0f, 1.0f);
                    if (_p != _lastP)
                    { //Detects user input on any of the three sliders
                      //Only create instance of _mesh when user changes pivot
                        if (_pivotUnchanged) _mesh = _meshFilter.mesh; _pivotUnchanged = false;
                        UpdatePivot();
                        _lastP = _p;
                    }
                    if (GUILayout.Button("Center"))
                    { //Set pivot to the center of the _mesh bounds
                      //Only create instance of _mesh when user changes pivot
                        if (_pivotUnchanged) _mesh = _meshFilter.mesh; _pivotUnchanged = false;
                        _p = Vector3.zero;
                        UpdatePivot();
                        _lastP = _p;
                    }
                    GUILayout.Label("Bounds" + _mesh.bounds.ToString());
                }
                else
                {
                    GUILayout.Label("Selectedct does not have a Mesh specified.");
                }
            }
            else
            {
                GUILayout.Label("Noct selected in Hierarchy.");
            }
        }

        //Achieve the movement of the pivot by moving the transform position in the specified direction
        //and then moving all vertices of the _mesh in the opposite direction back to where they were in world-space
        private void UpdatePivot()
        {
            Vector3 diff = Vector3.Scale(_mesh.bounds.extents, _lastP - _p); //Calculate difference in 3d position
            _obj.transform.position -= Vector3.Scale(diff, _obj.transform.localScale); //Move object position
                                                                                       //Iterate over all vertices and move them in the opposite direction of the object position movement
            Vector3[] verts = _mesh.vertices;
            for (int i = 0; i < verts.Length; i++)
            {
                verts[i] += diff;
            }
            _mesh.vertices = verts; //Assign the vertex array back to the _mesh
            _mesh.RecalculateBounds(); //Recalculate bounds of the _mesh, for the renderer's sake
                                       //The 'center' parameter of certain colliders needs to be adjusted
                                       //when the transform position is modified
            if (_col)
            {
                if (_col is BoxCollider)
                {
                    ((BoxCollider)_col).center += diff;
                }
                else if (_col is CapsuleCollider)
                {
                    ((CapsuleCollider)_col).center += diff;
                }
                else if (_col is SphereCollider)
                {
                    ((SphereCollider)_col).center += diff;
                }
            }
        }

        //Look at the object's transform position in comparison to the center of its _mesh bounds
        //and calculate the pivot values for xyz
        private void UpdatePivotVector()
        {
            Bounds b = _mesh.bounds;
            Vector3 offset = -1 * b.center;
            _p = _lastP = new Vector3(offset.x / b.extents.x, offset.y / b.extents.y, offset.z / b.extents.z);
        }

        //When a selection change notification is received
        //recalculate the variables and references for the new object
        [UsedImplicitly]
        private void OnSelectionChange()
        {
            RecognizeSelectedObject();
        }

        //Gather references for the selected object and its components
        //and update the pivot vector if the object has a Mesh specified
        private void RecognizeSelectedObject()
        {
            Transform t = Selection.activeTransform;
            _obj = t ? t.gameObject : null;
            if (_obj)
            {
                _meshFilter = _obj.GetComponent(typeof(MeshFilter)) as MeshFilter;
                _mesh = _meshFilter ? _meshFilter.sharedMesh : null;
                if (_mesh)
                    UpdatePivotVector();
                _col = _obj.GetComponent(typeof(Collider)) as Collider;
                _pivotUnchanged = true;
            }
            else
            {
                _mesh = null;
            }
        }
    }
}