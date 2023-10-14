using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CubeFactoryWithPool: MonoBehaviour, IFactory<object, Cube>
{
    [SerializeField]
    private List<Cube> cubes;
    [SerializeField]
    private Cube prefab;
    [Inject]
    private DiContainer container;

    public Cube Create(object param)
    {
        Cube newCube = cubes.Find(b => !b.gameObject.activeSelf);

        if (newCube == null)
        {
            newCube = container.InstantiatePrefabForComponent<Cube>(prefab, transform);
            cubes.Add(newCube);
        }

        if(param is Vector3 position)
        {
            newCube.transform.position = position;
        }

        return newCube;
    }

    public void ClearAll()
    {
        foreach(Cube cube in cubes)
        {
            Destroy(cube.gameObject);
        }
        cubes.Clear();
        GC.Collect();
    }

    private void OnDestroy()
    {
        ClearAll();
    }
}
