using System;
using Core.Model.Vo;
using I2.Loc;
using UnityEngine;

namespace Project.Model.Player
{
  [Serializable]
  public class PlayerModel : IPlayerModel
  {
    public ISettingsVo Settings { get; set; }

    public UserVo User { get; set; }


    public string Token { get; set; }

    public string FacebookToken { get; set; }


    public PlayerModel()
    {
      Reset();
    }

    public void Reset()
    {
      string language = PlayerPrefs.GetString("language");
      Settings = new SettingsVo
      {
        Music = 1f,
        Volume = 1f,
        Effects = .8f,
        AdvancedVolume = true,
        Language = string.IsNullOrEmpty(language) ? LocalizationManager.CurrentLanguageCode : language
      };

      User = new UserVo();
    }
  }
}