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
        public static DebugCommand COMMANDS;
        public static DebugCommand<int> DEBUGCONSOLE_LOGMESSAGESMAXAMOUNT;
        public static DebugCommand<string> DEBUG_LOG;
        public static DebugCommand DEBUG_LOG_TEST;
        public static DebugCommand<object> DESTROY;
        public static DebugCommand<bool> DRAW_FPS;
        public static DebugCommand HELP;
        public static DebugCommand<int> LOAD_SCENE;
        public static DebugCommand<int> LOG_MESSAGES_VIEWPORT;
        public static DebugCommand<Vector3> MOVE_CAMERA;
        public static DebugCommand<Vector3> MOVE_PLAYER;
        public static DebugCommand RESTART_SCENE;
        public static DebugCommand<Tuple<string, string, string>> SCREEN_SETRESOLUTION;
        public static DebugCommand<bool> SHOW_AUTOCOMPLETE;
        public static DebugCommand<bool> SHOW_LOG_MESSAGES;
        public static DebugCommand<bool> SHOW_UNTIY_LOG;
        public static DebugCommand<float> TIME_TIMESCALE;

        public List<object> commands;

        private DebugConsole console;

        public void Invoke()
        {
            this.console = DebugConsole.Instance;
            InvokeCommands();
            commands = new List<object>
            {
                AUDIOLISTENER_VOLUME,
                BUILD_VERSION,
                COMMANDS,
                DEBUGCONSOLE_LOGMESSAGESMAXAMOUNT,
                DEBUG_LOG,
                DEBUG_LOG_TEST,
                DESTROY,
                DRAW_FPS,
                HELP,
                LOAD_SCENE,
                LOG_MESSAGES_VIEWPORT,
                MOVE_CAMERA,
                MOVE_PLAYER,
                RESTART_SCENE,
                SCREEN_SETRESOLUTION,
                SHOW_AUTOCOMPLETE,
                SHOW_LOG_MESSAGES,
                SHOW_UNTIY_LOG,
                TIME_TIMESCALE
            };
        }

        private void InvokeCommands()
        {
            AUDIOLISTENER_VOLUME = new DebugCommand<float>("AudioListener.volume", "Set AudioListener.volume, which controls the game sound volume (0.0 to 1.0) (non logarithmic)", "AudioListener.volume <float>", (x) =>
            {
                x = Mathf.Clamp(x, 0.0f, 1f);
                AudioListener.volume = x;
                DebugConsole.Log("[Debug Console] Set AudioListener.volume to " + x);
            });
            BUILD_VERSION = new DebugCommand("build_version", "Log the application current build version", "build_version", () => { DebugConsole.Log("[Debug Console] Build version: " + Application.version); });
            COMMANDS = new DebugCommand("commands", "Shows a list of all commands", "commands", () =>
            {
                DebugConsole.Log("<color=orange>[Commands]</color>");
                for(int i = 0; i < console.commands.Count; i++)
                {
                    DebugCommandBase commandBase = console.commands[i] as DebugCommandBase;
                    DebugConsole.Log(commandBase.ID + " : " + commandBase.Description);
                }
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
            DRAW_FPS = new DebugCommand<bool>("draw_fps", "Draw the game fps in the top left corner", "draw_fps <bool>", (x) => 
            {
                if(x)
                {
                    console.OnDisableExtend += console.OnGUIExtend - OnGUIDrawFPS;
                    console.OnGUIExtend += OnGUIDrawFPS;
                }
                else
                {
                    console.OnGUIExtend -= OnGUIDrawFPS;
                }
            });
            HELP = new DebugCommand("help", "Shows commands to help interacting with this debug console", "help", () =>
            {
                DebugConsole.Log("<color=orange>[Help]</color>");
                DebugConsole.Log("commands : shows a list of all commands");
                DebugConsole.Log("Debug Console created by SLIDDES");
            });
            LOAD_SCENE = new DebugCommand<int>("load_scene", "Load a scene by its build index", "load_scene <build_index>", (x) =>
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
            RESTART_SCENE = new DebugCommand("restart_scene", "Restart the current active scene", "restart_scene", () =>
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
            SHOW_AUTOCOMPLETE = new DebugCommand<bool>("show_autocomplete", "Show the DebugConsole auto complete GUI for commands", "show_autocomplete <bool>", (x) =>
            {
                console.showAutoComplete = x;
                DebugConsole.Log("[Debug Console] Set DebugConsole.showAutoComplete to " + x);
            });
            SHOW_LOG_MESSAGES = new DebugCommand<bool>("show_log_messages", "Show the DebugConsole.Log messages", "show_log_messages <bool>", (x) =>
            {
                console.showLogMessages = x;
            });
            SHOW_UNTIY_LOG = new DebugCommand<bool>("show_unity_log", "Shows the UnityEngine.Debug.Log in the DebugConsole", "show_unity_log <bool>", (x) =>
            {
                if(x)
                {
                    console.OnDisableExtend += () => Application.logMessageReceived -= ShowUnityLogInConsole;
                    Application.logMessageReceived += ShowUnityLogInConsole;
                }
                else
                {
                    Application.logMessageReceived -= ShowUnityLogInConsole;
                }
            });
            TIME_TIMESCALE = new DebugCommand<float>("Time.timeScale", "Set the scale at which time passes", "Time.timeScale <float>", (x) =>
            {
                x = Mathf.Clamp(x, 0, 100);
                Time.timeScale = x;
                DebugConsole.Log("[Debug Console] Set Time.timeScale to " + x);
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