namespace MessageServer.Infrastructure;

public class CircuitBreaker
{
    public delegate CircuitBreaker CircuitBreakerFactory(TimeSpan timeOut);
    
    private readonly TimeSpan _timeout;
    private DateTime _lastFailure;
    private Exception? _lastException;
    private int _failureCount;

    public CircuitBreaker(TimeSpan timeout)
    {
        _timeout = timeout;
    }

    /// <summary>
    /// Detects failures and encapsulates the logic of preventing a failure from constantly recurring
    /// </summary>
    /// <param name="action">Callback to execute</param>
    /// <exception cref="Exception">Throws after times out</exception>
    public void Execute(Action action)
    {
        if (_failureCount > 3 && DateTime.UtcNow - _lastFailure < _timeout)
        {
            throw new Exception("Circuit breaker is open", _lastException);
        }

        try
        {
            action();
            _failureCount = 0;
        }
        catch (Exception? ex)
        {
            _lastFailure = DateTime.UtcNow;
            _lastException = ex;
            _failureCount++;
            throw;
        }
    }
}