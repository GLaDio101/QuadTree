using Assets.Plugins;
using UnityEngine;

namespace Service.Share.Imp
{
    public class NativeShareService : IShareService
    {
        public void Message(string message)
        {
            NativeShare.Share(message, "", GetAppUrl(), Application.productName);
        }

        public void Image(Texture2D image)
        {
            string imagePath = Application.persistentDataPath + "/screenshot.png";

            byte[] bytes = image.EncodeToPNG();
            System.IO.File.WriteAllBytes(imagePath, bytes);

            NativeShare.Share("", imagePath, GetAppUrl(), Application.productName);
        }

        public void Link(string link)
        {
            NativeShare.Share("", "", link, Application.productName);
        }

        private string GetAppUrl()
        {
            return "market://details?id=" + Application.identifier;
        }
    }
}
