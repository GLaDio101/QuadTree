namespace Service.NetConnection
{
    public interface INetConnectionService
    {
        void Init(bool auto = false);

        void Check();

        bool Auto { get; set; }

        bool Available { get; }

        NetConnectionStatus Status { get; }
    }
}