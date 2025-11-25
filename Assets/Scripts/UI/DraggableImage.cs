
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggableImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _originParent;
    private CanvasGroup _canvasGroup;
    [SerializeField] GameObject _prefab;
    [SerializeField] private ItemData _itemDate;
    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        
    }
    private void Start()
    {
        if(UIManager.Instance != null)
        {
            UIManager.Instance.RegisterIcon(_itemDate.ItemID, gameObject);
            
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _originParent = transform.parent;
        transform.SetParent(_originParent.root);
        _canvasGroup.alpha = 0.5f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;

        if (!IsDroppedOnUI(eventData))
        {
            SpawnCubeAtMousePosition(eventData.position);
            gameObject.SetActive(false); //만약 UI이미지 없애버릴거면
            
            transform.SetParent(_originParent);
        }
        else
        {
            
            transform.SetParent(_originParent);
            
        }
    }
    //UI에 드롭시켰는지 판단할 메서드
    private bool IsDroppedOnUI(PointerEventData eventData)
    {

        return eventData.pointerCurrentRaycast.gameObject != null;
    }

    private void SpawnCubeAtMousePosition(Vector2 screenPosition)
    {
        Vector3 screenPoint = screenPosition;

        screenPoint.z = Camera.main.nearClipPlane;

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);

        worldPoint.z = 0f;

        GameObject spawnObj = Instantiate(_itemDate.prefab, worldPoint,Quaternion.identity);

        TakeBackToInven item = spawnObj.GetComponent<TakeBackToInven>();
        if(item != null)
        {
            item.Initialize(_itemDate);
        }
    }
}
