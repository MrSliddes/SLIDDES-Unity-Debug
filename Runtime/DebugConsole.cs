using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SLIDDES.Debug
{
    /// <summary>
    /// Base script for displaying the debug console in game
    /// </summary>
    public class DebugConsole : MonoBehaviour
    {
        public static DebugConsole Instance
        {
            get
            {
                if(instance == null)
                {
                    UnityEngine.Debug.Log("[Debug Console] not active in scene, creating DebugConsole");
                    return new GameObject("DebugConsole").AddComponent<DebugConsole>();
                }
                return instance;
            }
        }

        /// <summary>
        /// Contains the list of commands (DebugCommand)
        /// </summary>
        public List<object> commands;

        /// <summary>
        /// For adding external code to the OnGUI draw of DebugConsole
        /// </summary>
        public Action OnGUIExtend;
        /// <summary>
        /// For adding external code to the OnGUI draw of DebugConsole when DebugConsole.show is true
        /// </summary>
        public Action OnGUIConsoleExtend;
        /// <summary>
        /// For adding external code to OnEnable
        /// </summary>
        public Action OnEnableExtend;
        /// <summary>
        /// For adding external code to ondisable
        /// </summary>
        public Action OnDisableExtend;

        protected static DebugConsole instance;

        /// <summary>
        /// Show the debug console log messages window
        /// </summary>
        [HideInInspector] public bool showLogMessages;
        /// <summary>
        /// Show the auto complete gui
        /// </summary>
        [HideInInspector] public bool showAutoComplete;
        /// <summary>
        /// The max amount of log messages the console can keep track of before deleting old ones
        /// </summary>
        [HideInInspector] public int logMessagesMaxAmount;
        /// <summary>
        /// The height of the log messages gui, 0 = normal, 1 = half screen height, 2 = full screen height
        /// </summary>
        [HideInInspector] public float logMessagesHeightIndex;

        /// <summary>
        /// Has the button arrow down been pressed prev frame
        /// </summary>
        private bool buttonPressedArrowDown;
        /// <summary>
        /// Has the button arrow up been pressed prev frame
        /// </summary>
        private bool buttonPressedArrowUp;
        /// <summary>
        /// Has the button enter been pressed prev frame
        /// </summary>
        private bool buttonPressedEnter;
        /// <summary>
        /// Has the button tab been pressed prev frame
        /// </summary>
        private bool buttonPressedTab;
        /// <summary>
        /// Has the input value changed compared to previous frame?
        /// </summary>
        private bool inputHasChanged;
        /// <summary>
        /// If the autoCompleteIndex has changed
        /// </summary>
        private bool newAutoCompleteIndex;
        /// <summary>
        /// Show the debug console
        /// </summary>
        private bool show;        
        /// <summary>
        /// The current autocomplete list selected index for autocompletion
        /// </summary>
        private int autoCompleteIndex;
        /// <summary>
        /// The amount of log messages (lines) in logMessages
        /// </summary>
        private int logMessagesAmount;
        /// <summary>
        /// The line height of the font
        /// </summary>
        private float lineHeight = 15f;
        /// <summary>
        /// Keeps track of the viewport log messages height
        /// </summary>
        private float logMessagesHeight = 6;        
        /// <summary>
        /// The auto complete string
        /// </summary>
        private string autoComplete;
        /// <summary>
        /// The auto complete string shown in the bar
        /// </summary>
        private string autoCompleteBar;
        /// <summary>
        /// The input given to the console
        /// </summary>
        private string input = "";
        /// <summary>
        /// The outputted log messages of the debug console
        /// </summary>
        private string logMessages;
        /// <summary>
        /// The previous command enterd
        /// </summary>
        private string previousCommand = "";
        /// <summary>
        /// The previous input (last frame) to check if input has changed
        /// </summary>
        private string previousInput;
        /// <summary>
        /// The vector2 to keep track of the scrollview
        /// </summary>
        private Vector2 autoCompleteScroll;
        /// <summary>
        /// The vector2 to keep track of the scrollview
        /// </summary>
        private Vector2 logMessageScroll;
        /// <summary>
        /// Textures used for GUI
        /// </summary>
        private Texture2D textureBlack;
        private Texture2D textureBlack05;
        private Texture2D textureBlack06;
        private Texture2D textureBlack08;
        private Texture2D textureGray08;
        private Texture2D textureDefault;
        private TextEditor textEditor;
        /// <summary>
        /// List of autocomplete values
        /// </summary>
        private List<string> autoCompleteList;
        private DebugCommandsBuildIn commandsBuildIn;

        private void Awake()
        {
            // Get
            if(instance != null)
            {
                UnityEngine.Debug.Log("[Debug Console] is already active in scene.");
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Load adjustable values
            showLogMessages = true;
            showAutoComplete = true;
            logMessagesMaxAmount = 1000;
            logMessagesHeightIndex = 0;

            // Set
            // List all commands
            commands = new List<object>();
            // Add build in commands
            commandsBuildIn = new DebugCommandsBuildIn();
            commandsBuildIn.Invoke();
            commands.AddRange(commandsBuildIn.commands);
            // Add custom commands (this took around 4 hours of coding so you better appreciated it)
            // Get all possible types that derive from base class CustomCommand and add the commands from them
            Type[] customCommands = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsSubclassOf(typeof(CustomCommand))).ToArray();
            int customCommandsAdded = 0;
            for(int i = 0; i < customCommands.Length; i++)
            {
                // Get an object from the command class
                CustomCommand cc = Activator.CreateInstance(customCommands[i]) as CustomCommand;

                // Invoke the class (assigns the commands to the command list)
                cc.Invoke();

                // Loop trough the CustomCommand.commands list and add it to DebugConsole.commands
                DebugCommand command = null;
                for(int j = 0; j < cc.commands.Count; j++)
                {
                    command = cc.commands[j] as DebugCommand;
                    commands.Add(command);
                    customCommandsAdded++;
                }
            }
            Log("[Debug Console] Loaded " + customCommandsAdded + " custom command(s)");

            autoCompleteList = new List<string>();

            // Textures
            textureBlack = new Texture2D(1, 1);
            textureBlack.SetPixel(0, 0, Color.black);
            textureBlack.Apply();
            textureBlack05 = new Texture2D(1, 1);
            textureBlack05.SetPixel(0, 0, new Color(0, 0, 0, 0.5f));
            textureBlack05.Apply();
            textureBlack06 = new Texture2D(1, 1);
            textureBlack06.SetPixel(0, 0, new Color(0, 0, 0, 0.6f));
            textureBlack06.Apply();
            textureBlack08 = new Texture2D(1, 1);
            textureBlack08.SetPixel(0, 0, new Color(0, 0, 0, 0.8f));
            textureBlack08.Apply();
            textureGray08 = new Texture2D(1, 1);
            textureGray08.SetPixel(0, 0, new Color(0, 0, 0, 0.8f));
            textureGray08.Apply();
        }

        private void OnEnable()
        {
            OnEnableExtend?.Invoke();
        }

        private void OnDisable()
        {
            OnDisableExtend?.Invoke();
        }

        private void Update()
        {
            // Toggling debug console
            if(Input.GetKeyDown(KeyCode.F6) || Input.GetKeyDown(KeyCode.Tilde) || Input.GetKeyDown(KeyCode.BackQuote)) ToggleShow();
        }

        private void OnGUI()
        {
            OnGUIExtend?.Invoke();

            if(!show) return;
            float y = 0;
            textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);

            // Check if input changed
            if(previousInput != input)
            {
                inputHasChanged = true;
                previousInput = input;
            }
            else inputHasChanged = false;

            // Draw log messages
            if(showLogMessages)
            {
                float height;
                switch(logMessagesHeightIndex)
                {
                    case 2: height = Screen.height - 24; break;
                    case 1: height = (Screen.height - 24) / 2; break;
                    default: height = 120; break;
                }
                DrawQuad(new Rect(0, 0, Screen.width, height), ref textureBlack06);
                logMessageScroll = GUI.BeginScrollView(new Rect(0, 0, Screen.width, height), logMessageScroll, new Rect(0, 0, Screen.width - 16, logMessagesHeight));

                GUI.Label(new Rect(4, 0, Screen.width, logMessagesHeight), logMessages);

                GUI.EndScrollView();
                y = height;
            }

            // Draw console bar
            GUI.skin.textField.normal.background = textureBlack08;
            GUI.skin.textField.hover.background = textureGray08;
            GUI.skin.textField.focused.background = textureBlack08;
            GUI.SetNextControlName("consoleBar");
            input = GUI.TextField(new Rect(0, y, Screen.width, 24), input);
            if(Event.current.keyCode == KeyCode.BackQuote) GUI.FocusControl("consoleBar");
            if(input.Contains('`'))
            {
                ToggleShow();
                input = "";
            }
            
            //if(Event.current.type == EventType.Repaint && new Rect(0, y, Screen.width, 24).Contains(Event.current.mousePosition))
            //{
            //    print("hoverr");
            //}

            // Help message
            if(string.IsNullOrEmpty(input))
            {
                Color c = GUI.color; GUI.color = Color.gray;
                GUI.Label(new Rect(4, y, Screen.width - 4, 24), "> Type \"help\" for more information about the debug console");
                GUI.color = c;
            }
            else
            {
                // Auto complete command
                if(showAutoComplete)
                {
                    float preY = y;                    

                    if(inputHasChanged)
                    {
                        autoComplete = "";
                        autoCompleteList.Clear();
                        autoCompleteIndex = 0;

                        // Get all the same commands based on input
                        for(int i = 0; i < commands.Count; i++)
                        {
                            DebugCommandBase commandBase = commands[i] as DebugCommandBase;
                            if(input.ToLower() == commandBase.ID.Substring(0, Mathf.Clamp(input.Length, 0, commandBase.ID.Length)).ToLower())
                            {
                                autoCompleteList.Add(commandBase.Format + " : " + commandBase.Description);
                            }
                        }
                    }

                    // Ownly draw if there are matches
                    if(autoCompleteList.Count > 0)
                    {
                        if(inputHasChanged)
                        {
                            for(int i = 0; i < autoCompleteList.Count; i++)
                            {
                                autoComplete += autoCompleteList[i] + "\n";
                            }
                        }

                        // Calculate the auto complete width and height
                        string[] words = autoComplete.Split('\n');
                        string longest = words.OrderByDescending(s => s.Length).First();
                        Vector2 size = GUILayoutUtility.GetRect(new GUIContent(longest), GUIStyle.none).size;
                        float width = size.x + 8;
                        float height = (size.y * words.Length) - 8;

                        // If log message fullscreen place it ontop
                        switch(logMessagesHeightIndex)
                        {
                            case 2: y = y - height - 4; break;
                            default: y += 28; break;
                        }
                               
                        // Draw auto complete suggestions
                        DrawQuad(new Rect(4, y, width, height), ref textureBlack);
                        autoCompleteScroll = GUI.BeginScrollView(new Rect(0, y, width, height), autoCompleteScroll, new Rect(0, y, width, height));
                        GUI.Label(new Rect(8, y, width, height), autoComplete);
                        GUI.EndScrollView();

                        // Auto complete in console bar
                        if(inputHasChanged || newAutoCompleteIndex)
                        {
                            if(newAutoCompleteIndex) newAutoCompleteIndex = false;
                            int charLoc = autoCompleteList[autoCompleteIndex].IndexOf(" ", System.StringComparison.Ordinal);
                            string s = autoCompleteList[autoCompleteIndex].Substring(0, charLoc);
                            s = s.Substring(Mathf.Clamp(input.Length, 0, s.Length), (s.Length) - input.Length);
                            autoCompleteBar = "<color=#ffffff00>" + input + "</color>" + "<color=yellow>" + s + "</color>";
                        }
                        // If log message fullscreen place it ontop
                        switch(logMessagesHeightIndex)
                        {
                            case 2: y = Screen.height - 24; break;
                            default: y -= 28; break;
                        }
                        GUI.Label(new Rect(4, y, Screen.width, 24), autoCompleteBar);

                        y = preY;
                    }
                }
            }

            // Handle input
            if(Event.current.isKey)
            {
                // Enter command
                if(Event.current.keyCode == KeyCode.Return)
                {
                    if(!buttonPressedEnter)
                    {
                        buttonPressedEnter = true;
                        OnInputReturn();
                    }
                    if(Event.current.type == EventType.KeyUp) buttonPressedEnter = false;
                }

                if(Event.current.keyCode == KeyCode.UpArrow)
                {
                    // Show last command
                    if(!buttonPressedArrowUp)
                    {
                        buttonPressedArrowUp = true;
                        if(input == "")
                        {
                            input = Instance.previousCommand;
                            GUI.FocusControl("consoleBar");
                        }
                        else
                        {
                            if(autoCompleteIndex > 0)
                            {
                                autoCompleteIndex--;
                                newAutoCompleteIndex = true;
                            }
                        }
                    }
                    if(Event.current.type == EventType.KeyUp)
                    {
                        buttonPressedArrowUp = false;
                        // Move cursor to end
                        if(input == Instance.previousCommand)
                        {
                            textEditor.cursorIndex = input.Length;
                            textEditor.selectIndex = input.Length;
                        }
                    }
                }

                if(Event.current.keyCode == KeyCode.DownArrow)
                {
                    if(!buttonPressedArrowDown)
                    {
                        buttonPressedArrowDown = true;
                        if(autoCompleteIndex < autoCompleteList.Count - 1)
                        {
                            autoCompleteIndex++;
                            newAutoCompleteIndex = true;
                        }
                    }
                    if(Event.current.type == EventType.KeyUp)
                    {
                        buttonPressedArrowDown = false;
                    }
                }

                // TAB auto complete
                if(Event.current.keyCode == KeyCode.Tab && autoCompleteList.Count > 0)
                {
                    if(!buttonPressedTab)
                    {
                        buttonPressedTab = true;
                        int charLoc = autoCompleteList[autoCompleteIndex].IndexOf(" ", System.StringComparison.Ordinal);
                        input = autoCompleteList[autoCompleteIndex].Substring(0, charLoc);
                        // Move type cursor to end of string

                        // cant seem to move cursor to the end in here
                    }
                    if(Event.current.type == EventType.KeyUp)
                    {
                        buttonPressedTab = false;
                        // Move cursor to end
                        textEditor.cursorIndex = input.Length;
                        textEditor.selectIndex = input.Length;
                    }
                }
            }

            OnGUIConsoleExtend?.Invoke();
        }

        /// <summary>
        /// When the user wants the input to be processed
        /// </summary>
        public static void OnInputReturn()
        {
            if(Instance.show)
            {
                Instance.HandleInput();
            }
        }

        /// <summary>
        /// Output a new log message to the debug console
        /// </summary>
        /// <param name="s">The string message</param>
        /// <param name="autoScroll">Is this function gonna be called multiple times in a for loop? (prevent scrolling)</param>
        public static void Log(string s, bool autoScroll = true)
        {
            // Limit logMessage amount
            if(Instance.logMessagesAmount + 1 > Instance.logMessagesMaxAmount)
            {
                Instance.logMessages = Instance.logMessages.Substring(Instance.logMessages.IndexOf("\n") + 1); // Important only checking for \n and not System.Environment.NewLine
            }
            else
            {
                Instance.logMessagesAmount++;
                Instance.logMessagesHeight += Instance.lineHeight;
                if(autoScroll) Instance.logMessageScroll.y += Instance.lineHeight;
            }

            // Add message
            Instance.logMessages += "> " + s + "\n";            
        }

        /// <summary>
        /// Toggle the debug console visabillity
        /// </summary>
        public static void ToggleShow()
        {
            Instance.show = !Instance.show;
        }

        /// <summary>
        /// Draws a gui box with no smoothed corners
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color"></param>
        private void DrawQuad(Rect position, ref Texture2D tex)
        {
            textureDefault = GUI.skin.box.normal.background;
            GUI.skin.box.normal.background = tex;
            GUI.Box(position, GUIContent.none);
            GUI.skin.box.normal.background = textureDefault;
        }

        /// <summary>
        /// Handles the input from the user send to the debug console
        /// </summary>
        private void HandleInput()
        {
            if(string.IsNullOrEmpty(input)) return;
            
            Instance.previousCommand = input;
            string[] properties = input.Split(' ');

            // Find out what command is used
            bool validCommand = false;
            for(int i = 0; i < commands.Count; i++)
            {
                DebugCommandBase commandBase = commands[i] as DebugCommandBase;

                // Check if the input string contains the command id
                if(properties[0].ToLower() == commandBase.ID.ToLower())
                {
                    validCommand = true;

                    // Process it as the right DebugCommand type (This is where you can add code that tells how to handle custom types)
                    if(commands[i] as DebugCommand != null)
                    {
                        // Invoke command
                        (commands[i] as DebugCommand).Invoke();
                    }
                    else if(commands[i] as DebugCommand<bool> != null)
                    {
                        int v = -1;                        
                        if(properties.Length != 2) Log("[Debug Console] Error: expected 2 arguments, got " + properties.Length);
                        else if(properties[1] == "true") v = 1;
                        else if(properties[1] == "false") v = 0;
                        else
                        {
                            Log("[Debug Console] Error: expected bool value in command, got " + properties[1]);
                        }

                        if(v != -1)
                        {
                            bool b = v == 1;
                            (commands[i] as DebugCommand<bool>).Invoke(b);
                        }
                    }
                    else if(commands[i] as DebugCommand<int> != null)
                    {
                        if(properties.Length != 2)
                        {
                            Log("[Debug Console] Error: expected 2 arguments, got " + properties.Length);
                        }
                        else
                        {
                            int v = 0;
                            if(int.TryParse(properties[1], out v))
                            {
                                (commands[i] as DebugCommand<int>).Invoke(v);
                            }
                            else
                            {
                                Log("[Debug Console] Error: expected int value in command, got " + properties[1]);
                            }
                        }
                    }
                    else if(commands[i] as DebugCommand<float> != null)
                    {
                        if(properties.Length != 2)
                        {
                            Log("[Debug Console] Error: expected 2 arguments, got " + properties.Length);
                        }
                        else
                        {
                            float v = 0;
                            if(float.TryParse(properties[1], out v))
                            {
                                (commands[i] as DebugCommand<float>).Invoke(v);
                            }
                            else
                            {
                                Log("[Debug Console] Error: expected float value in command, got " + properties[1]);
                            }
                        }
                    }
                    else if(commands[i] as DebugCommand<string> != null)
                    {
                        if(properties.Length != 2)
                        {
                            Log("[Debug Console] Error: expected 2 arguments, got " + properties.Length);
                        }
                        else
                        {
                            int from = input.IndexOf('"') + 1;
                            int to = input.LastIndexOf('"');
                            string s = input.Substring(from, to - from);
                            (commands[i] as DebugCommand<string>).Invoke(s);
                        }
                    }
                    else if(commands[i] as DebugCommand<Vector2> != null)
                    {
                        if(properties.Length != 3)
                        {
                            Log("[Debug Console] Error: expected 3 arguments, got " + properties.Length);
                        }
                        else
                        {
                            float x = 0, y = 0;
                            if(float.TryParse(properties[1], out x) && float.TryParse(properties[2], out y))
                            {
                                (commands[i] as DebugCommand<Vector2>).Invoke(new Vector2(x, y));
                            }
                            else
                            {
                                Log("[Debug Console] Error: <vector2> not passed in correctly, got " + properties[1] + " " + properties[2]);
                            }
                        }
                    }
                    else if(commands[i] as DebugCommand<Vector3> != null)
                    {
                        if(properties.Length != 4)
                        {
                            Log("[Debug Console] Error: expected 4 arguments, got " + properties.Length);
                        }
                        else
                        {
                            float x = 0, y = 0, z = 0;
                            if(float.TryParse(properties[1], out x) && float.TryParse(properties[2], out y) && float.TryParse(properties[3], out z))
                            {
                                (commands[i] as DebugCommand<Vector3>).Invoke(new Vector3(x, y, z));
                            }
                            else
                            {
                                Log("[Debug Console] Error: <vector3> not passed in correctly, got " + properties[1] + " " + properties[2] + " " + properties[3]);
                            }
                        }
                    }
                    else if(commands[i] as DebugCommand<Tuple<string, string>> != null)
                    {
                        if(properties.Length != 3)
                        {
                            Log("[Debug Console] Error: expected 3 arguments, got " + properties.Length);
                        }
                        else
                        {
                            (commands[i] as DebugCommand<Tuple<string,string>>).Invoke(Tuple.Create(properties[1], properties[2]));
                        }
                    }
                    else if(commands[i] as DebugCommand<Tuple<string, string, string>> != null)
                    {
                        if(properties.Length != 4)
                        {
                            Log("[Debug Console] Error: expected 4 arguments, got " + properties.Length);
                        }
                        else
                        {
                            (commands[i] as DebugCommand<Tuple<string, string, string>>).Invoke(Tuple.Create(properties[1], properties[2], properties[3]));
                        }
                    }
                    else if(commands[i] as DebugCommand<Tuple<string, string, string, string>> != null)
                    {
                        if(properties.Length != 5)
                        {
                            Log("[Debug Console] Error: expected 5 arguments, got " + properties.Length);
                        }
                        else
                        {
                            (commands[i] as DebugCommand<Tuple<string, string, string, string>>).Invoke(Tuple.Create(properties[1], properties[2], properties[3], properties[4]));
                        }
                    }
                    else if(commands[i] as DebugCommand<Tuple<string, string, string, string, string>> != null)
                    {
                        if(properties.Length != 6)
                        {
                            Log("[Debug Console] Error: expected 6 arguments, got " + properties.Length);
                        }
                        else
                        {
                            (commands[i] as DebugCommand<Tuple<string, string, string, string, string>>).Invoke(Tuple.Create(properties[1], properties[2], properties[3], properties[4], properties[5]));
                        }
                    }
                    else if(commands[i] as DebugCommand<Tuple<string, string, string, string, string, string>> != null)
                    {
                        if(properties.Length != 7)
                        {
                            Log("[Debug Console] Error: expected 7 arguments, got " + properties.Length);
                        }
                        else
                        {
                            (commands[i] as DebugCommand<Tuple<string, string, string, string, string, string>>).Invoke(Tuple.Create(properties[1], properties[2], properties[3], properties[4], properties[5], properties[6]));
                        }
                    }
                    else if(commands[i] as DebugCommand<Tuple<string, string, string, string, string, string, string>> != null)
                    {
                        if(properties.Length != 8)
                        {
                            Log("[Debug Console] Error: expected 8 arguments, got " + properties.Length);
                        }
                        else
                        {
                            (commands[i] as DebugCommand<Tuple<string, string, string, string, string, string, string>>).Invoke(Tuple.Create(properties[1], properties[2], properties[3], properties[4], properties[5], properties[6], properties[7]));
                        }
                    }
                    else if(commands[i] as DebugCommand<object> != null)
                    {
                        if(properties.Length != 2)
                        {
                            Log("[Debug Console] Error: expected 2 arguments, got " + properties.Length);
                        }
                        else
                        {
                            (commands[i] as DebugCommand<object>).Invoke(properties[1]);
                        }
                    }
                    else
                    {
                        Log("[Debug Console] Error: command recognised but cannot invoke it (corresponding handle statement not found in DebugConsole.HandleInput())");
                    }
                }
            }

            if(!validCommand)
            {
                Log("[Debug Console] Error: command \"" + input + "\" not recognised");
            }

            // Reset the input value
            input = "";
        }        
    }
}