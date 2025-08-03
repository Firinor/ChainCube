using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [field: SerializeField]
    public int Score { get; set; } = 0;
    [field: SerializeField]
    public ECubeForm form { get; private set; }

    public bool IsInGame = false;
    public CubeCollideEffect CollideEffect = new NormalCube();
    public static Action<int> OnMerge;

    [Inject]
    protected CubeFactoryWithPool cubeFactory;
    [Inject]
    private GameSettings settings;

    public Collider Collider;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;

    
    [Inject]
    private void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CollideEffect.OnTriggerEnter(this, other);
    }

    public void RefreshMaterial()
    {
        
        cubeFactory.SetCubeParams(this, new(){Score = (ECube)Score, Form = form});
    }
    public void GetReadyToLaunch()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        gameObject.SetActive(true);
        CollideEffect = new NormalCube();

        CubeSliding cubeSliding = GetComponent<CubeSliding>();
        if (cubeSliding is null)
            cubeSliding = gameObject.AddComponent<CubeSliding>();
        else
            cubeSliding.SlidingOn();
    }
    public void Launch()
    {
        rb.isKinematic = false;
        rb.AddForce(Vector3.forward * settings.PunchForce);
    }
    public void SetScore(ECube cube)
    {
        Score = (int)cube;
    }
    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }
    public void AddUpForce()
    {
        rb.inertiaTensor = Vector3.zero;
        rb.automaticInertiaTensor = true;

        Vector3 upWithSpread = new Vector3(
            Random.value * settings.SpreadForce, 
            settings.UpForce, 
            Random.value * settings.SpreadForce);

        rb.AddForce(upWithSpread, ForceMode.Impulse);
    }

    public void ToPool()
    {
        IsInGame = false;
        gameObject.SetActive(false);
    }
}