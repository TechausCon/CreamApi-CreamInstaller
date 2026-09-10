using CreamInstaller.Resources;
using CreamInstaller.Utility;
using Xunit;

namespace CreamInstaller.Tests;

public class HashingTests
{
    [Theory]
    [InlineData("sha256:aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)]
    [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", true)]
    [InlineData("sha256:short", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void TryParseSha256Digest_ValidatesInput(string? digest, bool expected)
    {
        bool ok = Hashing.TryParseSha256Digest(digest!, out string? hex);
        Assert.Equal(expected, ok);
        if (expected)
            Assert.Equal(64, hex!.Length);
    }
}

public class CreamApiConfigTests
{
    [Fact]
    public void ParseConfigDlcLines_ReadsDlcSection()
    {
        string[] lines =
        [
            "[steam]",
            "appid = 123",
            "[dlc]",
            "1001 = First DLC",
            "1002=Second DLC",
            "[extra]",
            "ignored = yes"
        ];

        List<(string id, string name)> result = CreamAPI.ParseConfigDlcLines(lines);

        Assert.Equal(2, result.Count);
        Assert.Equal(("1001", "First DLC"), result[0]);
        Assert.Equal(("1002", "Second DLC"), result[1]);
    }

    [Fact]
    public void ParseConfigDlcLines_EmptyWithoutDlcSection()
    {
        List<(string id, string name)> result = CreamAPI.ParseConfigDlcLines(["[steam]", "appid=1"]);
        Assert.Empty(result);
    }
}
