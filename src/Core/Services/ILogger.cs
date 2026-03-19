namespace Shiron.HonamiStack.Core.Services;

public interface ILogger {
    void Log(string message);
    void LogMarkup(string message);
    void Debug(string message);

    void Error(string message);
    void Exception(Exception exception);
}
