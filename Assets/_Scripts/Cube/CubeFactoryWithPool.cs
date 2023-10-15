using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class CubeFactoryWithPool: MonoBehaviour, IFactory<object, Cube>
{
    [SerializeField]
    private List<Cube> cubes;
    [SerializeField]
    private Dictionary<int, Material> materials = new();
    [SerializeField]
    private Cube prefab;
    [Inject]
    private DiContainer container;

    public Cube Create(object param)
    {
        Cube newCube = cubes.Find(b => !b.IsInGame);

        if (newCube == null)
        {
            newCube = container.InstantiatePrefabForComponent<Cube>(prefab, transform);
            cubes.Add(newCube);
        }

        if(param is CubeWithPosition cubeParam)
        {
            newCube.transform.position = cubeParam.Position;
            newCube.SetScore(cubeParam.Cube);
            StartCoroutine(SetMaterial(newCube, (int)cubeParam.Cube));
        }
        else if (param is Vector3 cubePosition)
        {
            newCube.transform.position = cubePosition;
        }

        newCube.IsInGame = true;

        return newCube;
    }

    public IEnumerator<AsyncOperationHandle<Material>> SetMaterial(Cube cube, int cubeScore)
    {
        if (!materials.ContainsKey(cubeScore))
        {
            AsyncOperationHandle<Material> loadHandle = Addressables.LoadAssetAsync<Material>(cubeScore.ToString());

            yield return loadHandle;

            if (!materials.ContainsKey(cubeScore))
            {
                materials.Add(cubeScore, loadHandle.Result);
            }
        }
        cube.SetMaterial(materials[cubeScore]);
    }

    public void ClearAll()
    {
        foreach(Cube cube in cubes)
        {
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
