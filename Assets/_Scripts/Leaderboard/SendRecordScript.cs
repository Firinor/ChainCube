using Firestore;
using UnityEngine;
using Zenject;

public class SendRecordScript : MonoBehaviour
{
    [Inject] 
    private Player player;
    [SerializeField] 
    private Restart restart;
    
    public async void SendRecord()
    {
        string playerName = PlayerPrefs.GetString(PrefsKey.PlayerName);
        await new LeaderboardAPI().CreateDocumentAsync(playerName, player.CurrentScore.Value, this);
        restart.OnClickRestart();
    }
}
