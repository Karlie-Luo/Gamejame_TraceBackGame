using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PeopleScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void onBeginButtonClicked()
    {
        SceneManager.LoadScene("BeginScene");
    }
}
