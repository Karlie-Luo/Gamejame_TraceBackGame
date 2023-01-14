using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//µ­Èëµ­³ö
public class SceneFadeInOut : MonoBehaviour
{
    public float fadeSpeed = 1.5f;
    public bool sceneStarting = true;
    public static bool sceneEnding = false;
    private RawImage rawImage;
    public static bool sceneReloading = false;
    
    void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    void Start()
    {
    }

    void Update()
    {
        if (sceneStarting)
        {
            StartScene();
        }

        if (sceneEnding)
        {
            Debug.Log("end scene");
            EndScene();
        }
    }

    private void FadeToClear()
    {
        rawImage.color = Color.Lerp(rawImage.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    private void FadeToBlack()
    {
        rawImage.color = Color.Lerp(rawImage.color, Color.black, fadeSpeed * Time.deltaTime);
    }

    void StartScene()
    {
        FadeToClear();
        if (rawImage.color.a < 0.05f)
        {
            rawImage.color = Color.clear;
            rawImage.enabled = false;
            sceneStarting = false;
        }
    }

    void EndScene()
    {
        rawImage.enabled = true;
        FadeToBlack();
        if (rawImage.color.a > 0.95f)
        {
            sceneEnding = false;
            if(sceneReloading)
            {
                Debug.Log("reload this scene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                sceneReloading = false;
            }
            else
            {
                Debug.Log("load next scene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }          
        }
    }

    public void ReloadEffect()
    {
        sceneEnding = true;
        sceneReloading = true;
    }

    public void NextScene()
    {
        Debug.Log("next scene");
        sceneEnding = true;
        sceneReloading = false;
    }
    
}

