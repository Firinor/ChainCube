using Zenject;

public class Installer : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Player>().AsSingle();

        //StateMachine
        Container.Bind<GameplayStateMachine>().AsSingle();
        Container.Bind<InGameRules>().AsSingle();
        Container.Bind<InPauseRules>().AsSingle();
        
        Container.Bind<PlayerCubeMachine>().AsSingle();
    }
    
}