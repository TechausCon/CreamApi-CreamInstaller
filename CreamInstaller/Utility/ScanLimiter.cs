using System;
using System.Threading;
using System.Threading.Tasks;

namespace CreamInstaller.Utility;

/// <summary>
/// Bounds parallel scan work (game + DLC queries) so store/SteamCMD fan-out cannot unboundedly explode.
/// </summary>
internal static class ScanLimiter
{
    internal const int MaxConcurrentGameScans = 8;
    internal const int MaxConcurrentDlcQueries = 12;

    private static readonly SemaphoreSlim GameSlots = new(MaxConcurrentGameScans, MaxConcurrentGameScans);
    private static readonly SemaphoreSlim DlcSlots = new(MaxConcurrentDlcQueries, MaxConcurrentDlcQueries);

    internal static async Task RunGameAsync(Func<Task> work, CancellationToken cancellationToken = default)
    {
        await GameSlots.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await work().ConfigureAwait(false);
        }
        finally
        {
            _ = GameSlots.Release();
        }
    }

    internal static async Task RunDlcAsync(Func<Task> work, CancellationToken cancellationToken = default)
    {
        await DlcSlots.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await work().ConfigureAwait(false);
        }
        finally
        {
            _ = DlcSlots.Release();
        }
    }
}
