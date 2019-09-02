using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{
    public void changeMenuScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
