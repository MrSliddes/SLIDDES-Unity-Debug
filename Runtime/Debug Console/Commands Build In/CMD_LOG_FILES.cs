using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;

namespace SLIDDES.Debug // 1.1 or use SLIDDES.Debug namespace for access to CustomCommand
{
    /// <summary>
    /// For accesing log files during editor/runtime
    /// </summary>
    /// <see cref=">https://docs.unity3d.com/Manual/LogFiles.html"/>
    public class CMD_LOG_FILES : CustomCommand
    {       
        public static DebugCommand LOG_FILES_EDITOR;
        public static DebugCommand LOG_FILES_PACKAGE_MANAGER;
        public static DebugCommand LOG_FILES_PLAYER;

        public override void Invoke() // 6. You need to overide the Invoke methode where you can instantiate the DebugCommands
        {            
            LOG_FILES_EDITOR = new DebugCommand("log_files_editor", "View the editor log", "log_files_editor", () =>
            {
#if UNITY_EDITOR_WIN
                Process.Start("explorer.exe", string.Format("/open, {0}\\Unity\\Editor\\Editor.log", Environment.GetEnvironmentVariable("LocalAppData")));
#elif UNITY_EDITOR_OSX
                DebugConsole.Log("[DebugConsole] Not implemented for this platform");
#elif UNITY_EDITOR_LINUX
                DebugConsole.Log("[DebugConsole] Not implemented for this platform");
#endif
            });

            LOG_FILES_PACKAGE_MANAGER = new DebugCommand("log_files_package_manager", "View the package manager log", "log_files_package_manager", () =>
            {
#if UNITY_EDITOR_WIN
                Process.Start("explorer.exe", string.Format("/open, {0}\\Unity\\Editor\\upm.log", Environment.GetEnvironmentVariable("LocalAppData")));
#elif UNITY_EDITOR_OSX
                DebugConsole.Log("[DebugConsole] Not implemented for this platform");
#elif UNITY_EDITOR_LINUX
                DebugConsole.Log("[DebugConsole] Not implemented for this platform");
#endif
            });

            LOG_FILES_PLAYER = new DebugCommand("log_files_player", "View the player log", "log_files_player", () =>
            {
#if UNITY_EDITOR_WIN
                string loc = string.Format(@"%USERPROFILE%\AppData\LocalLow\{0}\{1}\Player.log", Application.companyName, Application.productName);
                string path = Environment.ExpandEnvironmentVariables(loc);
                DebugConsole.Log(path);
                Process.Start("explorer.exe", "/open, " + path);
#elif UNITY_EDITOR_OSX
                DebugConsole.Log("[DebugConsole] Not implemented for this platform");
#elif UNITY_EDITOR_LINUX
                DebugConsole.Log("[DebugConsole] Not implemented for this platform");
#endif
            });


            commands = new List<object>()
            {
#if UNITY_EDITOR
                LOG_FILES_EDITOR,
                LOG_FILES_PACKAGE_MANAGER,
#endif
                LOG_FILES_PLAYER
            };
        }
    }
}