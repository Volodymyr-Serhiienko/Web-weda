using Microsoft.Data.Sqlite;

namespace MyDB
{
    public class DBconnect : IDisposable
	{
        private static DBconnect? _database;
        private readonly SqliteConnection connection = new("Data Source=AstroDB.db");
		private bool disposed = false;

		private DBconnect(){}
        
        public static DBconnect getDataBase()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            _database ??= new DBconnect();
            return _database;
        }
        
        public SqliteCommand DoSql(string sqlExpression)
        {
            connection.Open();
            return new SqliteCommand(sqlExpression, connection);
        }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					connection.Dispose();
				}

				disposed = true;
			}
		}
	}
}