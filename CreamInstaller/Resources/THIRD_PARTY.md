# Embedded third-party binaries

SHA-256 checksums for every embedded DLL live in `manifest.json` (same folder).

| Component | Upstream | Notes |
|-----------|----------|-------|
| Koaloader | https://github.com/acidicoala/Koaloader | Proxy DLLs under `Koaloader/` |
| SmokeAPI | https://github.com/acidicoala/SmokeAPI | `SmokeAPI/steam_api*.dll` |
| ScreamAPI | https://github.com/acidicoala/ScreamAPI | `ScreamAPI/EOSSDK-*.dll` |
| Uplay R1 Unlocker | https://github.com/acidicoala/UplayR1Unlocker | `UplayR1/` |
| Uplay R2 Unlocker | https://github.com/acidicoala/UplayR2Unlocker | `UplayR2/` |
| CreamAPI | restored from project history when missing from tree | `CreamAPI/steam_api*.dll` |

Regenerate the manifest after updating binaries:

```powershell
./Hashing/GetHashes.ps1
# or re-run the PowerShell block that writes Resources/manifest.json
```
