using UnityEngine;
using UnityEditor;

public class QuickScreenshot : MonoBehaviour
{
    [MenuItem("Tools/Screenshot")]
    static void QuickScreenshotSet()
    {
        int width = 1080;
        int height = 1920;
        
        string path = EditorUtility.SaveFilePanel(
            "QuickScreenshotSet",
            "",
            $"Screenshot_{width}x{height}_{System.DateTime.Now:yyyyMMdd_HHmmss}.png",
            "png");
        
        if (!string.IsNullOrEmpty(path))
        {
            ScreenCapture.CaptureScreenshot(path);
            Debug.Log($"QuickScreenshotSet: {path}");
        }
    }
}