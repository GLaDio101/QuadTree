using System.Collections.Generic;

namespace Core.Manager.Screen
{
    public interface IScreenModel
    {
        List<string> IgnoreHistory { get; set; }

        List<IPanelVo> History { get; set; }

        string GetHistoryData();
    }
}