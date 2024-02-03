using Microsoft.Data.Sqlite;

namespace Yakify.TestBase;

public abstract class SqliteTests : IDisposable
{
    private bool disposedValue;

    protected SqliteTests()
    {
        Connection = new SqliteConnection($"DataSource={Guid.NewGuid()};mode=memory;cache=shared");
        Connection.Open();
    }

    protected readonly SqliteConnection Connection;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Connection.Close();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
