using UnityEditor;
using UnityEngine;

namespace oculog.Core
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class PlayStateNotifier
    {
        public static bool ShouldNotExit { get; set; }

        public delegate void EditorExitEvents();
        public static EditorExitEvents editorExitEvents;

        static PlayStateNotifier()
        {
            EditorApplication.playModeStateChanged += ModeChanged;
        }

        private static void ModeChanged(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingPlayMode || !ShouldNotExit) return;
            EditorApplication.isPlaying = true;
            editorExitEvents();
        }
    }
#endif
}