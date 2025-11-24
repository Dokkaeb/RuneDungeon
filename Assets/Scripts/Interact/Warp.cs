using UnityEngine;

public class Warp : MonoBehaviour, IInteractable
{
    [SerializeField] Transform _warpPoint;
    bool _isWarped = false;

    public void Interact(Witch interactor)
    {
        if (_isWarped)
        {
            interactor.transform.position = _warpPoint.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            _isWarped = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            _isWarped = false;
        }
    }
}
