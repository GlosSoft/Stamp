using System;
using System.Text;
using System.Text.RegularExpressions;
using Mono.Cecil;

public partial class ModuleWeaver
{
    private string ReplaceVersion3rd(string versionString, int rev)
    {
        Version version;
        if (!Version.TryParse(versionString, out version))
        {
            throw new WeavingException("The version string must be prefixed with a valid Version. The following string does not: " + versionString);
        }

        var ret = version.Minor == 0 ? new Version(version.Major, 0, rev, 0) : new Version(version.Major + 1, 0, rev, 0);

        return ret.ToString();
    }

    private string AttributeHack(string versionString, VersionInfo ver)
    {
        var sb = new StringBuilder(versionString).Append(" ");
        if (ver.BranchName.StartsWith("/svn/FleetObserver/trunk/", StringComparison.InvariantCultureIgnoreCase))
        {
            sb.Append("Trunk");
        }
        else
        {
            sb.Append(new Regex("^/svn/FleetObserver/", RegexOptions.IgnoreCase).Replace(ver.BranchName, string.Empty, 1));
            sb.Append(" ").Append(Environment.MachineName);
        }

        return sb.ToString();
    }
}