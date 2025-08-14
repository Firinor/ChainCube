using TMPro;
using UnityEngine;
using Zenject;

public class NewBestScorePanel : MonoBehaviour
{
    [Inject] 
    private Player player;

    [SerializeField] 
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TMP_InputField playerNameInputField;

    [Inject]
    public void Initialize()
    {
        playerNameInputField.text = PlayerPrefs.GetString(PrefsKey.PlayerName);
        scoreText.text = player.CurrentScore.Value.ToString();
    }
}
