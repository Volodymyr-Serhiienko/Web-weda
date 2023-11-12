using Microsoft.Data.Sqlite;

namespace WebApplication1.Services
{
	public interface ILogWriterService
	{
		Task WriteLogAsync(string message);
		Task WriteSqliteLogAsync(string message, SqliteException e);
        Task WriteExeptionLogAsync(string message, Exception e);
    }

	public class LogWriterService : ILogWriterService, IAsyncDisposable
	{
		private readonly TextWriter _textWriter;

		public LogWriterService(TextWriter textWriter)
		{
			_textWriter = textWriter;
		}

		public async Task WriteLogAsync(string message)
		{
			await _textWriter.WriteLineAsync($"{DateTime.UtcNow.AddHours(3)} - {message}.");
			await _textWriter.WriteLineAsync();
			await _textWriter.FlushAsync();
		}
		public async Task WriteSqliteLogAsync(string message, SqliteException e)
		{
			await _textWriter.WriteLineAsync($"{DateTime.UtcNow.AddHours(3)} - {message}. {e.GetType()}: {e.Message}.");
			await _textWriter.WriteLineAsync();
			await _textWriter.FlushAsync();
		}
        public async Task WriteExeptionLogAsync(string message, Exception e)
        {
            await _textWriter.WriteLineAsync($"{DateTime.UtcNow.AddHours(3)} - {message}. {e.GetType()}: {e.Message}.");
            await _textWriter.WriteLineAsync();
            await _textWriter.FlushAsync();
        }

        public async ValueTask DisposeAsync()
		{
			await _textWriter.DisposeAsync();
			GC.SuppressFinalize(this);
		}
	}
}