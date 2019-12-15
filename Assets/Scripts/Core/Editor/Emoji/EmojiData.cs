using System;

namespace Core.Editor.Emoji
{
  [Serializable]
  public class EmojiData
  {
    public string name;

    public string unified;

//    public List<int> unicodeList;
    public int unicode;
    public string non_qualified;
    public string docomo;
    public string au;
    public string softbank;
    public string google;
    public string image;
    public int sheet_x;
    public int sheet_y;
    public string short_name;
    public string[] short_names;
    public string text;
    public string texts;
    public string category;
    public int sort_order;
    public string added_in;
    public string has_img_apple;
    public string has_img_google;
    public string has_img_twitter;
    public string has_img_emojione;
    public string has_img_facebook;

    public string has_img_messenger;
//    public DataSet skin_variations;
  }
}