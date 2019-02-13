extern alias svn18;

using System.IO;

public class SvnHelper : ISvnHelper
{
    SvnHelper18 svn18;

    public SvnHelper()
    {
        svn18 = new SvnHelper18();
    }

    public string TreeWalkForSvnDir(string currentDirectory)
    {
        return svn18.TreeWalkForSvnDir(currentDirectory);
    }

    public VersionInfo GetSvnInfo(string targetFolder)
    {
        return svn18.GetSvnInfo(targetFolder);
    }
}

public class SvnHelper18 : ISvnHelper
{
    public string TreeWalkForSvnDir(string currentDirectory)
    {
        while (true)
        {
            var svnDir = Path.Combine(currentDirectory, @".svn");
            var svnDir_underscoreTortoiseHack = Path.Combine(currentDirectory, @".svn");

            if (Directory.Exists(svnDir) || Directory.Exists(svnDir_underscoreTortoiseHack))
            {
                return currentDirectory;
            }

            try
            {
                var parent = Directory.GetParent(currentDirectory);
                if (parent == null)
                {
                    break;
                }
                currentDirectory = parent.FullName;
            }
            catch
            {
                // trouble with tree walk.
                return null;
            }
        }
        return null;
    }

    public VersionInfo GetSvnInfo(string targetFolder)
    {
        svn18::SharpSvn.SvnWorkingCopyVersion version;
        using (var client = new svn18::SharpSvn.SvnWorkingCopyClient())
        {
            client.GetVersion(targetFolder, out version);
        }

        svn18::SharpSvn.SvnInfoEventArgs info;
        using (var client = new svn18::SharpSvn.SvnClient())
        {
            client.GetInfo(targetFolder, out info);
        }

        return new VersionInfo()
        {
            BranchName = info.Uri.AbsolutePath,
            HasChanges = version.Modified,
            Revision = (int)version.End,
        };
    }
}