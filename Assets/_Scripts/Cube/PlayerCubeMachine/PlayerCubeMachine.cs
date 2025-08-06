using UnityEngine;
using Zenject;

public class PlayerCubeMachine
{
    private PlayerCubeStatus currentStatus;
    [Inject]
    private CubeFactoryWithPool cubeFactory;
    [Inject]
    private CubeChanceWeight cubeChance;

    private Cube currentCube;

    private Cube cachedCube;
    private Cubid cachedCubid;

    public void SwitchToStatus(PlayerCubeStatus newStatus)
    {
        if (currentStatus is not null)
            currentStatus.OnExit();

        if (currentStatus is not null && currentStatus.ECube != newStatus.ECube)
        {
            cachedCube = currentCube;
            cachedCubid = new Cubid(cachedCube);
            
            newStatus.cubeFactory = cubeFactory;
            newStatus.currentCube = currentCube;
            currentStatus = newStatus;
            currentStatus.OnEnter();
        }
        else // return to normal
        {
            if (cachedCube is not null)
            {
                currentCube = cachedCube;
                cachedCube = null;
            }

            if (cachedCubid.Score > 0)
            {
                currentCube.Score = (int)cachedCubid.Score;
                currentCube.form = cachedCubid.Form;
                cachedCubid = default;
            }

            currentStatus = new NormalCubeStatus();
            currentStatus.cubeFactory = cubeFactory;
            currentStatus.currentCube = currentCube;
            currentStatus.OnEnter();
            currentCube.GetReadyToLaunch();
        }
    }

    public void TryShoot()
    {
        currentStatus.Launch();
        currentStatus = null;
        
        currentCube = null;
        cachedCube = null;
        cachedCubid = default;
    }

    public void NewCube(Cube cube = null)
    {
        if (cube is null)
        {
            cachedCubid = GetRandomCubid();
            currentCube = cubeFactory.Create(cachedCubid);
        }
        else
        {
            currentCube = cube;
        }
        
        SwitchToStatus(new NormalCubeStatus());
        currentCube.GetReadyToLaunch();
    }
    
    private Cubid GetRandomCubid()
    {
        int random = Random.Range(1, cubeChance.WeightOfAllElements+1);
        int index = 0, weigth = 0;

        while (weigth < random)
        {
            weigth += cubeChance.cubeWeightList[index].Weight;
            index++;
        }

        int randomForm = Random.Range(0, maxExclusive: 2);

        Cubid result = new()
        {
            Score = cubeChance.cubeWeightList[index-1].Cube,
            Form = (ECubeForm)randomForm
        };
        
        return result;
    }
}