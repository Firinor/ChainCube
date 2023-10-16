using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ScriptableInstaller", menuName = "Installers/ScriptableInstaller")]
public class ScriptableInstaller : ScriptableObjectInstaller<ScriptableInstaller>
{
    [SerializeField]
    private CubeChanceWeight weights;
    [SerializeField]
    private GameSettings settings;

    public override void InstallBindings()
    {
        Container.BindInstance(weights).AsSingle();
        Container.BindInstance(settings).AsSingle();
    }
}