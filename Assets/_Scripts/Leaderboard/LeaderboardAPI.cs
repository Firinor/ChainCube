using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Firestore
{
    public class LeaderboardAPI
    {
        private const string PUBLIC_API_KEY = "AIzaSyBBr2CGsXefO9b6xc9rjKz_aqC_x9Z6Bdc";
        private const string PROJECT_NAME = "cubid-2048";
        private const string COLLECTION_NAME = "Leaderboard";
        private const string URL =
            "https://firestore.googleapis.com/v1/projects/"+PROJECT_NAME+"/databases/(default)/documents/"+COLLECTION_NAME;
        private string idToken;

        public IEnumerator Start()
        {
            yield return SignInAnonymously();

            if (!string.IsNullOrEmpty(idToken))
            {
                yield return GetFirestoreData();
            }
        }

        private IEnumerator SignInAnonymously()
        {
            string url = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={PUBLIC_API_KEY}";

            var data = new
            {
                returnSecureToken = true
            };

            string jsonData = JsonUtility.ToJson(data);

            using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, jsonData))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var response = JsonConvert.DeserializeObject<AuthResult>(request.downloadHandler.text);
                    idToken = response.idToken;
                }
                else
                {
                    Debug.LogError("SignInError: " + request.error);
                }
            }
        }

        private IEnumerator GetFirestoreData()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(URL))
            {
                request.SetRequestHeader("Authorization", $"Bearer {idToken}");
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var response = JsonConvert.DeserializeObject<LeaderboardData>(request.downloadHandler.text);

                    foreach (var doc in response.documents)
                    {
                        Debug.Log(doc.Name + ": " + doc.Score);
                    }
                }
                else
                {
                    Debug.LogError("GetDataError: " + request.error);
                }
            }
        }
        
        public async Task CreateDocumentAsync(string playerName, int score, MonoBehaviour mono)
        {
            await CoroutineAsTask(SignInAnonymously());
            
            Task CoroutineAsTask(IEnumerator coroutine)
            {
                var completionSource = new TaskCompletionSource<bool>();
                mono.StartCoroutine(RunCoroutine(coroutine, completionSource));
                return completionSource.Task;
            }
            IEnumerator RunCoroutine(IEnumerator coroutine, TaskCompletionSource<bool> completionSource)
            {
                yield return mono.StartCoroutine(coroutine);
                completionSource.SetResult(true);
            }
            
            var document = new 
            {
                fields = new
                {
                    name = new { stringValue = playerName },
                    score = new { integerValue = score.ToString() }
                }
            };
            
            string jsonBody = JsonConvert.SerializeObject(document);
            
            string url = URL;//+"?documentId={documentId}"
            
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", idToken);

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Debug.Log($"Error: {response.StatusCode}");
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.Log(errorContent);
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Exception: {ex.Message}");
            }
        }
    }
}