using System.Collections.Generic;

namespace Service.Achievements
{
    public interface IAchievementsService
    {
        void Load();

        void ShowList();

        void Unlock(string achievementId);

        void Reveal(string achievementId);

        void Increment(string achievementId);

        List<AcvievementVo> List { get; } 
    }
}