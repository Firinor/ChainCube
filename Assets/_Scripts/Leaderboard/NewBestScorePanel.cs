using FirAnimations;
using TMPro;
using UnityEngine;
using Zenject;

public class NewBestScorePanel : MonoBehaviour
{
    [Inject] 
    private Player player;
    [SerializeField]
    public OneSoundPlayer sound;
    [SerializeField] 
    private TextMeshProUGUI scoreText;

    [SerializeField]
    public TMP_InputField PlayerNameInputField;

    public TextCounterAnimation TextCounter;
    
    [Inject]
    public void Initialize()
    {
        PlayerNameInputField.text = PlayerPrefs.GetString(PrefsKey.PlayerName);
        scoreText.text = player.CurrentScore.Value.ToString();
    }

    private void OnEnable()
    {
        sound.Play();
    }
}