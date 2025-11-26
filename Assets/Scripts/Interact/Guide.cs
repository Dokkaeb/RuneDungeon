
using UnityEngine;


public class Guide : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _pr;
    public void Interact(Witch interactor)
    {
        _pr.SetActive(true);
    }
}
