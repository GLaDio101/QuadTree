using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Tools
{
    public class SpriteChecker
    {
        // This will create a Menu named "Support", with a sub-menu 
        // named "SpriteChecker", in the Editor menu bar.
        [MenuItem("Tools/SpriteChecker")]
        public static void CheckSpritesTagsAndBundles()
        {
            // Get all the GUIDs (identifiers in project) of the Sprites in the Project
            string[] guids = AssetDatabase.FindAssets("t:sprite");

            // Dictionary to store the tags and bundle names
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter ti = TextureImporter.GetAtPath(path) as TextureImporter;

                // If the tag is not in the dictionary, add it
                if (!dict.ContainsKey(ti.spritePackingTag))
                    dict.Add(ti.spritePackingTag, ti.assetBundleName);
                else
                    // If the tag is associated with another bundle name, show an error
                    if (dict[ti.spritePackingTag] != ti.assetBundleName)
                    Debug.LogWarning("Sprite : " + ti.assetPath + " should be packed in " + dict[ti.spritePackingTag]);
            }
        }

        [MenuItem("Tools/TextureChecker")]
        public static void CheckTexturesTagsAndBundles()
        {
            // Get all the GUIDs (identifiers in project) of the Sprites in the Project
            string[] guids = AssetDatabase.FindAssets("t:Texture");

            // Dictionary to store the tags and bundle names
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter ti = TextureImporter.GetAtPath(path) as TextureImporter;

                // If the tag is not in the dictionary, add it
                if (!dict.ContainsKey(ti.spritePackingTag))
                    dict.Add(ti.spritePackingTag, ti.assetBundleName);
                else
                    // If the tag is associated with another bundle name, show an error
                    if (dict[ti.spritePackingTag] != ti.assetBundleName)
                    Debug.LogWarning("Texture : " + ti.assetPath + " should be packed in " + dict[ti.spritePackingTag]);
            }
        }
    }
}