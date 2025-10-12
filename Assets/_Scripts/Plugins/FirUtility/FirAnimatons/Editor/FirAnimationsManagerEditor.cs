using FirAnimations;
using UnityEditor;
using UnityEngine;

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
        if (GUILayout.Button(isPlaying?"ll":">"))
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


[CustomEditor(typeof(FirPositionAnimation))]
public class FirPositionAnimationEditor : Editor
{
    private bool isPlaying;

    public override void OnInspectorGUI()
    {
        if (EditorApplication.isPlaying)
        {
            DrawDefaultInspector();
            return;
        }
        
        FirPositionAnimation script = (FirPositionAnimation)target;
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("<<"))
        {
            Off();
            script.ToStartPoint();
        }
        if (GUILayout.Button(isPlaying?"ll":">"))
        {
            if (isPlaying)
            {
                Off();
            }
            else
            {
                EditorApplication.update += script.Update;
                isPlaying = true;
                script.OnComplete += Off;
                script.Initialize();
                script.ToStartPoint();
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
[CustomEditor(typeof(FirZoomAnimation))]
public class FirZoomAnimationEditor : Editor
{
    private bool isPlaying;

    public override void OnInspectorGUI()
    {
        if (EditorApplication.isPlaying)
        {
            DrawDefaultInspector();
            return;
        }
        
        FirZoomAnimation script = (FirZoomAnimation)target;
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("<<"))
        {
            Off();
            script.ToStartPoint();
        }
        if (GUILayout.Button(isPlaying?"ll":">"))
        {
            if (isPlaying)
            {
                Off();
            }
            else
            {
                EditorApplication.update += script.Update;
                isPlaying = true;
                script.OnComplete += Off;
                script.Initialize();
                script.ToStartPoint();
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
        FirZoomAnimation script = (FirZoomAnimation)target;
        
        isPlaying = false;
        script.OnComplete -= Off;
        EditorApplication.update -= script.Update;
    }
}