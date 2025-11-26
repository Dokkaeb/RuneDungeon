using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //ÀÎµ¦½º¹øÈ£·Î ¾À Á¢±Ù
    public void LoadSceneByIndex(int scenIndex)
    {
        if(scenIndex <0 ||scenIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("¾À ÀÎµ¦½º ¹üÀ§ ¹þ¾î³²");
            return;
        }
        SceneManager.LoadScene(scenIndex);
    }
    //Àç½ÃÀÛ
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
