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

        public void PushPatch(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles)
        {
            VSS vss = new VSS(remoteBaseLocation, "Dmitry");
            vss.PushDir(localDir, patchFiles, remoteRoot, remoteLinkRoot);
        }
    }
}
