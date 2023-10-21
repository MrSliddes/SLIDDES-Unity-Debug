# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)

## [1.3.0] - 2023-10-21
### Added
- ProfilerManager.cs for profiling in editor / build

## [1.2.0] - 2021-11-13
### Added
- Executed command now shows up in DebugConsole.Log
- Commands Build In folder
- Command log_files for editor/package manager/player (windows only)
- Command screenshot for taking a screenshot

### Changed
- Sample folder intergration

### Fixed
- License warning of not having a meta file

## [1.1.1] - 2021-10-29
### Changed
- Renamed SLIDDES.Debug to SLIDDES.Debugging to avoid Unity namespace error UnityEngine.Debug

## [1.1.0] - 2021-07-24
### Added
- How to close text to console bar
- Command 'quit', Exit application / playmode
- Command 'log_messages_clear', Clear all log messages from DebugConsole
- Command 'fps_limit', Set a limit to max fps
- Command 'fps_vsynccount', Set QualitySettings.vSyncCount
- Command 'system_info', display information about system
- Command 'camera_fov', set the main camera fieldOfView
- Command 'ui_draw', toggle drawing ui
- Command 'debugconsole_autocompletemode', change the way autocomplete suggests commands

### Changed
- Command 'draw_fps' renamed to 'fps_draw'
- Command 'show_log_messages' renamed to 'log_messages_draw'
- Command 'show_autocomplete' renamed to 'debugconsole_autocomplete'
- Command 'show_unity_log' renamed to 'debug_log_link'
- Command 'restart_scene' renamed to 'scene_restart'
- Command 'load_scene' renamed to 'scene_load'

## [1.0.0] - 2021-07-23
### Added
- First public release