using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SLIDDES.Debug
{
    /// <summary>
    /// Class containing build in commands
    /// </summary>
    public class DebugCommandsBuildIn
    {
        public static DebugCommand<float> AUDIOLISTENER_VOLUME;
        public static DebugCommand BUILD_VERSION;
        public static DebugCommand<float> CAMERA_FOV;
        public static DebugCommand COMMANDS;
        public static DebugCommand DEBUGCONSOLE_AUTOCOMPLETE;
        public static DebugCommand<int> DEBUGCONSOLE_AUTOCOMPLETEMODE;
        public static DebugCommand<int> DEBUGCONSOLE_LOGMESSAGESMAXAMOUNT;
        public static DebugCommand<string> DEBUG_LOG;
        public static DebugCommand DEBUG_LOG_LINK;
        public static DebugCommand DEBUG_LOG_TEST;
        public static DebugCommand<object> DESTROY;
        public static DebugCommand FPS_DRAW;
        public static DebugCommand<int> FPS_LIMIT;
        public static DebugCommand<int> FPS_VSYNCCOUNT;
        public static DebugCommand HELP;
        public static DebugCommand<int> SCENE_LOAD;
        public static DebugCommand LOG_MESSAGES_CLEAR;
        public static DebugCommand LOG_MESSAGES_DRAW;
        public static DebugCommand<int> LOG_MESSAGES_VIEWPORT;
        public static DebugCommand<Vector3> MOVE_CAMERA;
        public static DebugCommand<Vector3> MOVE_PLAYER;
        public static DebugCommand<bool> QUIT;
        public static DebugCommand SCENE_RESTART;
        public static DebugCommand<Tuple<string, string, string>> SCREEN_SETRESOLUTION;
        public static DebugCommand SYSTEM_INFO;
        public static DebugCommand<float> TIME_TIMESCALE;
        public static DebugCommand UI_DRAW;

        public List<object> commands;

        private DebugConsole console;

        private bool toggleDEBUGCONSOLE_AUTOCOMPLETE;
        private bool toggleDEBUG_LOG_LINK;
        private bool toggleFPS_DRAW;
        private bool toggleLOG_MESSAGES_DRAW = true;
        private bool toggleUI_DRAW = true;

        public void Invoke()
        {
            this.console = DebugConsole.Instance;
            InvokeCommands();
            // If you do not want default commands in your application you can uncomment them here
            commands = new List<object>
            {
                AUDIOLISTENER_VOLUME,
                BUILD_VERSION,
                CAMERA_FOV,
                COMMANDS,
                DEBUGCONSOLE_AUTOCOMPLETE,
                DEBUGCONSOLE_AUTOCOMPLETEMODE,
                DEBUGCONSOLE_LOGMESSAGESMAXAMOUNT,
                DEBUG_LOG,
                DEBUG_LOG_TEST,
                DESTROY,
                FPS_DRAW,
                FPS_LIMIT,
                FPS_VSYNCCOUNT,
                HELP,
                SCENE_LOAD,
                LOG_MESSAGES_CLEAR,
                LOG_MESSAGES_DRAW,
                LOG_MESSAGES_VIEWPORT,
                MOVE_CAMERA,
                MOVE_PLAYER,
                QUIT,
                SCENE_RESTART,
                SCREEN_SETRESOLUTION,
                DEBUG_LOG_LINK,
                SYSTEM_INFO,
                TIME_TIMESCALE,
                UI_DRAW
            };
        }

        private void InvokeCommands()
        {
            AUDIOLISTENER_VOLUME = new DebugCommand<float>("AudioListener.volume", "Set AudioListener.volume, which controls the application sound volume (0.0 to 1.0) (non logarithmic)", "AudioListener.volume <float>", (x) =>
            {
                x = Mathf.Clamp(x, 0.0f, 1f);
                AudioListener.volume = x;
                DebugConsole.Log("[Debug Console] Set AudioListener.volume to " + x);
            });
            BUILD_VERSION = new DebugCommand("build_version", "Log the application current build version", "build_version", () => { DebugConsole.Log("[Debug Console] Build version: " + Application.version); });
            CAMERA_FOV = new DebugCommand<float>("camera_fov", "Set main camera field of view (0.001 to 179)", "camera_fov <float>", (x) =>
            {
                x = Mathf.Clamp(x, 0.001f, 179);
                if(Camera.main == null)
                {
                    DebugConsole.Log("[Debug Console] Error: no camera with tag MainCamera found");
                }
                else
                {
                    Camera.main.fieldOfView = x;
                    DebugConsole.Log("[Debug Console] Set Camera.main.fieldOfView to " + x);
                }
            });
            COMMANDS = new DebugCommand("commands", "Shows a list of all commands", "commands", () =>
            {
                DebugConsole.Log("<color=orange>[Commands]</color>");
                for(int i = 0; i < console.commands.Count; i++)
                {
                    DebugCommandBase commandBase = console.commands[i] as DebugCommandBase;
                    DebugConsole.Log(commandBase.ID + " : " + commandBase.Description);
                }
            });
            DEBUGCONSOLE_AUTOCOMPLETE = new DebugCommand("debugconsole_autocomplete", "Toggle the DebugConsole auto complete GUI for commands", "debugconsole_autocomplete", () =>
            {
                toggleDEBUGCONSOLE_AUTOCOMPLETE = !toggleDEBUGCONSOLE_AUTOCOMPLETE;
                console.showAutoComplete = toggleDEBUGCONSOLE_AUTOCOMPLETE;
                DebugConsole.Log("[Debug Console] Set DebugConsole.showAutoComplete to " + toggleDEBUGCONSOLE_AUTOCOMPLETE);
            });
            DEBUGCONSOLE_AUTOCOMPLETEMODE = new DebugCommand<int>("debugconsole_autocompletemode", "Change the way autocomplete suggests (0 = default/strickt, 1 = contains/loose)", "debugconsole_autocompletemode <int>", (x) =>
            {
                x = Mathf.Clamp(x, 0, 1);
                console.autoCompleteMode = x;
                DebugConsole.Log("[Debug Console] Set DebugConsole.autoCompleteMode to " + x);
            });
            DEBUGCONSOLE_LOGMESSAGESMAXAMOUNT = new DebugCommand<int>("DebugConsole.logMessagesMaxAmount", "The max amount of log messages (lines) the console can have before deleting old ones", "DebugConsole.logMessagesMaxAmount <int>", (x) =>
            {
                x = Mathf.Clamp(x, 1, 99999);
                console.logMessagesMaxAmount = x;
                DebugConsole.Log("[Debug Console] Set DebugConsole.logMessagesMaxAmount to " + x);
            });
            DEBUG_LOG = new DebugCommand<string>("debug_log", "Log a custom message in Unity's Debug.Log", "debug_log \"<string>\"", (x) =>
            {
                DebugConsole.Log("[Debug.Log] " + x);
                UnityEngine.Debug.Log("[Debug Console] " + x);
            });
            DEBUG_LOG_LINK = new DebugCommand("debug_log_link", "Toggle linking the UnityEngine.Debug.Log to the DebugConsole", "debug_log_link", () =>
            {
                toggleDEBUG_LOG_LINK = !toggleDEBUG_LOG_LINK;
                if(toggleDEBUG_LOG_LINK)
                {
                    console.OnDisableExtend += () => Application.logMessageReceived -= ShowUnityLogInConsole;
                    Application.logMessageReceived += ShowUnityLogInConsole;
                }
                else
                {
                    Application.logMessageReceived -= ShowUnityLogInConsole;
                }
            });
            DEBUG_LOG_TEST = new DebugCommand("debug_log_test", "Prints a debug log test message in UnityEngine.Debug.Log", "debug_log_test", () => { UnityEngine.Debug.Log("[Debug Console] Test msg trough debug console " + System.DateTime.Now.ToString()); });
            DESTROY = new DebugCommand<object>("destroy", "Destroy a GameObject by name/tag", "destroy (name:||tag:)<string>", (x) =>
            {
                if(false) //x.ToString().GetType() != null
                {
                    // Stuffs not working, the t type is never found in unity -> Activator.CreateInstance(customCommands[i]) as CustomCommand; solution?nope
                    // Possible sol is to have predefined classes
                    //string s = x as string;
                    //DebugConsole.Log(s);
                    //Type t = Type.GetType(s);
                    //DebugConsole.Log(t?.ToString());
                    ////if(t.BaseType != typeof(MonoBehaviour))
                    ////{
                    ////    DebugConsole.Log("[Debug Console] Error: type " + t + " does not derive from Monobehaviour");
                    ////    //return;
                    ////}
                    ////object a = null;
                    //Type generic = null;
                    //generic.MakeGenericType(t);
                    //Type t = Activator.CreateInstance(x) as Type;
                    //GameObject a = GameObject.FindObjectOfType<t>().gameObject;//Type.GetType(t.Name)
                    //UnityEngine.Debug.Log(a);
                    //if(a == null)
                    //{
                    //    DebugConsole.Log("[Debug Console] Error: no GameObject with type " + t + " found");
                    //}
                    //else
                    //{
                    //    DebugConsole.Log("[Debug Console] Destroyed GameObject with type " + t);
                    //    GameObject.Destroy(a as GameObject);
                    //}
                }
                else if(x as string != null)
                {
                    string s = x as string;
                    if(s.Contains("tag:"))
                    {
                        s = s.Replace("tag:", "");
                        GameObject a = GameObject.FindWithTag(s);
                        if(a == null)
                        {
                            DebugConsole.Log("[Debug Console] Error: no GameObject with tag " + s + " found");
                        }
                        else
                        {
                            DebugConsole.Log("[Debug Console] Destroyed GameObject with tag " + s);
                            GameObject.Destroy(a);
                        }
                    }
                    else
                    {
                        if(s.Contains("name:")) s = s.Replace("name:", "");
                        GameObject a = GameObject.Find(s);
                        if(a == null)
                        {
                            DebugConsole.Log("[Debug Console] Error: no GameObject with name " + s + " found");
                        }
                        else
                        {
                            DebugConsole.Log("[Debug Console] Destroyed GameObject with name " + s);
                            GameObject.Destroy(a);
                        }
                    }
                }                
                else
                {
                    DebugConsole.Log("[Debug Console] Error: argument needs to be passed as a string or Component");
                }
            });
            FPS_DRAW = new DebugCommand("fps_draw", "Toggle drawing the application fps in the top left corner", "fps_draw <bool>", () => 
            {
                toggleFPS_DRAW = !toggleFPS_DRAW;
                if(toggleFPS_DRAW)
                {
                    console.OnDisableExtend += console.OnGUIExtend - OnGUIDrawFPS;
                    console.OnGUIExtend += OnGUIDrawFPS;
                }
                else
                {
                    console.OnGUIExtend -= OnGUIDrawFPS;
                }
            });
            FPS_LIMIT = new DebugCommand<int>("fps_limit", "Set Application.targetFrameRate (-1 to 999999)", "fps_limit <int>", (x) =>
            {
                x = Mathf.Clamp(x, -1, 999999);
                if(x == 0) x = -1;
                Application.targetFrameRate = x;
                DebugConsole.Log("[Debug Console] Set Application.targetFrameRate to " + x);
            });
            FPS_VSYNCCOUNT = new DebugCommand<int>("fps_vsynccount", "Set QualitySettings.vSyncCount (0 to 4). If value > 0 then fps_limit will be ignored.", "fps_vsynccount <int>", (x) =>
            {
                x = Mathf.Clamp(x, 0, 4);
                QualitySettings.vSyncCount = x;
                DebugConsole.Log("[Debug Console] Set QualitySettings.vSyncCount to " + x);
            });
            HELP = new DebugCommand("help", "Shows commands to help interacting with this debug console", "help", () =>
            {
                DebugConsole.Log("<color=orange>[Help]</color>");
                DebugConsole.Log("commands : shows a list of all commands");
                DebugConsole.Log("Debug Console created by SLIDDES");
            });
            LOG_MESSAGES_CLEAR = new DebugCommand("log_messages_clear", "Clear all log messages of DebugConsole", "log_messages_clear", () =>
            {
                DebugConsole.ClearLog();
            });
            LOG_MESSAGES_DRAW = new DebugCommand("log_messages_draw", "Toggle drawing the DebugConsole.Log messages", "log_messages_draw", () =>
            {
                toggleLOG_MESSAGES_DRAW = !toggleLOG_MESSAGES_DRAW;
                console.showLogMessages = toggleLOG_MESSAGES_DRAW;
            });
            LOG_MESSAGES_VIEWPORT = new DebugCommand<int>("log_messages_viewport", "Change the viewport size of the DebugConsole log messages", "log_messages_viewport (0 to 2)<int>", (x) =>
            {
                x = Mathf.Clamp(x, 0, 2);
                console.logMessagesHeightIndex = x;
                switch(x)
                {
                    case 1: DebugConsole.Log("[Debug Console] Set log messages viewport to half screen"); break;
                    case 2: DebugConsole.Log("[Debug Console] Set log messages viewport to full screen"); break;
                    default: DebugConsole.Log("[Debug Console] Set log messages viewport to default"); break;
                }
            });
            MOVE_CAMERA = new DebugCommand<Vector3>("move_camera", "Move the GameObject with the tag MainCamera (x, y, z)", "move_camera <float> <float> <float>", (x) =>
            {
                GameObject a = GameObject.FindWithTag("MainCamera");
                if(a == null)
                {
                    DebugConsole.Log("[Debug Console] Error: no GameObject with tag MainCamera found");
                    return;
                }
                a.transform.position = x;
                DebugConsole.Log("[Debug Console] Moved camera position to " + x);
            });
            MOVE_PLAYER = new DebugCommand<Vector3>("move_player", "Move the GameObject with the tag Player to given position (x, y, z)", "move_player <float> <float> <float>", (x) =>
            {
                GameObject a = GameObject.FindWithTag("Player");
                if(a == null)
                {
                    DebugConsole.Log("[Debug Console] Error: no GameObject with tag Player found");
                    return;
                }
                a.transform.position = x;
                DebugConsole.Log("[Debug Console] Moved player position to " + x);
            });
            QUIT = new DebugCommand<bool>("quit", "Quit the application", "quit <bool>", (x) =>
            {
                if(x)
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.ExitPlaymode();
#else
                    Application.Quit();
#endif
                }
                else DebugConsole.Log("[Debug Console] Then why did you type this command?");
            });
            SCENE_LOAD = new DebugCommand<int>("scene_load", "Load a scene by its build index", "scene_load <build_index>", (x) =>
            {
                DebugConsole.Log("[Debug Console] Load build index scene " + x);
                // Check if scene build index exists
                if(x < 0 || x > SceneManager.sceneCountInBuildSettings - 1)
                {
                    DebugConsole.Log("[Debug Console] Error: no scene found with build index " + x);
                }
                else
                {
                    SceneManager.LoadScene(x);
                }
            });
            SCENE_RESTART = new DebugCommand("scene_restart", "Restart the current active scene", "scene_restart", () =>
            {
                DebugConsole.Log("[Debug Console] Restart current scene with build index " + SceneManager.GetActiveScene().buildIndex);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
            SCREEN_SETRESOLUTION = new DebugCommand<Tuple<string, string, string>>("Screen.SetResolution", "Switches the screen resolution (width, height, fullscreen)", "Screen.SetResolution <int> <int> <bool>", (x) =>
            {
                int width = 0, height = 0;
                bool fullScreen = false;
                // type casting goes wrong
                if(int.TryParse(x.Item1, out width) && int.TryParse(x.Item2, out height) && bool.TryParse(x.Item3, out fullScreen))
                {
                    Screen.SetResolution(width, height, fullScreen);
                    DebugConsole.Log("[Debug Console] Set screen resolution to " + width + "x" + height + ", fullscreen: " + fullScreen);
                }
                else
                {
                    DebugConsole.Log("[Debug Console] Error: argument(s) not passed in correctly, got " + x.Item1.ToString() + " " + x.Item2.ToString() + " " + x.Item3.ToString());
                }
            });
            SYSTEM_INFO = new DebugCommand("system_info", "Show info about the system", "system_info", () =>
            {
                DebugConsole.Log("<color=orange>[System Info]</color>");
                DebugConsole.Log("[Debug Console] Screen size: " + Screen.width + "x" + Screen.height);
                DebugConsole.Log("[Debug Console] Fullscreen: " + Screen.fullScreen);
                DebugConsole.Log("[Debug Console] Fullscreen Mode: " + Screen.fullScreenMode);
                DebugConsole.Log("[Debug Console] Operating System: " + SystemInfo.operatingSystem);
                DebugConsole.Log("[Debug Console] Processor: " + SystemInfo.processorType);
                DebugConsole.Log("[Debug Console] Graphics Card: " + SystemInfo.graphicsDeviceName);
                DebugConsole.Log("[Debug Console] Memory Present: " + SystemInfo.systemMemorySize);
                DebugConsole.Log("[Debug Console] Audio Device Availabe: " + SystemInfo.supportsAudio);
            });
            TIME_TIMESCALE = new DebugCommand<float>("Time.timeScale", "Set the scale at which time passes", "Time.timeScale <float>", (x) =>
            {
                x = Mathf.Clamp(x, 0, 100);
                Time.timeScale = x;
                DebugConsole.Log("[Debug Console] Set Time.timeScale to " + x);
            });
            UI_DRAW = new DebugCommand("ui_draw", "Toggle drawing UI elements", "ui_draw", () =>
            {
                toggleUI_DRAW = !toggleUI_DRAW;
                Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
                foreach(Canvas item in canvases)
                {
                    item.enabled = toggleUI_DRAW;
                }
                DebugConsole.Log("[Debug Console] Set draw UI to " + toggleUI_DRAW);
            });
        }

        #region Functions

        /// <summary>
        /// Shows UnityEngine.Debug.log messages in console
        /// </summary>
        /// <param name="logString"></param>
        /// <param name="stackTrace"></param>
        /// <param name="type"></param>
        private void ShowUnityLogInConsole(string logString, string stackTrace, LogType type)
        {
            string s = "[" + System.DateTime.UtcNow.ToString("HH:mm:ss") + "] [" + type + "] " + logString;
            DebugConsole.Log("[UEDL] " + s);
            if(type == LogType.Exception)
            {
                DebugConsole.Log("[UEDL] " + stackTrace);
            }
        }

        /// <summary>
        /// GUI methode for drawing fps
        /// </summary>
        private void OnGUIDrawFPS()
        {
            Color c = GUI.contentColor; GUI.contentColor = Color.green;
            GUI.Label(new Rect(4, 4, 50, 20), ((int)(1f / Time.unscaledDeltaTime)).ToString());
            GUI.contentColor = c;
        }

        #endregion        
    }
}