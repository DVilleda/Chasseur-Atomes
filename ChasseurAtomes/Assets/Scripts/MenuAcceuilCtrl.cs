using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAcceuilCtrl : MonoBehaviour
{
    public string Niveau1;

    public void StartGame()
    {
        StartCoroutine(LoadStart());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public IEnumerator LoadStart()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Niveau1);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
