using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ScriptableInstaller", menuName = "Installers/ScriptableInstaller")]
public class ScriptableInstaller : ScriptableObjectInstaller<ScriptableInstaller>
{
    [SerializeField]
    private CubeChanceWeight weights;

    public override void InstallBindings()
    {
        Container.BindInstance(weights).AsSingle();
    }
}