namespace Core.Editor.Tools
{
  public class GetCharSets : UnityEditor.Editor
  {
//    [MenuItem("Tools/Extra/Get Character Sets")]
//    [UsedImplicitly]
//    private static void ClearNameNumbersOperation()
//    {
//      LanguageManager manager = LanguageManager.Instance;
//      manager.runInEditMode = true;
//      List<SmartCultureInfo> languages = manager.GetSupportedLanguages();
//      List<string> charSets = new List<string>();
//      foreach (SmartCultureInfo language in languages)
//      {
//        string charSet = "";
//        manager.ChangeLanguage(language);
//        List<string> allKeys = manager.GetAllKeys();
//
//        foreach (string key in allKeys)
//        {
//          string textValue = manager.GetTextValue(key);
//
//          foreach (char c in textValue)
//          {
//            if (!charSet.Contains(c.ToString()))
//            {
//              charSet += c.ToString();
//            }
//          }
//        }
//
//        Debug.Log(language.languageCode);
//        Debug.Log(new string(charSet.ToCharArray().OrderBy(c => c).ToArray()));
//        charSets.Add(charSet);
//      }
//    }
  }
}