using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void changeMenuScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
