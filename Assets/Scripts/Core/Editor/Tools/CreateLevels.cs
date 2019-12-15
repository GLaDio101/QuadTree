using JetBrains.Annotations;
using UnityEditor;

namespace Core.Editor.Tools
{
    public class CreateLevels : ScriptableWizard
    {
        public int Start;

        public int Count;

        [MenuItem("Tools/Scene/Create Levels #&%l")]
        [UsedImplicitly]
        private static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Create Levels", typeof(CreateLevels), "Create");
        }

        [UsedImplicitly]
        private void OnWizardCreate()
        {
            var path = AssetDatabase.GetAssetPath(Selection.objects[0]);
            for (int i = Start; i < Count; i++)
            {
                var newPath = path.Replace("LevelBase", "LevelLabel" + i);
                var result = AssetDatabase.CopyAsset(path, newPath);

                if (result)
                {
                    AssetImporter importer = AssetImporter.GetAtPath(newPath);
                    importer.assetBundleName = "level" + i;
                }
            }
        }
    }
}