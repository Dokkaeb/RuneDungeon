using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance{  get; private set; }

    private Dictionary<string,GameObject> _iconMap = new Dictionary<string,GameObject>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void RegisterIcon(string itemID,GameObject iconObject)
    {
        if (!_iconMap.ContainsKey(itemID))
        {
            _iconMap.Add(itemID, iconObject);
        }
        else
        {
            Debug.LogWarning($"{itemID}는 이미 등록함");
        }
    }
    public void RestoreIcon(string itemID)
    {
        if(_iconMap.TryGetValue(itemID, out GameObject icon))
        {
            icon.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"{itemID}없음");
        }
    }
}
