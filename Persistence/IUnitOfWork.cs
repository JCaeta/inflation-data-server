namespace InflationDataServer.Persistence
{
    public interface IUnitOfWork
    {
        void connect();
        void disconnect();
    }
}
