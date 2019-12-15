using System;

namespace Core.View.Tab
{
  public interface ITabButtonList
  {
    Action OnSelectedChanged { get; set; }

    string Selected { get; set; }

    void Add(string value);

    void Remove(string value);

    void DeselectAll();

    void Clear();
  }
}