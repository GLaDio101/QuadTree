/*using System;
using System.Collections.Generic;
using Assets.Scripts.Service.Authentication;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Service.Achievements.Imp
{
    public class GooglePlayAchievementsService : IAchievementsService
    {
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher { get; set; }

        private PlayGamesPlatform _service;

        public List<AcvievementVo> List { get; private set; }

        private Dictionary<string, AcvievementVo> _map;

        private bool _isDirty = true;

        [Inject(ServiceType.GooglePlay)]
        public IAuthenticationService authService { get; set; }

        [PostConstruct]
        public void OnPostConstruct()
        {
            dispatcher.AddListener(AuthenticationEvent.LoggedIn, OnLoggedAchIn);
        }

        private void OnLoggedAchIn(IEvent payload)
        {
            if ((ServiceType)payload.data != ServiceType.GooglePlay)
                return;

            dispatcher.RemoveListener(AuthenticationEvent.LoggedIn, OnLoggedAchIn);
            if (PlayGamesPlatform.Instance.IsAuthenticated())
                _service = PlayGamesPlatform.Instance;
        }

        public void Load()
        {
            if (!authService.Connected)
            {
                Debug.LogWarning("Not connected to Google Play!");
                authService.Login();
                return;
            }

            if (_map != null && _isDirty)
            {
                Reload();
                return;
            }

            _map = new Dictionary<string, AcvievementVo>();

            _service.LoadAchievementDescriptions(OnAchievementsDescriptionsLoaded);
        }

        private void OnAchievementsDescriptionsLoaded(IAchievementDescription[] list)
        {
            foreach (IAchievementDescription ad in list)
            {
                _map.Add(ad.id, new AcvievementVo()
                {
                    Id = ad.id,
                    Title = ad.title,
                    Description = ad.unachievedDescription,
                    Points = ad.points,
                    IsHidden = ad.hidden,
                    Image = ad.image
                });
            }

            Reload();
        }

        private void Reload()
        {
            if (!_isDirty)
                return;

            _service.LoadAchievements(OnAchievementsLoaded);
        }

        private void OnAchievementsLoaded(IAchievement[] list)
        {
            List = new List<AcvievementVo>();
            foreach (IAchievement achievement in list)
            {
                var avo = _map[achievement.id];

                if (!achievement.hidden)
                {
                    avo.Percentage = achievement.percentCompleted;
                    avo.Date = achievement.lastReportedDate;
                    List.Add(avo);
                }
            }

            _isDirty = false;

            dispatcher.Dispatch(AchievementsEvent.DataReady);
        }

        public void Unlock(string achievementId)
        {
            if (!authService.Connected)
            {
                Debug.LogWarning("Not connected to Google Play!");
                return;
            }

            if(_service == null)
            {
                if (PlayGamesPlatform.Instance.IsAuthenticated())
                    _service = PlayGamesPlatform.Instance;
            }

            _service.ReportProgress(achievementId, 100.0f, OnUnlocked);
        }

        private void OnUnlocked(bool result)
        {
            if (!result)
            {
                Debug.LogWarning("Problem unlocking achievement.");
                return;
            }

            dispatcher.Dispatch(AchievementsEvent.Unlocked);
            _isDirty = true;
        }

        public void Reveal(string achievementId)
        {
            if (!authService.Connected)
            {
                Debug.LogWarning("Not connected to Google Play!");
                return;
            }

            if (_service == null)
            {
                if (PlayGamesPlatform.Instance.IsAuthenticated())
                    _service = PlayGamesPlatform.Instance;
            }

            _service.ReportProgress(achievementId, 0.0f, OnRevealed);
        }

        private void OnRevealed(bool result)
        {
            if (!result)
            {
                Debug.LogWarning("Problem revealing achievement.");
                return;
            }

            _isDirty = true;
        }

        public void Increment(string achievementId)
        {
            if (!authService.Connected)
            {
                Debug.LogWarning("Not connected to Google Play!");
                return;
            }

            if (_service == null)
            {
                if (PlayGamesPlatform.Instance.IsAuthenticated())
                    _service = PlayGamesPlatform.Instance;
            }

            _service.IncrementAchievement(achievementId, 1, OnIncremented);
        }

        private void OnIncremented(bool result)
        {
            if (!result)
            {
                Debug.LogWarning("Problem incrementing achievement.");
                return;
            }

            _isDirty = true;
        }

        public void ShowList()
        {
            if (!authService.Connected)
            {
                Debug.LogWarning("Not connected to Google Play!");
                authService.Login();
                return;
            }

            _service.ShowAchievementsUI(OnClosed);
        }

        private void OnClosed(UIStatus status)
        {
            Debug.Log(status);
        }
    }
}*/