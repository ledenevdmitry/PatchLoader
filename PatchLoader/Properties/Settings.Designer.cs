﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PatchLoader.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("$/Objects_ATC")]
        public string RemoteRoot {
            get {
                return ((string)(this["RemoteRoot"]));
            }
            set {
                this["RemoteRoot"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("$/Patches/Working")]
        public string RemoteLinkRoot {
            get {
                return ((string)(this["RemoteLinkRoot"]));
            }
            set {
                this["RemoteLinkRoot"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\bivss-dmz\\DWH\\srcsafe.ini")]
        public string BaseLocation {
            get {
                return ((string)(this["BaseLocation"]));
            }
            set {
                this["BaseLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LastSavedDir {
            get {
                return ((string)(this["LastSavedDir"]));
            }
            set {
                this["LastSavedDir"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".*")]
        public string AddToRep {
            get {
                return ((string)(this["AddToRep"]));
            }
            set {
                this["AddToRep"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".*")]
        public string AddToPatch {
            get {
                return ((string)(this["AddToPatch"]));
            }
            set {
                this["AddToPatch"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("script_|file_sc")]
        public string NotAddToRep {
            get {
                return ((string)(this["NotAddToRep"]));
            }
            set {
                this["NotAddToRep"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("vssver2.scc")]
        public string NotAddToPatch {
            get {
                return ((string)(this["NotAddToPatch"]));
            }
            set {
                this["NotAddToPatch"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("db_scripts")]
        public string ScriptsSubdir {
            get {
                return ((string)(this["ScriptsSubdir"]));
            }
            set {
                this["ScriptsSubdir"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("infa_xml")]
        public string InfaSubdir {
            get {
                return ((string)(this["InfaSubdir"]));
            }
            set {
                this["InfaSubdir"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Index\r\nProcedure\r\nTable\r\nTablespace\r\nUser\r\nView")]
        public string RepStructureScripts {
            get {
                return ((string)(this["RepStructureScripts"]));
            }
            set {
                this["RepStructureScripts"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Configs\r\nMappings\r\nMapplets\r\nSchedulers\r\nSessions\r\nSources\r\nTargets\r\nTasks\r\nTrans" +
            "formations\r\nUser-Defined Function\r\nWorkflows")]
        public string RepStructureInfa {
            get {
                return ((string)(this["RepStructureInfa"]));
            }
            set {
                this["RepStructureInfa"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("X:\\IPatch\\patch_ins_pr_slesh\\PI.exe")]
        public string PatchInstallerPath {
            get {
                return ((string)(this["PatchInstallerPath"]));
            }
            set {
                this["PatchInstallerPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("$/Patches/Working STAB2\r\n$/Patches/Test/_ODH\r\n$/Patches/Test/_CORE\r\n$/Patches/REL" +
            "EASE DWH_NEW_DELAYED\r\n$/Patches/RELEASE DWH_NEW/ODH\r\n$/Patches/RELEASE DWH_NEW/C" +
            "ORE\r\n$/Patches/Working\r\n$/Patches/Production")]
        public string SearchRoots {
            get {
                return ((string)(this["SearchRoots"]));
            }
            set {
                this["SearchRoots"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Program Files (x86)\\Microsoft Visual SourceSafe")]
        public string SSPath {
            get {
                return ((string)(this["SSPath"]));
            }
            set {
                this["SSPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WF_(C|Z)\\d+._FIX.*\\.xml")]
        public string CreateSTWFRegex {
            get {
                return ((string)(this["CreateSTWFRegex"]));
            }
            set {
                this["CreateSTWFRegex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("flow_control@rep_dw_prod")]
        public string STWFFolder {
            get {
                return ((string)(this["STWFFolder"]));
            }
            set {
                this["STWFFolder"] = value;
            }
        }
    }
}
