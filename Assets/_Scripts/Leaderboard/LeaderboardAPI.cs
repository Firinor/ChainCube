using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Newtonsoft.Json;

namespace Firestore
{
    public class LeaderboardAPI
    {
        private const string PUBLIC_API_KEY = "AIzaSyBBr2CGsXefO9b6xc9rjKz_aqC_x9Z6Bdc";
        private const string PROJECT_NAME = "cubid-2048";
        private const string COLLECTION_NAME = "Leaderboard";

        private const string URL =
            "https://firestore.googleapis.com/v1/projects/" + PROJECT_NAME + "/databases/(default)/documents/" +
            COLLECTION_NAME;

        private const string AUTH_URL =
            "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + PUBLIC_API_KEY;

        private string idToken;
        private string refreshToken;

        public IEnumerator GetLeaderboardData(Action<LeaderboardData> onSuccess = null)
        {
            yield return SignInAnonymously();

            if (!string.IsNullOrEmpty(idToken))
            {
                yield return GetFirestoreData(onSuccess);
            }
        }

        private IEnumerator SignInAnonymously()
        {
            if (String.IsNullOrEmpty(idToken))
            {
                var data = new
                {
                    returnSecureToken = true
                };

                string jsonData = JsonUtility.ToJson(data);

                using UnityWebRequest request = UnityWebRequest.PostWwwForm(AUTH_URL, jsonData);
                request.SetRequestHeader("Content-Type", "application/json");
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var response = JsonConvert.DeserializeObject<AuthResult>(request.downloadHandler.text);
                    idToken = response.idToken;
                    refreshToken = response.refreshToken;
                }
                else
                {
                    Debug.LogError("SignInError: " + request.error);
                }
            }
            else
            {
                yield return RefreshToken();
            }
        }

        private IEnumerator RefreshToken()
        {
            var data = new
            {
                grant_type = "refresh_token",
                refresh_token = refreshToken
            };

            string jsonData = JsonUtility.ToJson(data);

            using UnityWebRequest request = UnityWebRequest.PostWwwForm(AUTH_URL, jsonData);
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonConvert.DeserializeObject<RefreshAuthResult>(request.downloadHandler.text);
                idToken = response.id_token;
                refreshToken = response.refresh_token;
            }
            else
            {
                Debug.LogError("RefreshToken error: " + request.error);
            }
        }

        private IEnumerator GetFirestoreData(Action<LeaderboardData> onSuccess)
        {
            using UnityWebRequest request = UnityWebRequest.Get(URL);
            request.SetRequestHeader("Authorization", $"Bearer {idToken}");
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonConvert.DeserializeObject<LeaderboardData>(request.downloadHandler.text);

                onSuccess?.Invoke(response);
            }
            else
            {
                Debug.LogError("GetDataError: " + request.error);
            }
        }

        public IEnumerator CreateDocumentAsync(string playerName, int score, MonoBehaviour mono)
        {
            yield return SignInAnonymously();

            var document = new
            {
                fields = new
                {
                    name = new { stringValue = playerName },
                    score = new { integerValue = score.ToString() }
                }
            };

            string jsonBody = JsonConvert.SerializeObject(document);

            string url = URL; //+"?documentId={documentId}"

            using UnityWebRequest request = UnityWebRequest.PostWwwForm(url, jsonBody);
            request.SetRequestHeader("Authorization", $"Bearer {idToken}");
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"Error: {request.result}");
                Debug.Log("CreateDocumentAsyncError: " + request.error);
            }
        }
    }
}