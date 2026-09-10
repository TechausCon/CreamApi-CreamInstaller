using System.IO;
using CreamInstaller.Forms;
using CreamInstaller.Utility;
using static CreamInstaller.Resources.Resources;

namespace CreamInstaller.Resources;

/// <summary>
/// Shared backup/restore/write pipeline for Steamworks steam_api*.dll unlocker installs.
/// </summary>
internal static class SteamworksDllInstaller
{
    internal static void BackupOriginalIfNeeded(string apiPath, string backupPath, InstallForm installForm, string logTag)
    {
        if (!apiPath.FileExists() || backupPath.FileExists())
            return;

        apiPath.MoveFile(backupPath!, true);
        installForm?.UpdateUser(
            $"Renamed Steamworks: {Path.GetFileName(apiPath)} -> {Path.GetFileName(backupPath)}",
            LogTextBox.Action, false);
        ProgramData.Log.Info($"[{logTag}] Backed up {Path.GetFileName(apiPath)} -> {Path.GetFileName(backupPath)}",
            LogDestination.Unlocker);
    }

    internal static void WriteResourceIfBackupExists(string resourceName, string apiPath, string backupPath,
        InstallForm installForm, string logTag, string productLabel)
    {
        if (!backupPath.FileExists())
            return;

        resourceName.WriteManifestResource(apiPath);
        installForm?.UpdateUser($"Wrote {productLabel}: {Path.GetFileName(apiPath)}", LogTextBox.Action, false);
        ProgramData.Log.Info($"[{logTag}] Wrote {productLabel} DLL to: {apiPath}", LogDestination.Unlocker);
    }

    internal static void RestoreOriginal(string apiPath, string backupPath, InstallForm installForm, string logTag,
        string productLabel)
    {
        if (!backupPath.FileExists())
            return;

        if (apiPath.FileExists())
        {
            apiPath.DeleteFile(true);
            installForm?.UpdateUser($"Deleted {productLabel}: {Path.GetFileName(apiPath)}", LogTextBox.Action, false);
        }

        backupPath.MoveFile(apiPath!);
        installForm?.UpdateUser(
            $"Restored Steamworks: {Path.GetFileName(backupPath)} -> {Path.GetFileName(apiPath)}",
            LogTextBox.Action, false);
        ProgramData.Log.Info($"[{logTag}] Restored original {Path.GetFileName(apiPath)} from backup",
            LogDestination.Unlocker);
    }

    internal static void InstallSteamApiPair(string api32, string api32Backup, string api64, string api64Backup,
        string resource32, string resource64, InstallForm installForm, string logTag, string productLabel)
    {
        BackupOriginalIfNeeded(api32, api32Backup, installForm, logTag);
        WriteResourceIfBackupExists(resource32, api32, api32Backup, installForm, logTag, productLabel);
        BackupOriginalIfNeeded(api64, api64Backup, installForm, logTag);
        WriteResourceIfBackupExists(resource64, api64, api64Backup, installForm, logTag, productLabel);
    }

    internal static void UninstallSteamApiPair(string api32, string api32Backup, string api64, string api64Backup,
        InstallForm installForm, string logTag, string productLabel)
    {
        RestoreOriginal(api32, api32Backup, installForm, logTag, productLabel);
        RestoreOriginal(api64, api64Backup, installForm, logTag, productLabel);
    }
}
