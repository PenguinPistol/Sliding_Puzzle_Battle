using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    private IEnumerator Start()
    {
        //Debug.Log("Databas load start");
        yield return StartCoroutine(Database.Instance.Load());
        //Debug.Log("Databas load Finish");

        //Debug.Log("data loaded? : " + Database.Instance.StageLoaded);
        SceneManager.LoadScene("Game");
    }
}
