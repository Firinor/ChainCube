using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class PlayerInput : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [Inject]
    private Player player;
    [Inject(Id = "PlayerCubeAnchor")]
    private Transform playerCubeAnchor;
    [Inject]
    private GameSettings settings;

    [SerializeField] 
    private Canvas canvas;
    
    public void OnDrag(PointerEventData eventData)
    {
        MovePlayer(eventData);
    }

    private void MovePlayer(PointerEventData eventData)
    {
        Vector3 newPosition = playerCubeAnchor.position;

        // Mouse.x = 1080 . Position.x => 1.55(board)
        // Mouse.x = 0 . Position.x => -1.55(board)
        // Mouse.x = 540 . Position.x => 0
        // Mouse.x = 800 . Position.x => ~ 1
        // Mouse.x = 200 . Position.x => ~ -1
        
        float aspect = Camera.main!.aspect;
        float halfOfScreen = Screen.width / 2; //600
        float playField = aspect*2;//Approximate ratio of the playing field
        float rate = (eventData.position.x - halfOfScreen) / Screen.width * playField; // -1/2 <=> 1/2

        newPosition.x = rate * settings.Sensitivity; //sensitivity ~ 4

        newPosition.x = Mathf.Clamp(newPosition.x, -settings.CubeBoard, settings.CubeBoard);

        playerCubeAnchor.position = newPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MovePlayer(eventData);
        player.TryShoot();
    }
    public void SwitchCubeTo(int cube)
    {
        player.SwitchCubeTo(cube);
    }
}
