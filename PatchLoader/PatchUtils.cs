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

        public bool PushPatch(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles, out List<string> vssPathCheckedOutToAnotherUser, string scriptsSubdir, string infaSubdir, List<string> repStructureInfa, List<string> repStructureScripts)
        {
            VSS vss = new VSS(remoteBaseLocation, "Dmitry");
            return vss.PushDir(localDir, patchFiles, remoteRoot, remoteLinkRoot, out vssPathCheckedOutToAnotherUser, scriptsSubdir, infaSubdir, repStructureInfa, repStructureScripts);
        }
    }
}
