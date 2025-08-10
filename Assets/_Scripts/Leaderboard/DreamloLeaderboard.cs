using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class DreamloLeaderboard
{
    private const string PROJECT_NAME = "cubid-2048-default-rtdb";
    private const string WEB_API_KEY = "hKRTkv0p6I62lFwwqPAGP1RVI3Eu5FoSbUHbThG5";

    public IEnumerator AddScore(string username, int score)
    {
        string url = $"https://{PROJECT_NAME}.europe-west1.firebasedatabase.app/Leaderboard.json?auth={WEB_API_KEY}";
        string json = $"{{\"name\":\"{username}\", \"score\":{score}}}";
        byte[] data = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest www = new UnityWebRequest(url, "POST");
        www.uploadHandler = new UploadHandlerRaw(data);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
    }
    public IEnumerator DownloadTopScores()
    {
        string url = $"https://{PROJECT_NAME}.europe-west1.firebasedatabase.app/Leaderboard.json?auth={WEB_API_KEY}&orderBy=\"score\"&limitToLast=25";
        
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка: " + www.error);
            yield break;
        }
        // Ответ в формате JSON (пример: {"-N123": {"name": "Player1", "score": 100}, ...})
        string jsonResponse = www.downloadHandler.text;
        
        Dictionary<string, LeaderboardEntry> leaderboard =  JsonConvert.DeserializeObject<Dictionary<string, LeaderboardEntry>>(jsonResponse);
        
        List<LeaderboardEntry> sortedEntries = new List<LeaderboardEntry>(leaderboard.Values);
        sortedEntries.Sort((a, b) => b.score.CompareTo(a.score));

        foreach (var entry in sortedEntries)
        {
            Debug.Log(entry.name + ": "+entry.score);
        }
    }
}

[Serializable]
public struct LeaderboardEntry
{
    public string name;
    public int score;
}