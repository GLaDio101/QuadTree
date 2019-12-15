namespace Core.Manager.Save
{
    public interface ISaveManager
    {
        void Login();

        void Save();

        void Reset();

        TModel GetData<TModel>();
    }
}