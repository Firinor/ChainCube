using System.Collections;
using Firestore;
using UnityEngine;
using Zenject;

public class SendRecordScript : MonoBehaviour
{
    [Inject] 
    private Player player;
    [SerializeField] 
    private Restart restart;

    public void SendRecord()
    {
        StartCoroutine(SendRecordCoroutine());
    }

    public IEnumerator SendRecordCoroutine()
    {
        string playerName = PlayerPrefs.GetString(PrefsKey.PlayerName);
        yield return new LeaderboardAPI().CreateDocumentAsync(playerName, player.CurrentScore.Value, this);
        restart.OnClickRestart();
    }
}
