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
            Debug.Log($"[UIManager] 아이콘 등록: ID={itemID}, Object={iconObject.name}");
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
            Debug.Log($"[UIManager] 아이콘 복원 성공: ID={itemID}, Object={icon.name}");
        }
        else
        {
            Debug.LogWarning($"{itemID}없음");
            Debug.LogWarning($"[UIManager] 아이콘 복원 실패: {itemID}없음. 등록된 키 목록: {string.Join(", ", _iconMap.Keys)}");
        }
    }
}
