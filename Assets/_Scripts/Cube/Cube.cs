using UnityEngine;
using Zenject;

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    public bool IsInGame = false;

    [field: SerializeField]
    public int score { get; private set; } = 0;

    [Inject]
    private CubeFactoryWithPool cubeFactory;

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
        if (!IsInGame)
            return;

        if(other.tag == "Cube")
        {
            Cube otherCube = other.GetComponent<Cube>();
            if(otherCube.score == score)
            {
                otherCube.IsInGame = false;
                otherCube.gameObject.SetActive(false);
                score *= 2;
                cubeFactory.SetMaterial(this, score);
                AddForce();
            }
        }
    }

    public void SetScore(ECube cube)
    {
        score = (int)cube;
    }

    private void AddForce()
    {
        rb.AddForce(Vector3.up);
    }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }
}