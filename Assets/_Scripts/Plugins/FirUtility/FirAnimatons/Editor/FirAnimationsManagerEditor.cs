using UnityEditor;
using UnityEngine;

namespace FirAnimations
{
    [CustomEditor(typeof(FirAnimationsManager))]
    public class FirAnimationsManagerEditor : Editor
    {
        private bool isPlaying;

        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
            {
                DrawDefaultInspector();
                return;
            }

            FirAnimationsManager script = (FirAnimationsManager)target;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("<<"))
            {
                Off();
                script.ToStartPoint();
            }

            if (GUILayout.Button(isPlaying ? "ll" : ">"))
            {
                if (isPlaying)
                {
                    Off();
                }
                else
                {
                    EditorApplication.update += script.Update;
                    isPlaying = true;
                    script.OnEndAllAnimations += Off;
                    //script.Initialize();
                    script.StartAnimations();
                }
            }

            if (GUILayout.Button(">>"))
            {
                Off();
                script.ToEndPoint();
            }

            EditorGUILayout.EndHorizontal();
            DrawDefaultInspector();
        }

        private void Off()
        {
            FirPositionAnimation script = (FirPositionAnimation)target;

            isPlaying = false;
            script.OnComplete -= Off;
            EditorApplication.update -= script.Update;
        }
    }
}