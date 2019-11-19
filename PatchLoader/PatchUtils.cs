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
        private readonly string remoteRoot;
        private readonly string remoteLinkRoot;
        private readonly string remoteBaseLocation;

        public PatchUtils(string remoteRoot, string remoteLinkRoot, string remoteBaseLocation)
        {
            this.remoteRoot = remoteRoot;
            this.remoteLinkRoot = remoteLinkRoot;
            this.remoteBaseLocation = remoteBaseLocation;
        }

        public bool PushPatch(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles, out List<string> vssPathCheckedOutToAnotherUser, string scriptsSubdir, string infaSubdir, List<string> repStructureScripts, List<string> repStructureInfa)
        {
            VSS vss = new VSS(remoteBaseLocation, "Dmitry");
            return vss.PushDir(localDir, patchFiles, remoteRoot, remoteLinkRoot, out vssPathCheckedOutToAnotherUser);
        }

        public void CreateStructure(string dirName, string subdir, List<string> repScripts)
        {
            VSS vss = new VSS(remoteBaseLocation, "Dmitry");
            vss.CreateStructure(dirName, remoteRoot, subdir, repScripts);
        }


        public static bool IsAcceptableDir(DirectoryInfo dir, string scriptsOrInfaDir, string schema, DirectoryInfo patchDir, List<string> acceptableRemotePathes)
        {
            foreach(string acceptablePath in acceptableRemotePathes)
            {
                List<string> subpathes = new List<string>();

                string [] folders = acceptablePath.Split('/');
                string aggFolder = "";

                foreach(string folder in folders)
                {
                    aggFolder += folder;
                    subpathes.Add(aggFolder);

                    aggFolder += '/';
                }

                
                foreach(string subpath in subpathes)
                {
                    if (Path.Combine(patchDir.FullName, scriptsOrInfaDir, schema, subpath.Replace('/', '\\')).Equals(dir.FullName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
