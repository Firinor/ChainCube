using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CubeStartPosition")]
public class InitialPlacement : ScriptableObject
{
    public List<CubeWithPosition> CubeWithPosition;
}
