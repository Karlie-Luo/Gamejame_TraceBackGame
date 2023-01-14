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
    private float changeTime = 0f;
    
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
            Debug.Log($"end scene:{rawImage.color.a}");
            EndScene();
        }
    }

    private void FadeToClear()
    {
       rawImage.color = Color.Lerp(rawImage.color, Color.clear, 0.06f);
    }

    private void FadeToBlack()
    {
       rawImage.color = Color.Lerp(rawImage.color, Color.black, 0.06f);
    }

    void StartScene()
    {
        FadeToClear();
        if (rawImage.color.a < 0.1f)
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
        if (rawImage.color.a > 0.9f)
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

