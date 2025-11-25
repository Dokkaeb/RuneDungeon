using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    private Vector3 _respawnPos;
    private bool _isActivated = false;

    private void Start()
    {
        _respawnPos = transform.position;
    }
    public void Interact(Witch interactor)
    {
        if (!_isActivated)
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.SetRespawnPoint(_respawnPos);
            }
            _isActivated = true;
        }
    }
}
