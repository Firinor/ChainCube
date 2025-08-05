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
    public ECubeForm form { get; set; }
    [SerializeField]
    private bool isInGame;
    public bool IsInGame => isInGame;

    public CubeCollideEffect CollideEffect = new NormalCube();
    public Action<Cube> Remove;
    public void RemoveCube(){Remove?.Invoke(this);}
    public Action<Cube> RefreshView;
    public void CheckView(){RefreshView?.Invoke(this);}
    
    [Inject]
    private GameSettings settings;
    
    public Collider Collider;
    public Collider Trigger;
    private Rigidbody rb;
    public Rigidbody Rigidbody => rb;
    private MeshRenderer meshRenderer;
    
    [Inject]
    private void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsInGame) return;

        if (other.CompareTag("Cube")
            || other.CompareTag("EndWall"))
        {
            SlidingOff();
            CollideEffect.OnTriggerEnter(this, other);
        }
    }
    public void GetReadyToLaunch()
    {
        isInGame = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        gameObject.SetActive(true);
        rb.isKinematic = true;
        SlidingOn();
    }
    public void SlidingOn()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionX 
                         | RigidbodyConstraints.FreezePositionY 
                         | RigidbodyConstraints.FreezeRotation;
    }
    private void SlidingOff()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
    }
    public void Launch()
    {
        rb.isKinematic = false;
        rb.AddForce(Vector3.forward * settings.PunchForce);
        isInGame = true;
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
}