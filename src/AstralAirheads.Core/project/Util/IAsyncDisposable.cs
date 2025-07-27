using System.Threading.Tasks;

namespace AstralAirheads.Util;

/// <summary>
/// Provides a mechanism for releasing unmanaged resources asynchronously.
/// </summary>
public interface IAsyncDisposable
{
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or
    /// resetting unmanaged resources asynchronously.
    /// </summary>
    Task DisposeAsync();
}
