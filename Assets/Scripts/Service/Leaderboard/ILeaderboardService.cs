using System.Collections.Generic;

namespace Service.Leaderboard
{
    public interface ILeaderboardService
    {
        int ItemCount { get; set; }

        List<LeaderboardVo> List { get; }

        void LoadBoardDataById(string boardId);

        void LoadMore(bool up);

        void ShowBoardList();

        void ShowBoardList(string boardid);

        void ShowBoardById(string boardId);

        void PostScore(int score, string boardId);
    }
}