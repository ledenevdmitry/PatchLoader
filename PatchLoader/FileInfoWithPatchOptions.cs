using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatchLoader
{
    public class FileInfoWithPatchOptions
    {
        public FileInfo FileInfo { get; private set; }
        public bool AddInRepDir { get; private set; }
        public bool AddToPatch { get; private set; }

        public FileInfoWithPatchOptions(FileInfo fileInfo, bool addInRepDir, bool addToPatch)
        {
            FileInfo = fileInfo;
            AddInRepDir = addInRepDir;
            AddToPatch = addToPatch;
        }
    }
}
