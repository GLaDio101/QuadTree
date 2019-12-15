using System.IO;
using Assets.Plugins;
using I2.Loc;
using UnityEngine;

namespace Core.Utils
{
    public class ShareAppLink : MonoBehaviour
    {
        public string TextKey = "AppShareTitle";

        public string Link = "";

        public Texture2D Image;

        public void Share()
        {
            string imagePath = Application.persistentDataPath + "/screenshot.png";

            byte[] bytes = Image.EncodeToPNG();
            File.WriteAllBytes(imagePath, bytes);

            var shareBody = LocalizationManager.GetTranslation(TextKey) + " #" + Application.productName.ToLower() + "\n " + Link;
            NativeShare.Share(shareBody, imagePath, Link, shareBody);
        }

        public void ShareScore(string text)
        {
            string imagePath = Application.persistentDataPath + "/screenshot.png";

            byte[] bytes = Image.EncodeToPNG();
            File.WriteAllBytes(imagePath, bytes);

            var shareBody = text + " #" + Application.productName.ToLower() + "\n " + Link;
            NativeShare.Share(shareBody, imagePath, Link, shareBody);
        }
    }
}