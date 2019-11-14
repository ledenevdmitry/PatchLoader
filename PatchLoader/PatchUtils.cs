﻿using System;
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
            return vss.PushDir(localDir, patchFiles, remoteRoot, remoteLinkRoot, out vssPathCheckedOutToAnotherUser,
                Properties.Settings.Default.RepStructureScripts.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                Properties.Settings.Default.RepStructureInfa.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                scriptsSubdir, infaSubdir, repStructureScripts, repStructureInfa);
        }

        public static bool IsAcceptableDir(DirectoryInfo dir, string scriptsOrInfaDir, DirectoryInfo patchDir, List<string> acceptableRemotePathes)
        {/*
            return acceptableRemotePathes
                .Select(x => Path.Combine(patchDir.FullName, scriptsOrInfaDir, x.Replace('/', '\\')))
                .Where(x => x.Equals(dir.FullName, StringComparison.InvariantCultureIgnoreCase)).Count() > 0;
*/
            foreach(string acceptablePath in acceptableRemotePathes)
            {
                List<string> subpathes = new List<string>();
                for(int i = 0; i != -1; i += acceptablePath.IndexOf('/', i))
                {
                    if (i > 0)
                    {
                        subpathes.Add(acceptablePath.Substring(0, i));
                    }
                }
                
                foreach(string subpath in subpathes)
                {
                    if (Path.Combine(patchDir.FullName, scriptsOrInfaDir, subpath.Replace('/', '\\')).Equals(dir.FullName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
