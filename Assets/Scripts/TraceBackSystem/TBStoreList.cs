using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
public class TBStoreList : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();

    public void BackToNormal()
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.GetComponent<Highlighter>().ConstantOff();
        }
    }

    public void CanBeChosen()
    {
        foreach (GameObject obj in gameObjects)
        {
            Debug.Log("CanBeChosen");
            obj.GetComponent<Highlighter>().ConstantOn(Color.red);
        }
    }

    public void IsChosen()
    {

    }
}
