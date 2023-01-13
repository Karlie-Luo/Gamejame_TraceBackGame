using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BeginScene : MonoBehaviour
{
    public void onBeginButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void onQuitButtonClicked()
    {
        Application.Quit();
    }
}
