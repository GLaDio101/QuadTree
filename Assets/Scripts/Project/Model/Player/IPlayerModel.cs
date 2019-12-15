using Core.Model;

namespace Project.Model.Player
{
  public interface IPlayerModel : IBasePlayerModel
  {
    string Token { get; set; }

    string FacebookToken { get; set; }
  }
}