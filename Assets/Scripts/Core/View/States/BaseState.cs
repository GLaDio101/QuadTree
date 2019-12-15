namespace Core.View.States
{
  public abstract class BaseState
  {
    public bool needUpdate;

    protected StateView view;

    public string Key { get; private set; }

    public BaseState(string key)
    {
      Key = key;
    }

    internal void setView(StateView v)
    {
      view = v;
      onInitialized();
    }


    /// <summary>
    /// called directly after the machine and context are set allowing the state to do any required setup
    /// </summary>
    public virtual void onInitialized()
    {
    }


    public virtual void begin()
    {
    }


    public virtual void reason()
    {
    }


    public abstract void update(float deltaTime);


    public virtual void end()
    {
    }
  }
}