using System.Collections.Generic;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Service.Friends.Imp
{
    public class DummyFriendsService : IFriendsService
    {
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher { get; set; }

        public List<FriendVo> List { get; private set; }

        public void Load()
        {
            //var rawData = @"{""data"":[{""name"":""Tayfun D\u00f6ker"",""id"":""1282482718479347""}],""paging"":{""cursors"":{""before"":""QVFIUnJ5WnhSb0h3NXlHYUxKWW9GUnF2ejRodF9qVzN0VWZAvWnRtWXJlMDVwZATJfcHBzbjZASMUt6NmRWYTlLUlAxd3Nnc1BDd1NsY2hEeEpValE3UkF3TFFn"",""after"":""QVFIUnJ5WnhSb0h3NXlHYUxKWW9GUnF2ejRodF9qVzN0VWZAvWnRtWXJlMDVwZATJfcHBzbjZASMUt6NmRWYTlLUlAxd3Nnc1BDd1NsY2hEeEpValE3UkF3TFFn""}},""summary"":{""total_count"":1}}";
            List = new List<FriendVo>();
            for (int i = 0; i < 10; i++)
            {
                List.Add(new FriendVo()
                {
                    Username = GetRandom(new[] { "John", "Sally", "Jimmy", "Caroline" }),
                    State = UserState.Online,
                    Image = null,
                    IsFriend = true,
                    Id = Random.Range(10000, 100000).ToString()
                });
            }

            //List = new List<FriendVo>();
            //var dict = Json.Deserialize(rawData) as Dictionary<string, object>;
            //var friends = (List<object>)dict["data"];
            //Debug.Log(friends.Count);
            //foreach (var t in friends)
            //{
            //    var friendDict = ((Dictionary<string, object>)(t));
            //    var friendVo = new FriendVo()
            //    {
            //        Id = (string)friendDict["id"],
            //        Username = (string)friendDict["name"]
            //    };
            //    List.Add(friendVo);
            //}

            //Debug.Log(List.Count);

            dispatcher.Dispatch(FriendsEvent.DataReady);
        }

        private static string GetRandom(IList<string> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    }
}