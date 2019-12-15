using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using UnityEngine.Networking;

namespace Service.DailyReward
{
  public class DailyRewardService : IDailyRewardService
  {
    // service link for timestamp
    private const string Url = "https://currentmillis.com/time/seconds-since-unix-epoch.php";

    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; }

    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    public RewardUserData UserData { get; private set; }

    private List<DailyRewardVo> List { get; set; }

    private int _currentTime;

    private bool _inited;

    private bool _dirty;

    private MonoBehaviour _root;

    [PostConstruct]
    public void OnPostConstruct()
    {
      List = new List<DailyRewardVo>();
    }

    public void AddReward(DailyRewardVo dailyRewardVo)
    {
      List.Add(dailyRewardVo);
    }

    public bool IsRewardReady
    {
      get
      {
        if (UserData == null)
          return false;

        if (UserData.SuccessiveSessionCount == 0)
          return true;

        return (24 * 60 * 60 - (_currentTime - UserData.LastSessionTime)) <= 0;
      }
    }

    public bool IsRewardMissed
    {
      get { return (2 * 24 * 60 * 60 - (_currentTime - UserData.LastSessionTime)) <= 0; }
    }

    public int RemainingTime
    {
      get
      {
        if (IsRewardReady)
          return 0;

        if (UserData == null)
          return 0;

        return 24 * 60 * 60 - (_currentTime - UserData.LastSessionTime);
      }
    }

    public int CurrentTime
    {
      get { return _currentTime; }
    }

#if UNITY_EDITOR
    public void PassADay()
    {
      _currentTime += 24 * 60 * 60;

      CheckStatus();
    }
#endif

    public DailyRewardVo GetReward()
    {
      if (UserData.SuccessiveSessionCount >= List.Count)
      {
        throw new InvalidOperationException("All rewards collected.");
      }

      UserData.SuccessiveSessionCount++;
      UserData.LastSessionTime = _currentTime;

      dispatcher.Dispatch(DailyRewardEvent.DataUpdated);

      return List[UserData.SuccessiveSessionCount - 1];
    }

    public void Init(RewardUserData userData)
    {
      if (_inited)
        return;

      UserData = userData;
      _root = contextView.GetComponent<ContextView>();
      _root.StartCoroutine(GetEpoch());
    }

    private int GetTime()
    {
      var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      return (int) (DateTime.UtcNow - epochStart).TotalSeconds;
    }

    private IEnumerator GetEpoch()
    {
      UnityWebRequest www = new UnityWebRequest(Url);
      yield return www;

      if (www.error != null)
      {
        dispatcher.Dispatch(DailyRewardEvent.Disabled);
        yield break;
      }

      int result = GetTime();
      if (int.TryParse(www.downloadHandler.text, out result))
      {
        _currentTime = result;
        _inited = true;
        _root.StartCoroutine(TimeTick());

        CheckStatus();
      }
    }

    private void CheckStatus()
    {
      if (IsRewardMissed)
      {
        UserData.LastSessionTime = 0;
        UserData.SuccessiveSessionCount = 0;
        _dirty = true;
      }

      if (IsRewardReady)
      {
        if (UserData.SuccessiveSessionCount == List.Count)
        {
          UserData.SuccessiveSessionCount = 0;
          _dirty = true;
        }
      }

      if (_dirty)
      {
        _dirty = false;
        dispatcher.Dispatch(DailyRewardEvent.DataUpdated);
      }
    }

    private IEnumerator TimeTick()
    {
      yield return new WaitForSecondsRealtime(1f);
      _currentTime++;

      CheckStatus();

      _root.StartCoroutine(TimeTick());
    }
  }
}