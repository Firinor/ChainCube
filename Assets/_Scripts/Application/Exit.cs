using UnityEditor;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void OnClickExit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
