using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatchLoader
{
    class PatchUtils
    {
        private string remoteRoot;
        private string remoteLinkRoot;
        private string remoteBaseLocation;

        public PatchUtils(string remoteRoot, string remoteLinkRoot, string remoteBaseLocation)
        {
            this.remoteRoot = remoteRoot;
            this.remoteLinkRoot = remoteLinkRoot;
            this.remoteBaseLocation = remoteBaseLocation;
        }

        public bool PushPatch(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles, out List<string> vssPathCheckedOutToAnotherUser, string scriptsSubdir, string infaSubdir, List<string> repStructureScripts, List<string> repStructureInfa)
        {
            VSS vss = new VSS(remoteBaseLocation, "Dmitry");
            return vss.PushDir(localDir, patchFiles, remoteRoot, remoteLinkRoot, out vssPathCheckedOutToAnotherUser, scriptsSubdir, infaSubdir, repStructureScripts, repStructureInfa);
        }

        public bool IsAcceptableDir(DirectoryInfo dir, DirectoryInfo patchDir, List<string> acceptableRemotePathes)
        {
            return acceptableRemotePathes
                .Select(x => Path.Combine(patchDir.FullName, x.Replace('/', '\\')))
                .Where(x => x.Equals(dir.FullName, StringComparison.InvariantCultureIgnoreCase)).Count() > 0;
        }

        public bool IsDirectoryNameInPath(DirectoryInfo path, string dirName)
        {
            if (path == null)
            {
                return false;
            }

            if (path.Name.Equals(dirName, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return IsDirectoryNameInPath(path.Parent, dirName);
        }
    }
}
