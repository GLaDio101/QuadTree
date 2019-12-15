using Core.Model.Vo;

namespace Core.Model
{
    public interface IBasePlayerModel
    {
        ISettingsVo Settings { get; set; }

        UserVo User { get; set; }

        void Reset();
    }
}