using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.Debugging
{
    /// <summary>
    /// Base class for custom commands
    /// </summary>
    public class CustomCommand
    {
        public List<object> commands;

        /// <summary>
        /// When the command gets invoked
        /// </summary>
        public virtual void Invoke() { }
    }
}