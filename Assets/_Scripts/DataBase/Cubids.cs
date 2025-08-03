using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cubid")]
public class Cubids : ScriptableObject
{
    public Cube CubePrefab;
    public Cube SpherePrefab;
    public Cube BombPrefab;

    public List<Material> CubeMaterials;
    public List<Material> SphereMaterials;

    public Material ghostMaterial;

    public Material RainbowCubeMaterial;
    public Material RainbowSphereMaterial;
}