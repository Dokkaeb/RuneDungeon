using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody2D))]
public class DragLauncher : MonoBehaviour
{
    [SerializeField] private float _launchForce = 10f;  //당긴 거리에 곱해질 힘
    [SerializeField] private float _maxDragDistance = 2f; //최대로 당길수 있는 거리
    [SerializeField] private LineRenderer _aimingLine;  //조준선
    [SerializeField] private Transform _pivotPoint;  //당겨지는 중심점

    private Rigidbody2D _rb;
    private bool _isDragging = false;
    private Vector2 _dragStartPos; //드래그 시작 마우스 위치
    private Vector2 _currentDragDir; // 당겨지는 방향
    private Vector2 _objPos;  //발사체 초기 위치

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.bodyType = RigidbodyType2D.Kinematic;
    }
    private void Start()
    {
        if(_pivotPoint == null)
        {
            _pivotPoint = transform;
        }
        _objPos = _pivotPoint.position;

        if(_aimingLine != null)
        {
            _aimingLine.positionCount = 2;
            _aimingLine.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("클릭 감지 성공!");
        _isDragging = true;
        _dragStartPos = GetMouseWorldPosition();

        transform.position = _objPos;
        _rb.linearVelocity = Vector2.zero;

        if(_aimingLine != null)
        {
            _aimingLine.enabled = true;
        }
    }
    private void OnMouseDrag()
    {

        if (!_isDragging) return;

        Vector2 currentMousePos = GetMouseWorldPosition();

        Vector2 dragVector = currentMousePos - _objPos;

        if(dragVector.magnitude > _maxDragDistance )
        {
            dragVector = dragVector.normalized * _maxDragDistance;
        }

        transform.position = _objPos - dragVector;

        _currentDragDir = -dragVector;

        if(_aimingLine != null)
        {
            _aimingLine.SetPosition(0,transform.position);
            _aimingLine.SetPosition(1, _objPos);
        }
    }
    private void OnMouseUp()
    {
        if (!_isDragging) return;
        _isDragging = false;

        if(_aimingLine != null)
        {
            _aimingLine.enabled = false;
        }

        float forceMagnitude = _currentDragDir.magnitude * _launchForce;
        Vector2 launchForce = _currentDragDir.normalized * forceMagnitude;

        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.AddForce(launchForce,ForceMode2D.Impulse);

        //발사후처리
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();

        // Z 축을 카메라의 near clip plane으로 설정하여 가장 가까운 월드 평면을 사용합니다.
        mouseScreenPos.z = Camera.main.nearClipPlane;

        // World 좌표로 변환
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        return new Vector2(worldPoint.x, worldPoint.y);
    }
    
}
