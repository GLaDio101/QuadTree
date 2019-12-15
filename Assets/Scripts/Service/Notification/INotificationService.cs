namespace Service.Notification
{
    public interface INotificationService
    {
        void Cancel(int id);

        void Schedule(int id, int afterSeconds, string message);
    }
}