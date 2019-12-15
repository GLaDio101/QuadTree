#if !UNITY_EDITOR_WIN
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEditor.iOS.Xcode;

namespace Assets.Editor
{
/*
    public class IosBuildPostProcessor
    {

		private static PBXProject _project;

		private static string _path;
		private static string _projectPath;
		private static string _target;

		private static void OpenProject ()
		{
			_projectPath = _path + "/Unity-iPhone.xcodeproj/project.pbxproj";

			_project = new PBXProject ();
			_project.ReadFromFile (_projectPath);

			_target = _project.TargetGuidByName ("Unity-iPhone");
		}

		private static void CloseProject ()
		{
			File.WriteAllText (_projectPath, _project.WriteToString ());
		}

		private static void CopyAndReplaceDirectory (string srcPath, string dstPath)
		{
			if (Directory.Exists (dstPath))
				return;
//			if (Directory.Exists (dstPath))
//				Directory.Delete (dstPath);
//			if (File.Exists (dstPath))
//				File.Delete (dstPath);

			Directory.CreateDirectory (dstPath);

			foreach (var file in Directory.GetFiles(srcPath))
				File.Copy (file, Path.Combine (dstPath, Path.GetFileName (file)));

			foreach (var dir in Directory.GetDirectories(srcPath))
				CopyAndReplaceDirectory (dir, Path.Combine (dstPath, Path.GetFileName (dir)));
		}

		private static void AddFramework (string framework)
		{
			//if (_project.HasFramework (framework))
				//return;

			_project.AddFrameworkToProject (_target, framework, false);
		}

		private static void AddExternalFramework (string framework)
		{
			var unityPath = "/../Frameworks/" + framework;
			var fullUnityPath = Application.dataPath + unityPath;

			var frameworkPath = "Frameworks/" + framework;
			var fullFrameworkPath = Path.Combine (_path, frameworkPath);

			CopyAndReplaceDirectory (fullUnityPath, fullFrameworkPath);

			var frameworkFileGuid = _project.AddFile (frameworkPath, frameworkPath, PBXSourceTree.Source);
			_project.AddFileToBuild (_target, frameworkFileGuid);
			AddFramework (framework);
		}

		private static void SetBuildProperties ()
		{
			_project.SetBuildProperty (_target, "DEVELOPMENT_TEAM", "ZF59YUWT2D");
			_project.SetBuildProperty (_target, "COMPRESS_PNG_FILES", "NO");
			_project.SetBuildProperty (_target, "DEBUG_INFORMATION_FORMAT", "dwarf");
			_project.SetBuildProperty (_target, "ENABLE_BITCODE", "NO");
			_project.SetBuildProperty (_target, "ARCHS", "$(ARCHS_STANDARD)");
			_project.SetBuildProperty (_target, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
			_project.SetBuildProperty (_target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
			_project.AddBuildProperty (_target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks");
			_project.AddBuildProperty (_target, "OTHER_LDFLAGS", "-ObjC");
			_project.AddBuildProperty (_target, "OTHER_LDFLAGS", "-fobjc-arc");
		}

		private static void AddCapability (string name)
		{
			string infoPlistPath = _path + "/Info.plist";

			var plistParser = new PlistDocument ();
			plistParser.ReadFromFile (infoPlistPath);
			plistParser.root ["UIRequiredDeviceCapabilities"].AsArray ().AddString (name);

			plistParser.WriteToFile (infoPlistPath);
		}

//		private static void RemoveCapability (string name)
//		{
//			string infoPlistPath = _path + "/Info.plist";
//
//			var plistParser = new PlistDocument ();
//			plistParser.ReadFromFile (infoPlistPath);
//			plistParser.root ["UIRequiredDeviceCapabilities"].AsArray ().
//
//			plistParser.WriteToFile (infoPlistPath);
//		}

		private static void DisableArcOnFile (string guid)
		{
			_project.RemoveFileFromBuild (_target, guid);
			_project.AddFileToBuildWithFlags (_target, guid, "-fno-objc-arc");
		}

		private static void DisableArcOnFileByProjectPath (string file)
		{
			var guid = _project.FindFileGuidByProjectPath (file);
			DisableArcOnFile (guid);
		}

		private static void DisableArcOnFileByRealPath (string file)
		{
			var guid = _project.FindFileGuidByRealPath (file);

			if (guid == null) {
				guid = _project.AddFile (file, file);
			}

			DisableArcOnFile (guid);
		}

		private static void SetPlistKeys(){
			string plistPath = _path + "/Info.plist";
			PlistDocument plist = new PlistDocument ();
			plist.ReadFromString (File.ReadAllText (plistPath));

			PlistElementDict rootDict = plist.root;
			rootDict.SetBoolean ("ITSAppUsesNonExemptEncryption", false);
			rootDict.SetString ("NSCameraUsageDescription", "$(PRODUCT_NAME) camera use for ads.");

			File.WriteAllText (plistPath, plist.WriteToString ());
		}

		private static void DisableArcOnFiles ()
		{
//			DisableArcOnFileByProjectPath ("Libraries/Plugins/iOS/GADUObjectCache.m");
//			DisableArcOnFileByProjectPath ("Libraries/Plugins/iOS/GADUInterstitial.m");
//			DisableArcOnFileByProjectPath ("Libraries/Plugins/iOS/GADURequest.m");
//			DisableArcOnFileByProjectPath ("Libraries/Plugins/iOS/GADUInterface.m");
//			DisableArcOnFileByProjectPath ("Libraries/Plugins/iOS/GADUBanner.m");
//
//			DisableArcOnFileByRealPath (Application.dataPath + "/Facebook/Editor/iOS/FbUnityInterface.mm");
		}

		private static void AddFrameworks ()
		{
//			AddFramework ("CoreData.framework");
//			AddFramework ("MediaPlayer.framework");
//			AddFramework ("Security.framework");
//			
//			AddFramework ("libxml2.2.dylib");
			//AddExternalFramework ("GoogleMobileAds.framework");
			//AddExternalFramework ("AdColony.framework");
		}

		//[PostProcessBuild]
		public static void OnPostprocessBuild (UnityEditor.BuildTarget buildTarget, string path)
		{
		    if (buildTarget == BuildTarget.iOS)
		    {
		        _path = path;

		        //AddCapability ("gamekit");
		        SetPlistKeys();

		        OpenProject();
		        AddFrameworks();
		        SetBuildProperties();
		        DisableArcOnFiles();
		        CloseProject();
		        Debug.Log("Xcode project configured.");
		    }
		}
    }
    */
}
#endif
