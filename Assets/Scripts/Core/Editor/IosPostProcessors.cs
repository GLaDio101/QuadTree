#if UNITY_IOS && !UNITY_EDITOR
using System.IO;
using GooglePlayGames.xcode;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Assets.Scripts.Project.Editor
{
    public class IosPostProcessors
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
                PBXProject proj = new PBXProject();
                proj.ReadFromString(File.ReadAllText(projPath));

                string nativeTarget = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                string testTarget = proj.TargetGuidByName(PBXProject.GetUnityTestTargetName());
                string[] buildTargets = new string[] { nativeTarget, testTarget };

                proj.SetBuildProperty(buildTargets, "ENABLE_BITCODE", "YES");
                File.WriteAllText(projPath, proj.WriteToString());
            }
        }
    }
}
#endif