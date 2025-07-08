using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void NextScene(){
        if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings) return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void PreviousScene(){
        if(SceneManager.GetActiveScene().buildIndex == 0) return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
    }
    public void ChangeSceneByName(string scene)
    {
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}
    public void QuitApp() {
		Application.Quit();
	}
}
