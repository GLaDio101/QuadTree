using System.Collections.Generic;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Service.Achievements.Imp
{
    public class DummyAchievementsService:IAchievementsService
    {
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher { get; set; }

        public List<AcvievementVo> List { get; private set; }

        public void Load()
        {
            List = new List<AcvievementVo>();
            for (int i = 0; i < 5; i++)
            {
               List.Add(new AcvievementVo()
               {
                   Title = "Ach" + (i+1),
                   Percentage = Random.Range(0,100)
               });
            }

            dispatcher.Dispatch(AchievementsEvent.DataReady);
        }

        public void ShowList()
        {
            Debug.Log("Dummy acvievement list opened.");
        }

        public void Unlock(string achievementId)
        {
            Debug.Log("Dummy acvievement unlocked by id " + achievementId);
        }

        public void Reveal(string achievementId)
        {
            Debug.Log("Dummy acvievement revealed by id " + achievementId);
        }

        public void Increment(string achievementId)
        {
            Debug.Log("Dummy acvievement incremented by id " + achievementId);
        }
    }
}