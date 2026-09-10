using System;
using System.Threading.Tasks;
using CreamInstaller.Platforms.Steam;

namespace CreamInstaller.Forms;

internal sealed partial class MainForm
{
    private const int SteamCmdTimeoutMs = 16000;
    private const string DlcRefreshLogPrefix = "[DLCRefresh] ";

    private static async Task<T> WithTimeout<T>(Task<T> task, int millisecondsTimeout)
    {
        if (await Task.WhenAny(task, Task.Delay(millisecondsTimeout)) == task)
            return await task;
        return default;
    }

    private static async Task<string> ResolveSteamDlcName(string dlcId, string parentGameName = null,
        string parentGameId = null)
    {
        StoreAppData dlcStore = await SteamStore.QueryStoreAPI(dlcId, isDlc: true, attempts: 0, parentGameName,
            parentGameId);
        if (dlcStore?.Name is not null)
            return dlcStore.Name;
        CmdAppData dlcCmd = await SteamCMD.GetAppInfo(dlcId);
        return dlcCmd?.Common?.Name ?? "Unknown";
    }
}
