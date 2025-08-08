using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class PlayerSpesialPresenter : MonoBehaviour
{
    [Inject] 
    private Player player;
    
    [SerializeField]
    private TextMeshProUGUI rainbowCountText;
    [SerializeField]
    private TextMeshProUGUI bombCountText;
    [SerializeField]
    private TextMeshProUGUI ghostCountText;

    private void Awake()
    {
        player.BombCount.Subscribe(ChangeBombCount);
        player.GhostCount.Subscribe(ChangeGhostCount);
        player.RainbowCount.Subscribe(ChangeRainbowCount);
        Destroy(this);
    }

    private void ChangeBombCount(int newCount)
    {
        bombCountText.text = newCount.ToString();
    }
    private void ChangeGhostCount(int newCount)
    {
        ghostCountText.text = newCount.ToString();
    }
    private void ChangeRainbowCount(int newCount)
    {
        rainbowCountText.text = newCount.ToString();
    }
}
