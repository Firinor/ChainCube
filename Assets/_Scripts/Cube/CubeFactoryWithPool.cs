using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class CubeFactoryWithPool: MonoBehaviour, IFactory<object, Cube>
{
    [SerializeField]
    private List<Cube> cubes = new();
    [SerializeField]
    private Dictionary<Cubid, Material> materials = new();

    [Inject] 
    private DiContainer container;
    
    private Cube CubePrefab;
    private Cube SpherePrefab;
    private Cube Bomb;

    [Inject]
    private void Initialize(Cubids cubids)
    {
        CubePrefab = cubids.CubePrefab;
        SpherePrefab = cubids.SpherePrefab;
        Bomb = container.InstantiatePrefabForComponent<Cube>(cubids.BombPrefab, transform);
        Bomb.ToPool();

        Cubid GhostCube = new()
        {
            Score = ECube.Ghost,
            Form = ECubeForm.Cube
        };
        materials.Add(GhostCube, cubids.ghostMaterial);
        Cubid GhostSphere = new()
        {
            Score = ECube.Ghost,
            Form = ECubeForm.Sphere
        };
        materials.Add(GhostSphere, cubids.ghostMaterial);
        
        for (int i = 0; i < cubids.CubeMaterials.Count; i++)
        {
            Cubid Cube = new()
            {
                Score = (ECube)(2<<i),
                Form = ECubeForm.Cube
            };
            materials.Add(Cube, cubids.CubeMaterials[i]);
            Cubid Sphere = new()
            {
                Score = (ECube)(2<<i),
                Form = ECubeForm.Sphere
            };
            materials.Add(Sphere, cubids.SphereMaterials[i]);
        }
    }
    
    public Cube Create(object param)
    {
        CubeWithPosition cubeParam = param as CubeWithPosition;
        
        Cube newCube = cubes.Find(
            b => !b.IsInGame
            && b.form == cubeParam.Cubid.Form);

        if (newCube is null)
        {
            if(cubeParam.Cubid.Form == ECubeForm.Cube)
                newCube = container.InstantiatePrefabForComponent<Cube>(CubePrefab, transform);
            else if(cubeParam.Cubid.Form == ECubeForm.Sphere)
                newCube = container.InstantiatePrefabForComponent<Cube>(SpherePrefab, transform);
            else
                throw new Exception();

            cubes.Add(newCube);
        }

        SetCubeParams(newCube, cubeParam.Cubid);
        float deltaY = newCube.transform.localScale.x/2;
        newCube.transform.position = cubeParam.Position + Vector3.up * deltaY;
        
        newCube.IsInGame = true;

        return newCube;
    }

    public void SetCubeParams(Cube cube, Cubid cubid)
    {
        cube.SetScore(cubid.Score);
        cube.SetMaterial(materials[cubid]);
        //int value = 8;
        //string binary = Convert.ToString(value, 2);
        //Which returns 1000.
        string binary = Convert.ToString((int)cubid.Score, 2);
        float scaleByScore = 1 + 0.1f * binary.Length;
        cube.transform.localScale = Vector3.one * scaleByScore;
    }

    public void AddToList(Cube cube)
    {
        if (!cubes.Contains(cube))
            cubes.Add(cube);
    }
    public void ClearAll()
    {
        foreach(Cube cube in cubes)
        {
            if(cube != null)
                Destroy(cube.gameObject);
        }
        cubes.Clear();
        if (transform.childCount > 0)
            DestroyAllChild();
        GC.Collect();
    }
    private void DestroyAllChild()
    {
        int i = transform.childCount;
        while (i > 0)
        {
            i--;
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void OnDestroy()
    {
        ClearAll();
    }
}
