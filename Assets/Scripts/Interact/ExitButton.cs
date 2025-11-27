using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            // 기존 리스너 제거 및 GameManager.QuitGame() 함수 연결
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(GameManager.Instance.ExitGame);

            Debug.Log("[GameQuitButtonBinder] 게임 종료 함수 연결 완료.");
        }
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
}
