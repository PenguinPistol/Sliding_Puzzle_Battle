using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ChangeScene", 3.0f);
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("Game");
    }
}
