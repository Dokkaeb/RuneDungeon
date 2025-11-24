using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TakeBackToInven : MonoBehaviour, IInteractable
{
    public ItemData _data;
    

    public void Initialize(ItemData data)
    {
        _data = data;
    }

    public void Interact(Witch interactor)
    {
        TakeBack();
    }

    public void TakeBack()
    {
        if(UIManager.Instance != null)
        {
            UIManager.Instance.RestoreIcon(_data.ItemID);
        }
        
        Destroy(gameObject);
    }

}
