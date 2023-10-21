using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SLIDDES.Debug
{
    /// <summary>
    /// For taking a screenshot
    /// </summary>
    public class CMD_SCREENSHOT : CustomCommand
    {        
        public static DebugCommand SCREENSHOT;

        public override void Invoke()
        {
            SCREENSHOT = new DebugCommand("screenshot", "Take a screenshot", "screenshot", () =>
            {
                string folder = string.Format("{0}/{1}/{2}", Application.persistentDataPath, Application.companyName, Application.productName);
                Directory.CreateDirectory(folder);
                string path = string.Format("{0}/{1}.png", folder, System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss-ff"));
                ScreenCapture.CaptureScreenshot(path);
                DebugConsole.Log("[DebugConsole] Took screenshot and saved it at: " + path);
                DebugConsole.Log(""); // Since path is longer
            });


            commands = new List<object>()
            {
                SCREENSHOT
            };
        }
    }
}