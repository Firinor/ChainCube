using UnityEngine;

[CreateAssetMenu(menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    public int PunchForce;
    public int UpForce;
    public float SpreadForce;
    [Space]
    public float CubeBoard;
    [Space]
    public float CubeReloadTime;
    public float Sensitivity;
}