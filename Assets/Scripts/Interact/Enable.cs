using UnityEngine;

public class Enable : MonoBehaviour, IInteractable
{
    public void Interact(Witch interactor)
    {
        gameObject.SetActive(false);
    }
}
