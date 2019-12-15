using UnityEngine;

namespace Core.View.Menu
{
  public class PopupIconMenu : MonoBehaviour
  {
    public GameObject List;

    private void Start()
    {
      Close();
    }

    public void OnToggle()
    {
      List.SetActive(!List.activeSelf);
    }

    public void Close()
    {
      List.SetActive(false);
    }

    public void Open()
    {
      List.SetActive(true);
    }
  }
}