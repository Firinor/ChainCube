using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CubeFactoryWithPool: MonoBehaviour, IFactory<object, Cube>
{
    [SerializeField]
    private List<Cube> cubes = new();
    [SerializeField]
    private Dictionary<Cubid, Material> materials = new();

    [Inject] 
    private DiContainer container;
    [Inject(Id = "PlayerCubeAnchor")]
    private Transform playerHand;

    private Cube CubePrefab;
    private Cube SpherePrefab;
    private Cube BombPrefab;
    
    public List<Cube> Bombes = new();

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
        BombPrefab = cubids.BombPrefab;
        CreateBomb();

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
        
        Cubid RainbowCube = new()
        {
            Score = ECube.Rainbow,
            Form = ECubeForm.Cube
        };
        materials.Add(RainbowCube, cubids.RainbowCubeMaterial);
        Cubid RainbowSphere = new()
        {
            Score = ECube.Rainbow,
            Form = ECubeForm.Sphere
        };
        materials.Add(RainbowSphere, cubids.RainbowSphereMaterial);
        
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

    private Cube CreateBomb()
    {
        Cube newBomb = container.InstantiatePrefabForComponent<Cube>(BombPrefab, playerHand);
        newBomb.Remove += ToBombPool;
        ToBombPool(newBomb);
        return newBomb;
    }

    private void FindAllCubesOnScene()
    {
        foreach (var cube in FindObjectsByType<Cube>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if(cube.transform.parent != playerHand)
                cube.transform.SetParent(transform);
            cube.Remove += ToPool;
            cube.RefreshView += RefreshView;
            if (!cube.gameObject.activeSelf)
                ToPool(cube);
        }
    }

    private void ToBombPool(Cube bomb)
    {
        Bombes.Add(bomb);
        bomb.gameObject.SetActive(false);
    }
    
    public void ToPool(Cube cube)
    {
        cubes.Add(cube);
        cube.gameObject.SetActive(false);
    }
    
    public Cube Create(object param)
    {
        Cubid cubeParam = (Cubid)param;
        
        Cube newCube = cubes.Find(c => !c.gameObject.activeSelf && c.form == cubeParam.Form);

        if (newCube is null)
        {
            if(cubeParam.Form == ECubeForm.Cube)
                newCube = container.InstantiatePrefabForComponent<Cube>(CubePrefab, playerHand);
            else if(cubeParam.Form == ECubeForm.Sphere)
                newCube = container.InstantiatePrefabForComponent<Cube>(SpherePrefab, playerHand);
            else
                throw new Exception();

            newCube.name += index++;
            newCube.Remove += ToPool;
            newCube.RefreshView += RefreshView;
        }

        newCube.Score = (int)cubeParam.Score;
        newCube.form = cubeParam.Form;
        newCube.CollideEffect = new NormalCube();
        RefreshView(newCube);
        ToPlayerHand(newCube);
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
        
        if(cube.Score <= 0) return;
        //int value = 8;
        //string binary = Convert.ToString(value, 2);
        //Which returns 1000.
        string binary = Convert.ToString(cube.Score, 2);
        float scaleByScore = minSize + scaleStep * binary.Length;
        cube.transform.localScale = Vector3.one * scaleByScore;
    }

    public void ToPlayerHand(Cube cube)
    {
        float deltaY = cube.transform.localScale.x/2;
        cube.transform.SetParent(playerHand);
        cube.transform.localPosition = Vector3.up * deltaY;
    }
    
    public Cube GetBomb()
    {
        Cube bomb = Bombes.Find(b => !b.gameObject.activeSelf);

        if (bomb is null)
            bomb = CreateBomb();
        
        Bomb bombCollide = new Bomb();
        bomb.CollideEffect = bombCollide;
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
        cubes.Add(bomb);
        bomb.transform.parent = playerHand;
        bomb.gameObject.SetActive(false);
        return bomb;
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
