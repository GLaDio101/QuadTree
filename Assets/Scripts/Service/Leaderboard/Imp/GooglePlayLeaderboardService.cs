#if UNITY_ANDROID
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Service.Authentication.Events;
using Service.Authentication.Providers;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Service.Leaderboard.Imp
{
  public class GooglePlayLeaderboardService : ILeaderboardService
  {
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    private PlayGamesPlatform _service;

    private LeaderboardScoreData _lastLoadedBoard;

    private Dictionary<string, IScore> _scoreMap;

    private int _backupScore;

    public int ItemCount { get; set; }

    public List<LeaderboardVo> List { get; private set; }

    [Inject(ServiceType.GooglePlay)]
    public IAuthenticationProvider authService { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      ItemCount = 100;
      dispatcher.AddListener(AuthenticationProviderEvent.LoggedIn, OnLoggedLeaderIn);
    }

    private void OnLoggedLeaderIn(IEvent payload)
    {
      if ((ServiceType) payload.data != ServiceType.GooglePlay)
        return;

      dispatcher.RemoveListener(AuthenticationProviderEvent.LoggedIn, OnLoggedLeaderIn);
      if (PlayGamesPlatform.Instance.IsAuthenticated())
        _service = PlayGamesPlatform.Instance;
    }

    public void LoadBoardDataById(string boardId)
    {
      if (!authService.Connected)
      {
        Debug.LogWarning("Not connected to Google Play!");
        authService.Login();
        return;
      }

      List = new List<LeaderboardVo>();
      _service.LoadScores(boardId, LeaderboardStart.PlayerCentered, ItemCount, LeaderboardCollection.Public,
        LeaderboardTimeSpan.AllTime, OnLeaderboardDataLoaded);
    }

    public void LoadMore(bool up)
    {
      if (!authService.Connected)
      {
        Debug.LogWarning("Not connected to Google Play!");
        return;
      }

      if (_lastLoadedBoard == null)
      {
        Debug.LogWarning("No board loaded. First LoadBoardDataById to load more.");
        return;
      }

      _service.LoadMoreScores(up ? _lastLoadedBoard.PrevPageToken : _lastLoadedBoard.NextPageToken, ItemCount,
        OnLeaderboardDataLoaded);
    }

    private void OnLeaderboardDataLoaded(LeaderboardScoreData data)
    {
      if (data.Valid)
      {
        _lastLoadedBoard = data;
        _scoreMap = new Dictionary<string, IScore>();
        var userIds = new List<string>();
        foreach (var score in data.Scores)
        {
          _scoreMap.Add(score.userID, score);
          userIds.Add(score.userID);
        }

        Debug.Log(userIds.Count);
        _service.LoadUsers(userIds.ToArray(), OnUsersLoaded);
      }
    }

    private void OnUsersLoaded(IUserProfile[] userList)
    {
      foreach (var user in userList)
      {
        List.Add(new LeaderboardVo()
        {
          Index = _scoreMap[user.id].rank,
          Username = user.userName,
          Point = _scoreMap[user.id].formattedValue,
          Image = user.image,
          IsFriend = user.isFriend,
          State = user.state,
          Date = _scoreMap[user.id].date
        });
      }

      dispatcher.Dispatch(LeaderboardEvent.DataReady);
    }

    public void ShowBoardList(string boardid)
    {
      if (!authService.Connected)
      {
        Debug.LogWarning("Not connected to Google Play!");
        authService.Login();
        return;
      }

      _service.ShowLeaderboardUI(boardid);
    }

    public void ShowBoardList()
    {
      if (!authService.Connected)
      {
        Debug.LogWarning("Not connected to Google Play!");
        authService.Login();
        return;
      }

      _service.ShowLeaderboardUI();
    }

    public void ShowBoardById(string boardId)
    {
      if (!authService.Connected)
      {
        Debug.LogWarning("Not connected to Google Play!");
        authService.Login();
        return;
      }

      _service.ShowLeaderboardUI(boardId);
    }

    public void PostScore(int score, string boardId)
    {
      if (!PlayGamesPlatform.Instance.IsAuthenticated())
      {
        Debug.LogWarning("Not connected to Google Play!");
        return;
      }

      _backupScore = score;

      if (PlayGamesPlatform.Instance != null)
        PlayGamesPlatform.Instance.ReportScore(score, boardId, OnScorePosted);
    }

    private void OnScorePosted(bool result)
    {
      //TODO: deal with any problem encountered when posting score
      if (!result)
      {
        Debug.LogError("Problem posting score! (Score: " + _backupScore + ")");
      }
    }
  }
}
#endif