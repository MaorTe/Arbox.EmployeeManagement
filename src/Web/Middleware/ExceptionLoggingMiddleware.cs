namespace Web.Middleware;

public class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _logDir;

    public ExceptionLoggingMiddleware(RequestDelegate next) {
        _next = next;
        _logDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        Directory.CreateDirectory(_logDir);
    }

    public async Task Invoke(HttpContext context) {
        try {
            await _next(context);
        }
        catch (Exception ex) {
            // build a timestamped filename
            var logFile = Path.Combine(_logDir,
                $"log-{DateTime.UtcNow:yyyy-MM-dd}.json");

            var entry = new {
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path,
                Exception = ex.ToString()
            };

            // append as one JSON per line
            await File.AppendAllTextAsync(logFile, System.Text.Json.JsonSerializer.Serialize(entry) + Environment.NewLine);

            // rethrow so that UseExceptionHandler can pick it up
            throw;
        }
    }
}
