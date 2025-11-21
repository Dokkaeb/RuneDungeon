using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    public ItemData _data;
    

    public void Initialize(ItemData data)
    {
        _data = data;
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
