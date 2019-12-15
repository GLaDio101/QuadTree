namespace Core.Manager.Pool
{
    public interface IPoolable
    {
        void Wake();

        void Sleep();
    }
}