using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.Debugging
{
    /// <summary>
    /// Base class for DebugCommand
    /// </summary>
    public class DebugCommandBase
    {
        /// <summary>
        /// The id of the command
        /// </summary>
        public string ID { get { return id; } }
        /// <summary>
        /// The description of the command
        /// </summary>
        public string Description { get { return description; } }
        /// <summary>
        /// The format of the command that tells the user how to format the command (how to write it)
        /// </summary>
        public string Format { get { return format; } }

        private string id;
        private string description;
        private string format;

        public DebugCommandBase(string id, string description, string format)
        {
            this.id = id;
            this.description = description;
            this.format = format;
        }
    }

    /// <summary>
    /// A normal DebugCommand
    /// </summary>
    public class DebugCommand : DebugCommandBase
    {
        private Action command;

        /// <summary>
        /// The DebugCommand constructor
        /// </summary>
        /// <param name="id">The id of the command, the name that needs to be typed in the debug console bar in order to execute this command</param>
        /// <param name="description">The description of the command, to display information on what the command does</param>
        /// <param name="format">The format of the command (how to type it in the bar with arguments)</param>
        /// <param name="command">Reference to the command that gets executed as an Action</param>
        public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
        {
            this.command = command;
        }

        public void Invoke()
        {
            command.Invoke();
        }
    }

    /// <summary>
    /// A DebugCommand with 1 type (for multiple types use DebugCommand<Tuple<t1,t2,t3>> (up to 8 types))
    /// </summary>
    /// <typeparam name="T1">Type 1</typeparam>
    public class DebugCommand<T1> : DebugCommandBase
    {
        private Action<T1> command;

        public DebugCommand(string id, string description, string format, Action<T1> command) : base(id, description, format)
        {
            this.command = command;
        }

        public void Invoke(T1 t1)
        {
            command.Invoke(t1);
        }
    }
}