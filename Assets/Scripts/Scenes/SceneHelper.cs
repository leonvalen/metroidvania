using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{
    public bool loadNewScene;

    // Update is called once per frame
    void Update()
    {
        if (loadNewScene)
        {
            LoadScene();
            loadNewScene = false;
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Level2");
    }
}
