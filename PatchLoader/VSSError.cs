using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatchLoader
{
    public class VSSErrors
    {
        public static string GetMessageByCode(int code, string name1 = "", string name2 = "", string number1 = "", string number2 = "")
        {
            switch ((short)code)
            {
                case -10600: return "File " + name1 + " may be corrupt";
                case -10159: return "Invalid date string: " + name1;
                case -10161: return "Invalid time or date string";
                case -10164: return "Too many file handles open.";
                case -10165: return "Access to file " + name1 + " denied";
                case -10166: return "Invalid drive: " + name1;
                case -10167: return "Invalid handle.";
                case -10168: return "Invalid filename: " + name1;
                case -10170: return "Invalid access code (bad parameter)";
                case -10171: return "Invalid DOS path: " + name1;
                case -10172: return "Dir " + name1 + " is in use";
                case -10173: return "Disk full";
                case -10175: return "File " + name1 + " already exists";
                case -10176: return "File " + name1 + "is locked";
                case -10178: return "File " + name1 + "not found";
                case -10180: return "Error reading from file";
                case -10181: return "File " + name1 + " is already open";
                case -10182: return "Too many file handles open";
                case -10183: return "Cannot rename to another volume";
                case -10184: return "Error writing to file";
                case -10200: return "Initialization variable " + name1 + " must be set to Yes or No";
                case -10201: return "Invalid syntax on line " + number1 + " of file " + name1;
                case -10202: return "Initialization variable " + name1 + " set to invalid number";
                case -10203: return "Initialization variable " + name1 + " set to invalid path";
                case -10205: return "Initialization variable " + name1 + " set to invalid value";
                case -10206: return "Cannot find initialization variable" + name1;
                case -10207: return "Initialization variable " + name1 + " must be between" + number1 + " and " + number2;
                case -10208: return "Too many SS.INI environment strings";
                case -10266: return "Timeout locking file:" + name1;
                case -10270: return "Out of memory";
                case -10625: return "You cannot modify the properties of a file that is checked out.";
                case -10279: return "You cannot perform a merge on a binary file, or a file that stores latest version only.";
                case -10280: return "Cannot check out " + name1 + ". It is binary and is already checked out.";
                case -10281: return name1 + " stores only the latest version and is already checked out.";
                case -10285: return "Error executing: " + name1;
                case -10626: return name1 + " is a SourceSafe configuration file and cannot be added.";
                case -10456: return "The SourceSafe database has been locked by the Administrator.";
                case -10402: return "Unable to rename % s to % s.";
                case -10403: return "Cannot find SS.INI file for user " + name1;
                case -10405: return "File " + name1 + " is currently checked out by" + name2;
                case -10406: return "You currently have file " + name1 + " checked out";
                case -10408: return "Cannot check out an old version of a file";
                case -10413: return "File " + name1 + " is currently checked out by " + name2;
                case -10415: return "An automatic merge has occurred and there are conflicts.\nEdit " + name1 + " to resolve them.";
                case -10418: return "Cannot delete the root project";
                case -10419: return "A deleted link to " + name1 + " already exists";
                case -10421: return "File " + name1 + " not found";
                case -10404: return "A history operation is already in progress";
                case -10423: return "You do not have access rights to " + name1;
                case -10426: return "A more recent version is checked out";
                case -10427: return "A writable copy of " + name1 + " already exists";
                case -10428: return "Move does not change the name of a project";
                case -10429: return "Project " + name1 + " does not exist";
                case -10430: return "Cannot move the root project";
                case -10431: return "Cannot roll back to the most recent version of " + name1;
                case -10432: return "Files have no common ancestor";
                case -10434: return name1 + " has been merged with no conflicts.";
                case -10435: return "File " + name1 + " is invalid. Files may not begin with $.";
                case -10436: return "File " + name1 + " is not checked out";
                case -10437: return "File " + name1 + " is not shared by any other projects";
                case -10438: return "Files are not branched";
                case -10457: return "Unable to open user login file " + name1 + ".";
                case -10439: return "Path " + name1 + " too long";
                case -10442: return "Rename does not move an item to another project";
                case -10443: return "Cannot Rename the root project";
                case -10447: return "Cannot Rollback to the most recent version of" + name1;
                case -10449: return "A project cannot be shared under a descendant.";
                case -10450: return "File " + name1 + " is already shared by this project";
                case -10515: return "Invalid SourceSafe syntax:" + name1;
                case -10550: return "Bad username syntax: " + name1;
                case -10551: return "Invalid password";
                case -10552: return "Incompatible database version";
                case -10553: return "Cannot delete the Admin user";
                case -10554: return "Permission denied";
                case -10555: return "Can not rename the Admin user";
                case -10556: return "Username too long";
                case -10557: return "User" + name1 + " already exists";
                case -10558: return "User " + name1 + "not found";
                case -10192: return "The URL for project" + name1 + " was not set properly.";
                case -10601: return "File " + name1 + " checked out";
                case -10602: return "Subproject or file not found";
                case -10603: return "Collision accessing database, please try again.";
                case -10614: return "File " + name1 + " is exclusively checked out.";
                case -10604: return "An item with the name " + name1 + " already exists";
                case -10605: return name1 + " is an invalid " + name2 + " name";
                case -10606: return "You can not move a project under itself";
                case -10607: return "File " + name1 + " does not retain old versions of itself";
                case -10608: return "File " + name1 + " cannot be checked into this project";
                case -10609: return "File or project not found";
                case -10610: return "Parent not found";
                case -10615: return "Version not found";
                case -10616: return "This command only works on files.";
                case -10617: return "This command only works on projects.";
                case -10194: return "A link in " + name1 + " was ignored because it was longer than SourceSafe can understand";
                case -10193: return "An error occurred while trying to check hyperlinks for " + name1;
                case -10440: return "Error loading SourceSafe add-in: " + name1;
                case -32766: return "Cancel";
                case -10999: return "Error loading resource string";
                default: return "Unhandled error occured, code: " + (short)code;
            }
        }
    }
}
