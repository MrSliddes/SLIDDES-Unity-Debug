# SLIDDES-Unity-Debug
A collection for debugging in Unity with an in game debug console.
SLIDDES © 2021

## About
Hello and thank you for using SLIDDES Software.
SLIDDES Unity Debug is a collection for debugging in Unity. It currently contains:
- Debug Console, for debugging in editor or in build. Easily add custom commands to it for your personal needs.

## Installation
You can install it as a package for Unity.

For more information on how to install it:
https://docs.unity3d.com/Manual/upm-ui-giturl.html

## How To Use It
1. After installing add an empty GameObject to the scene. Name it Debug Console (or whatever you like).
2. Click on the GameObject > Go to the inspector view > click Add Component and add the DebugConsole script.
3. You can now access the Debug Console in play-mode or build with ` / ~ / F6.
4. You can type 'help' in the console bar for more information and 'commands' for a list of all commands availabe
6. Thats it!
Note: There can only be 1 DebugConsole per scene, the DebugConsole handles this itself by destroying itself if another
DebugConsole is already active in the scene. You can add a DebugConsole for every scene if you like or just 1 in your
build index 0 scene (your start scene) since the DebugConsole is in DontDestroyOnLoad.

![Img DebugConsole 0](https://github.com/MrSliddes/SLIDDES-Unity-Debug/blob/Github-Info/SLIDDES_Unity_Debug_Img_0.jpg)
How the console first looks when you open it.

![Img DebugConsole 1](https://github.com/MrSliddes/SLIDDES-Unity-Debug/blob/Github-Info/SLIDDES_Unity_Debug_Img_1.jpg)
Typing in a command complete with autocomplete.

![Img DebugConsole 2](https://github.com/MrSliddes/SLIDDES-Unity-Debug/blob/Github-Info/SLIDDES_Unity_Debug_Img_2.jpg)
Expanded console view with a list of availabe commands

## Controls list
- [` / ~ / F6] Toggle DebugConsole
- [TAB] Autocomplete suggested text in yellow
- [Arrow Up] If no input in console set console input to last submitted command
- [Arrow Up / Arrow Down] Select through suggested commands when available

## Other
For more information or contact, go to https://sliddes.com/
