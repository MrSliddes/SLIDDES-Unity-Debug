using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLIDDES.Debugging; // 1. Important to add SLIDDES.Debugging to make use of the debug classes

namespace SLIDDES.Debugging // 1.1 or use SLIDDES.Debugging namespace for access to CustomCommand
{
    /// <summary>
    /// 2. Example class of how to write custom commands
    /// </summary>
    /// <seealso cref="DebugCommandsBuildIn"/> // 3. For more examples you can take a peek in this class (use commands with variables like bool/int/float or a custom type)
    public class SampleCustomCommand : CustomCommand // 4. Derive from CustomCommand. It doesn't matter where the file is located as it gets it from the base CustomCommand class
    {
        public static DebugCommand CUSTOM_COMMAND; // 5. You can have multiple DebugCommands in 1 class if you want or a different class for each command
                                                   //public static DebugCommand<bool> CUSTOM_COMMAND_WITH_BOOL;
                                                   //public static DebugCommand<int> CUSTOM_COMMAND_WITH_INT;
                                                   //public static DebugCommand<float> CUSTOM_COMMAND_WITH_FLOAT;
                                                   //public static DebugCommand<vector3> CUSTOM_COMMAND_WITH_V3;
                                                   //public static DebugCommand<CustomType> CUSTOM_COMMAND_WITH_CUSTOM_TYPE; -> if you want to handle a custom type you will have to handle it at DebugConsole.HandleInput() -> if else statement (commands[i] as DebugCommand != null)
       

        public override void Invoke() // 6. You need to overide the Invoke methode where you can instantiate the DebugCommands
        {
            
            // The DebugCommand constructor
            // arg0 The id of the command, the name that needs to be typed in the debug console bar in order to execute this command</param>
            // arg1 The description of the command, to display information on what the command does</param>
            // arg2 The format of the command (how to type it in the bar with arguments)</param>
            // arg3 Reference to the command that gets executed as an Action</param>
            CUSTOM_COMMAND = new DebugCommand("sample_custom_command_1", "An example of an custom command that does x", "sample_custom_command_1", () =>
            {
                // 7. Your command code goes in here
                UnityEngine.Debug.Log("Executed sample_custom_command");
            });

            // ! Important ! 8. needs to be after assigning DebugCommands & dont forget to add your DebugCommand after creating it here too
            commands = new List<object>()
            {
                CUSTOM_COMMAND
            };
        }

        // 9. Note, this script is located in a /Samples/ folder so it wont show up in runtime, move it into the Asset/Scripts/ folder to see it (it doesn't have to be specifically the Asset/Scripts/ folder)
    }
}