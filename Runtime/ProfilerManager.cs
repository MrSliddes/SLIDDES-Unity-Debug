using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;

namespace SLIDDES.Debug
{
    public class ProfilerManager : MonoBehaviour
    {
		private string statsText;

		// Profile recorders
		private ProfilerRecorder systemMemoryRecorder;
		private ProfilerRecorder gcMemoryRecorder;
		private ProfilerRecorder mainThreadTimeRecorder;
		private ProfilerRecorder drawCallsRecorder;
				
		void OnEnable()
		{
			systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
			gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
			mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
			drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
		}

		void OnDisable()
		{
			systemMemoryRecorder.Dispose();
			gcMemoryRecorder.Dispose();
			mainThreadTimeRecorder.Dispose();
			drawCallsRecorder.Dispose();
		}

		void Update()
		{
			var sb = new StringBuilder(500);
			sb.AppendLine("[ProfileManager] Use arrow keys to move up / down");
			sb.AppendLine($"Frame Time: {GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-6f):F1} ms");
			sb.AppendLine($"GC Memory: {gcMemoryRecorder.LastValue / (1024 * 1024)} MB");
			sb.AppendLine($"System Memory: {systemMemoryRecorder.LastValue / (1024 * 1024)} MB");
			sb.AppendLine($"Draw Calls: {drawCallsRecorder.LastValue}");
			statsText = sb.ToString();
		}

		void OnGUI()
		{
			GUIStyle gUIStyle = new GUIStyle();
			Vector2 v = gUIStyle.CalcSize(new GUIContent(statsText));
			GUI.TextArea(new Rect(10, 30, v.x + 32, v.y + 32), statsText);
		}

		private double GetRecorderFrameAverage(ProfilerRecorder recorder)
		{
			var samplesCount = recorder.Capacity;
			if(samplesCount == 0)
				return 0;

			double r = 0;
			var samples = new List<ProfilerRecorderSample>(samplesCount);
			recorder.CopyTo(samples);
			for(var i = 0; i < samples.Count; ++i)
				r += samples[i].Value;
			r /= samplesCount;

			return r;
		}
	}
}