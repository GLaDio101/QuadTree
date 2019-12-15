/*using System.Collections.Generic;
using Assets.Scripts.Service.Authentication;
using GooglePlayGames;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Service.Friends.Imp
{
    public class GooglePlayFriendsService : IFriendsService
    {
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher { get; set; }

        private PlayGamesPlatform _service;

        public List<FriendVo> List { get; private set; }

        [Inject(ServiceType.GooglePlay)]
        public IAuthenticationService authService { get; set; }

        [PostConstruct]
        public void OnPostConstruct()
        {
            dispatcher.AddListener(AuthenticationEvent.LoggedIn, OnLoggedIn);
        }

        private void OnLoggedIn(IEvent payload)
        {
            if ((ServiceType)payload.data != ServiceType.GooglePlay)
                return;

            dispatcher.RemoveListener(AuthenticationEvent.LoggedIn, OnLoggedIn);
            if (PlayGamesPlatform.Instance.IsAuthenticated())
                _service = PlayGamesPlatform.Instance;
        }

        public void Load()
        {
            if (!authService.Connected)
            {
                Debug.LogWarning("Not connected to Google Play!");
                return;
            }

            Debug.Log("Friends loading " + _service.localUser.friends.Length);

            _service.LoadFriends(_service.localUser, OnFriendsLoaded);
        }

        private void OnFriendsLoaded(bool result)
        {
            if (!result)
            {
                Debug.LogWarning("Problem loading friends!");
                return;
            }

            
            Debug.Log("Friends loaded " + _service.GetFriends().Length);

            List = new List<FriendVo>();
            foreach (IUserProfile userProfile in _service.GetFriends())
            {
                List.Add(new FriendVo()
                {
                    Username = userProfile.userName,
                    State = userProfile.state,
                    Image = userProfile.image,
                    IsFriend = userProfile.isFriend,
                    Id = userProfile.id
                });
            }

            dispatcher.Dispatch(FriendsEvent.DataReady);
        }
    }
}*/