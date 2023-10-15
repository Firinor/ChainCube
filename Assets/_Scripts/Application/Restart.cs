using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void OnClickRestart()
    {
        SceneManager.LoadScene(0);
    }
}
