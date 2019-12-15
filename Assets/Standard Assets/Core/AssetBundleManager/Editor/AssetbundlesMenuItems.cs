using UnityEditor;

namespace Standard_Assets.Core.AssetBundleManager.Editor
{
	public class AssetBundlesMenuItems
	{
		const string kSimulationMode = "Assets/AssetBundles/Simulation Mode";
	
		[MenuItem(kSimulationMode)]
		public static void ToggleSimulationMode ()
		{
			AssetBundles.AssetBundleManager.SimulateAssetBundleInEditor = !AssetBundles.AssetBundleManager.SimulateAssetBundleInEditor;
		}
	
		[MenuItem(kSimulationMode, true)]
		public static bool ToggleSimulationModeValidate ()
		{
			Menu.SetChecked(kSimulationMode, AssetBundles.AssetBundleManager.SimulateAssetBundleInEditor);
			return true;
		}
		
		[MenuItem ("Assets/AssetBundles/Build AssetBundles")]
		static public void BuildAssetBundles ()
		{
			BuildScript.BuildAssetBundles();

			BuildScript.AddExtension();
		}
	}
}