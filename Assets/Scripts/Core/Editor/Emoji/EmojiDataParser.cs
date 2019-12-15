using System.Collections.Generic;
using Core.Editor.Code;
using JetBrains.Annotations;
using SimpleJSON;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Emoji
{
  public class EmojiDataParser : UnityEditor.Editor
  {
    public const string Root = "Assets/Assets/Fonts/Resources/Sprite Assets/";

    [MenuItem("Tools/Parse Emoji Data")] // % – CTRL | # – Shift | & – Alt | _ - for single
    [UsedImplicitly]
    private static void Parse()
    {
      TMP_SpriteAsset spriteAsset = AssetDatabase.LoadAssetAtPath<TMP_SpriteAsset>(Root + "sheet_emojione_64.asset");

      spriteAsset.spriteInfoList.Sort(delegate(TMP_Sprite a, TMP_Sprite b)
      {
        int result = -a.x.CompareTo(b.x);
        if (result == 0)
        {
          return a.y.CompareTo(b.y);
        }

        return result;
      });
      spriteAsset.spriteInfoList.Reverse();

      var trooperData = CodeUtilities.LoadScript(Root + "emojione_64_data.json");

//      EmojiDataList data = JsonConvert.DeserializeObject<EmojiDataList>(trooperData);

      List<EmojiData> fullList = new List<EmojiData>();

      JSONNode jsonNode = JSON.Parse(trooperData);

      var node = jsonNode["list"];
      Debug.Log(node.AsArray.Count);

      for (int i = 0; i < node.AsArray.Count; i++)
      {
        var nodeAs = node.AsArray[i];
        var emojiData = new EmojiData
        {
          unified = nodeAs["unified"],
          short_name = nodeAs["short_name"]
        };
        fullList.Add(emojiData);

        string[] strings = emojiData.unified.Split('-');
        emojiData.unicode = int.Parse(strings[0], System.Globalization.NumberStyles.HexNumber);

        var skin_variations = nodeAs["skin_variations"];

        foreach (JSONNode child in skin_variations.Children)
        {
          var item = new EmojiData
          {
            unified = child["unified"],
            short_name = nodeAs["short_name"]
          };

          string[] strings2 = item.unified.Split('-');
          item.unicode = int.Parse(strings2[1], System.Globalization.NumberStyles.HexNumber);

          fullList.Add(item);
        }
      }

//      foreach (EmojiData emojiData in data.list)
//      {
//        fullList.Add(emojiData);

//        if (emojiData.skin_variations != null)
//        {
//          Debug.Log(emojiData.skin_variations);
////          foreach (EmojiData variation in emojiData.skin_variations.Values)
////          {
////            fullList.Add(variation);
////          }
//        }
//      }

//      data.list.Sort((x, y) => x.sort_order.CompareTo(y.sort_order));

//      foreach (EmojiData emojiData in fullList)
//      {
////        if (emojiData.short_name == null)
////        {
////          Debug.Log("missing short_name : " + emojiData.unified);
////        }
////
////        if (emojiData.name == null)
////        {
////          Debug.Log("missing name : " + emojiData.unified);
////        }
//
////        Debug.Log(emojiData.sort_order + " - " + emojiData.short_name);
//        string[] strings = emojiData.unified.Split('-');
//        emojiData.unicodeList = new List<int>();
//        foreach (string s in strings)
//        {
//          emojiData.unicodeList.Add(int.Parse(s, System.Globalization.NumberStyles.HexNumber));
//        }
//      }

//      return;


      Debug.Log(spriteAsset.spriteInfoList.Count);
//      Debug.Log(data.list.Count);
      Debug.Log(fullList.Count);

//      return;

//      for (var i = 0; i < spriteAsset.spriteInfoList.Count; i++)
//      {
//        TMP_Sprite tmpSprite = spriteAsset.spriteInfoList[i];
//        if (data.list.Count <= i)
//          continue;
//        EmojiData emojiData = data.list[i];
//        tmpSprite.name = emojiData.short_name;
//        if (emojiData.unicodeList.Count > 0)
//          tmpSprite.unicode = emojiData.unicodeList[0];
//      }

      for (var i = 0; i < fullList.Count; i++)
      {
        TMP_Sprite tmpSprite = spriteAsset.spriteInfoList[i];
        if (fullList.Count <= i)
          continue;
        EmojiData emojiData = fullList[i];
        tmpSprite.name = emojiData.short_name;
//        if (emojiData.unicodeList.Count > 0)
        tmpSprite.unicode = emojiData.unicode;
      }

      AssetDatabase.SaveAssets();
    }
  }
}