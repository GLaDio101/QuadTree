namespace Core.View.Tab
{
  public interface ITabButton
  {
    string Key { get; }

    void Remove();

    void Setup(string value, ITabButtonList list);

    void Activate();

    void Deactivate();
  }
}