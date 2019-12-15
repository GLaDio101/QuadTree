namespace Service.Notification.Imp
{
    public class DummyNotificationService:INotificationService
    {
        public void Cancel(int id)
        {
        }

        public void Schedule(int id, int afterSeconds, string message)
        {
        }
    }
}