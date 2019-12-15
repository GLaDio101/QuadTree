using System;

namespace Service.User
{
  [Serializable]
  public class UserProfile
  {
    public int UserId;

    public string UserName;

    public string PhotoUrl;

    public string ShareId;

    public int Level;
  }
}