using UnityEngine;

public class GameClear : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _clear;

    public void Interact(Witch interactor)
    {
        _clear.SetActive(true);
        GameManager.Instance.SetGamePause(true);
    }
}
