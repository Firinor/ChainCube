using UnityEngine;

public class Laggs : MonoBehaviour
{
    void Start()
    {
        To10();
    }
    [ContextMenu("To10")]
    void To10()
    {
        Application.targetFrameRate = 10;
    }
    [ContextMenu("ToMax")]
    void ToMax()
    {
        Application.targetFrameRate = 0;
    }
}
