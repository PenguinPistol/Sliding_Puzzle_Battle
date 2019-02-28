using UnityEngine;
using System.Collections;

public class TestObject : MonoBehaviour
{

    IEnumerator Start()
    {
        yield return StartCoroutine(Test());
        Debug.Log("Test1 Finished");
        yield return StartCoroutine(Test2());
        Debug.Log("Test2 Finished");
    }

    private IEnumerator Test()
    {
        Debug.Log("Test1 Start");

        for (int i = 0; i < 10; i++)
        {
            Debug.Log("" + i);

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator Test2()
    {
        Debug.Log("Test2 Start");

        for (int i = 10; i < 20; i++)
        {
            Debug.Log("" + i);

            yield return new WaitForSeconds(1f);
        }
    }

}
