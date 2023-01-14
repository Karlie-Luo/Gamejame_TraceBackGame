using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BeginScene : MonoBehaviour
{
    public GameObject sceneFadeInOut;
    public void onBeginButtonClicked()
    {
        Debug.Log("begin button clicked");
        sceneFadeInOut.GetComponentInChildren<SceneFadeInOut>().NextScene();
    }
    public void onPeopleButtonClicked()
    {
        SceneManager.LoadScene("PeopleScene");
    }
    public void onQuitButtonClicked()
    {
        Application.Quit();
    }
}
