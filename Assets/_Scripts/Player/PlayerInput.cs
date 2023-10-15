using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class PlayerInput : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [Inject]
    private Player player;
    [SerializeField]
    private GameObject playerCube;
    [SerializeField]
    private float board;
    [SerializeField]
    private float sensitivity;

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPosition = playerCube.transform.position;

        // Mouse.x = 1080 . Position.x => 1.55(board)
        // Mouse.x = 0 . Position.x => -1.55(board)
        // Mouse.x = 540 . Position.x => 0
        // Mouse.x = 800 . Position.x => ~1
        // Mouse.x = 200 . Position.x => ~ -1


        float halfOfScreen = Screen.width / 2;//540
        float rate = (Input.mousePosition.x - halfOfScreen) / Screen.width;// -1/2 <=> 1/2

        newPosition.x = rate * sensitivity;//sensitivity ~ 4

        newPosition.x = Mathf.Clamp(newPosition.x, -board, board);

        playerCube.transform.position = newPosition;
        player.SetCubePosition(newPosition);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        player.Shoot();
    }
}
