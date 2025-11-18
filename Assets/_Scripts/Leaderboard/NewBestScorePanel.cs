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

    public FirTextNumeratorAnimation numerator;

    [Inject]
    public void Initialize()
    {
        //PlayerNameInputField.text = PlayerPrefs.GetString(PrefsKey.PlayerName);
        scoreText.text = player.CurrentScore.Value.ToString();
    }

    private void OnEnable()
    {
        sound.Play();
        numerator.EndPosition = player.CurrentScore.Value;
    }
}