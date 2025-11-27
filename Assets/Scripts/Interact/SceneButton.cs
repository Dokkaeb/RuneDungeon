using UnityEngine;
using UnityEngine.UI;


public class SceneButton : MonoBehaviour
{
    [SerializeField] private int targetSceneIndex=0;

    private Button _sceneButton;
    

    private void Awake()
    {
        _sceneButton = GetComponent<Button>();
        
    }
    private void OnEnable()
    {
        SceneLoader sceneLoader = SceneLoader.Instance;

        _sceneButton.onClick.AddListener(() => OnButtonClick(sceneLoader));
        
    }
    private void OnButtonClick(SceneLoader loader)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetGamePause(false);
        }
        else
        {
           
            Debug.LogWarning("GameManager가 없으므로 일시정지 해제를 건너뜁니다.");
        }

        loader.LoadSceneByIndex(targetSceneIndex);
    }
    private void OnDisable()
    {
        // 오브젝트 비활성화 시 리스너를 제거하여 메모리 누수를 방지합니다.
        _sceneButton.onClick.RemoveAllListeners();
    }
}
