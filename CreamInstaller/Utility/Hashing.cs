using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace CreamInstaller.Utility;

internal static class Hashing
{
    internal static async Task<string> ComputeSha256HexAsync(string filePath, CancellationToken cancellationToken = default)
    {
        await using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, true);
        byte[] hash = await SHA256.HashDataAsync(stream, cancellationToken).ConfigureAwait(false);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    internal static bool TryParseSha256Digest(string digest, out string hex)
    {
        hex = null;
        if (string.IsNullOrWhiteSpace(digest))
            return false;

        string value = digest.Trim();
        const string prefix = "sha256:";
        if (value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            value = value[prefix.Length..];

        if (value.Length != 64)
            return false;

        foreach (char c in value)
        {
            bool hexChar = c is (>= '0' and <= '9') or (>= 'a' and <= 'f') or (>= 'A' and <= 'F');
            if (!hexChar)
                return false;
        }

        hex = value.ToLowerInvariant();
        return true;
    }
}
