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
    public Cube Bomb {get; private set;}

    private float minSize;
    private float scaleStep;

    private int index;

    [Header("VFX")]
    [SerializeField]
    private ParticleSystem[] boom;

    [Header("SFX")]
    [SerializeField]
    private CubeSounds popSound;
    [SerializeField]
    private BoomSound boomSound;
    
    [Inject]
    private void Initialize(Cubids cubids)
    {
        FindAllCubesOnScene();
        
        CubePrefab = cubids.CubePrefab;
        SpherePrefab = cubids.SpherePrefab;
        CreateBomb(cubids);

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

        minSize = cubids.MinSizeScale;
        scaleStep = cubids.StepSizeScale;
    }

    private void CreateBomb(Cubids cubids)
    {
        Bomb = container.InstantiatePrefabForComponent<Cube>(cubids.BombPrefab, transform);
        Bomb.Remove += ToPool;
        ToPool(Bomb);
    }

    private void FindAllCubesOnScene()
    {
        foreach (var cube in FindObjectsByType<Cube>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            cube.Remove += ToPool;
            cube.RefreshView += RefreshView;
            if (!cube.gameObject.activeSelf)
                ToPool(cube);
        }
    }

    public void ToPool(Cube cube)
    {
        cubes.Add(cube);
        cube.gameObject.SetActive(false);
    }
    
    public Cube Create(object param)
    {
        CubeWithPosition cubeParam = param as CubeWithPosition;
        
        Cube newCube = cubes.Find(c => c.form == cubeParam.Cubid.Form);

        if (newCube is null)
        {
            if(cubeParam.Cubid.Form == ECubeForm.Cube)
                newCube = container.InstantiatePrefabForComponent<Cube>(CubePrefab, transform);
            else if(cubeParam.Cubid.Form == ECubeForm.Sphere)
                newCube = container.InstantiatePrefabForComponent<Cube>(SpherePrefab, transform);
            else
                throw new Exception();

            newCube.name += index++;
            newCube.Remove += ToPool;
            newCube.RefreshView += RefreshView;
        }

        newCube.Score = (int)cubeParam.Cubid.Score;
        newCube.form = cubeParam.Cubid.Form;
        newCube.CollideEffect = new NormalCube();
        RefreshView(newCube);
        float deltaY = newCube.transform.localScale.x/2;
        newCube.transform.position = cubeParam.Position + Vector3.up * deltaY;
        cubes.Remove(newCube);
        return newCube;
    }

    public void RefreshView(Cube cube)
    {
        if(cube.form == ECubeForm.Bomb) 
            return;
        
        Cubid materialIndex = new()
        {
            Score = (ECube)cube.Score,
            Form = cube.form
        };
        cube.SetMaterial(materials[materialIndex]);
        //int value = 8;
        //string binary = Convert.ToString(value, 2);
        //Which returns 1000.
        string binary = Convert.ToString(cube.Score, 2);
        float scaleByScore = minSize + scaleStep * binary.Length;
        cube.transform.localScale = Vector3.one * scaleByScore;
    }

    public Cube CreateBomb(Cube fromCube)
    {
        Bomb bombCollide = new Bomb(fromCube);
        Bomb.CollideEffect = bombCollide;
        bombCollide.BoomAction += pos =>
        {
            foreach (var particleSystem in boom)
            {
                particleSystem.Stop();
                particleSystem.gameObject.SetActive(false);
                particleSystem.transform.position = pos;
                particleSystem.gameObject.SetActive(true);
            }
        };
        bombCollide.BoomAction += pos =>
        {
            boomSound.transform.position = pos;
            boomSound.Play();
        };
        cubes.Add(Bomb);
        Bomb.gameObject.SetActive(false);
        return Bomb;
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
