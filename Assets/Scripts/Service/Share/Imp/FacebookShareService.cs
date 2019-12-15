/*using System;
using Assets.Scripts.Service.Authentication;
using Assets.Scripts.Service.Exceptions;
using Facebook.Unity;
using UnityEngine;

namespace Assets.Scripts.Service.Share.Imp
{
    public class FacebookShareService : IShareService
    {
        [Inject(ServiceType.Facebook)]
        public IAuthenticationService authService { get; set; }

        public void Message(string message)
        {
            if (!authService.Connected)
            {
                throw new UnauthorizedAccessException("You have to login facebook to share.");
            }

            FB.FeedShare(
                link: new Uri(GetAppUrl()),
                linkDescription: message,
                callback: OnShared
                );
        }

        public void Image(Texture2D image)
        {
           throw new NotSupportedException("Image sharing on facebook not implemented.");
        }

        public void Link(string link)
        {
            if (!authService.Connected)
            {
                throw new UnauthorizedAccessException("You have to login facebook to share.");
            }

            FB.ShareLink(
                new Uri(link),
                callback: OnShared
                );
        }

        private void OnShared(IShareResult result)
        {
            if (result.Cancelled)
                return;

            if (!string.IsNullOrEmpty(result.Error))
                throw new NotCompletedException("Facebook Share: " + result.Error);

            Debug.Log("Shared with id: " + result.PostId);
        }

        private string GetAppUrl()
        {
            return "http://play.google.com/store/apps/details?id=" + Application.identifier;
        }
    }
}*/