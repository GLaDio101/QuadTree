/*using System.Collections.Generic;
using Assets.Scripts.Service.Authentication;
using Facebook.MiniJSON;
using Facebook.Unity;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Service.Friends.Imp
{
    public class FacebookFriendsService : IFriendsService
    {
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher { get; set; }

        public List<FriendVo> List { get; private set; }

        [Inject(ServiceType.Facebook)]
        public IAuthenticationService authService { get; set; }

        public void Load()
        {
            if (!FB.IsLoggedIn)
            {
                Debug.LogWarning("Not connected to Facebook!");
                return;
            }

            Debug.Log("Friends loading... " + authService.UserId);

            FB.API("/me/friends", HttpMethod.GET, OnFriendsLoaded);
        }

        private void OnFriendsLoaded(IGraphResult result)
        {
            if (!string.IsNullOrEmpty(result.Error))
            {
                Debug.LogWarning("Problem loading friends!" + result.Error);
                return;
            }

            List = new List<FriendVo>();
            var dict = Json.Deserialize(result.RawResult) as Dictionary<string, object>;
            var friends = (List<object>)dict["data"];
            foreach (var t in friends)
            {
                var friendDict = ((Dictionary<string, object>)(t));
                var friendVo = new FriendVo()
                {
                    Id = (string)friendDict["id"],
                    Username = (string)friendDict["name"],
                    //TODO: get 'friends_online_presence' permission and set 'online_presence' field IN ('active', 'idle')
                    State = UserState.Offline 
                };
                List.Add(friendVo);
            }

            dispatcher.Dispatch(FriendsEvent.DataReady);
        }
    }
}*/