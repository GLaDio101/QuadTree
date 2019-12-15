using UnityEngine;

namespace Service.Share
{
    public interface IShareService
    {
        void Message(string message);

        void Image(Texture2D image);

        void Link(string link);
    }
}
