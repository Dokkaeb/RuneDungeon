using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance{  get; private set; }

    [SerializeField] private HpBarView _hpUI;
    [SerializeField] private TextMeshProUGUI _interactGuide;
    

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
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
 
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_interactGuide != null)
        {
            // 캔버스 자체는 넘어왔으므로, _interactGuide는 유효해야 함.
            _interactGuide.gameObject.SetActive(false);
        }
        else
        {
            
            Debug.LogWarning("[UIManager] 핵심 UI 참조 (_interactGuide)가 null입니다. Inspector 연결을 확인하세요.");
            
        }
        //새 씬에서 플레이어 오브젝트를 찾아서 InitUI를 다시 호출
        Witch player = FindAnyObjectByType<Witch>();
        if (player != null)
        {
            InitUI(player); // HP 바를 새 플레이어에 연결
        }
    }
    public void InitUI(Witch player)
    {
        if(_hpUI != null)
        {
            _hpUI.Init(player);
        }
    }
    public void RegisterIcon(string itemID,GameObject iconObject)
    {
        if (!_iconMap.ContainsKey(itemID))
        {
            GameObject rootObject = iconObject.transform.root.gameObject;
            if (rootObject.GetComponent<UIManager>() == null) // UIManager가 붙어있는 오브젝트와 동일하지 않다면
            {
                //최상위 부모 오브젝트에 DontDestroyOnLoad를 적용하여
                // 아이콘 오브젝트와 그 부모 (Canvas 등)가 씬 전환 시 파괴되지 않도록 합니다.
                DontDestroyOnLoad(rootObject);
            }
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
            Debug.LogWarning($"[UIManager] 아이콘 복원 실패: {itemID}없음. 등록된 키 목록: {string.Join(", ", _iconMap.Keys)}");
        }
    }

    public void ShowInteractGuide()
    {
        if( _interactGuide != null)
        {
            _interactGuide.gameObject.SetActive(true);
        }
    }
    public void HideInteractGuide()
    {
        if(_interactGuide != null)
        {
            _interactGuide.gameObject.SetActive(false);
        }
    }
}
