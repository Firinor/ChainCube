using UnityEngine;
using Zenject;

public class Installer : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Player>().AsSingle();
        Container.Bind<GameplayStateMachine>().AsSingle();
    }
    
}