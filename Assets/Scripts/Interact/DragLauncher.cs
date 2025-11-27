using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

[RequireComponent (typeof(Rigidbody2D))]
public class DragLauncher : MonoBehaviour
{
    [SerializeField] private float _launchForce = 10f;  //발사 힘
    [SerializeField] private float _maxDragDistance = 2f; //최대 당김 거리
    [SerializeField] private LineRenderer _aimingLine;  //조준선
    [SerializeField] private Transform _pivotPoint;  //발사체 중심점

    [Header("카메라 설정")]
    [SerializeField] private CinemachineCamera _defaultCam; // 캐릭터 카메라
    [SerializeField] private CinemachineCamera _projectileCam; // 발사체 따라가는 카메라
    [SerializeField] private int _activePriority = 20; // 우선순위값
    [SerializeField] private float _followTime = 2f; //따라다니는 시간
    private int _originalPriority; // 기존 우선순위

    [Header("목표 맞추면 나오는 이미지")]
    [SerializeField] private GameObject _successImg;
    private bool _isSuccess = false;

    private Rigidbody2D _rb;
    private bool _isDragging = false;
    private Vector2 _dragStartPos; // 드래그 시작시 마우스 위치
    private Vector2 _currentDragDir; // 마우스 당긴 방향
    private Vector2 _objPos;  // 발사체 초기위치

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.bodyType = RigidbodyType2D.Kinematic;
    }
    private void Start()
    {
        //발사 지점 설정
        if(_pivotPoint == null)
        {
            _pivotPoint = transform;
        }
        _objPos = _pivotPoint.position;

        //조준선 설정
        if(_aimingLine != null)
        {
            _aimingLine.positionCount = 2;
            _aimingLine.enabled = false;
        }
        //발사체 카메라 우선순위 저장
        if (_projectileCam != null)
        {
            _originalPriority = _projectileCam.Priority.Value;
            _projectileCam.Follow = null;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isSuccess) return;

        if (other.CompareTag("TargetZone"))
        {
            _isSuccess = true;
            //성공시 이미지 활성화
            if(_successImg != null)
            {
                _successImg.SetActive(true);
            }
            //맞추면 멈추기
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;
            _rb.bodyType = RigidbodyType2D.Kinematic;

            //코루틴 강제종료후 바로 파괴시키는 코루틴 시작
            StopAllCoroutines();
            StartCoroutine(ReturnCameraAndDestroy());
        }
    }

    private void OnMouseDown()
    {
        
        _isDragging = true;
        _dragStartPos = GetMouseWorldPosition();

        //속도 초기화, 위치 초기화
        transform.position = _objPos;
        _rb.linearVelocity = Vector2.zero;

        //조준선 활성화
        if(_aimingLine != null)
        {
            _aimingLine.enabled = true;
        }
    }
    private void OnMouseDrag()
    {

        if (!_isDragging) return;

        Vector2 currentMousePos = GetMouseWorldPosition();
        //마우스위치에서 발사체 위치까지의 벡터
        Vector2 dragVector = currentMousePos - _objPos;

        //드래그 거리 제한
        if(dragVector.magnitude > _maxDragDistance )
        {
            dragVector = dragVector.normalized * _maxDragDistance;
        }
        //당기는 반대로 움직이기
        transform.position = _objPos - dragVector;
        //드래그 반대로 힘 발생
        _currentDragDir = -dragVector;
        //조준선 위치설정
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
        //조준선 끄기
        if(_aimingLine != null)
        {
            _aimingLine.enabled = false;
        }
        //발사 힘 계산
        float forceMagnitude = _currentDragDir.magnitude * _launchForce;
        Vector2 launchForce = _currentDragDir.normalized * forceMagnitude;

        //키네마틱 해체해서 날아가게, 임펄스로 힘주기
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.AddForce(launchForce,ForceMode2D.Impulse);

        //발사후에 카메라 따라가게
        if (_projectileCam != null)
        {
            _projectileCam.Priority.Value = _activePriority;
            _projectileCam.Follow = transform;
        }

        //몇초뒤 사라지게 코루틴 시작
        StartCoroutine(HandlePostLaunch());

        //스크립트 꺼서 한번만 발사하게하기
        enabled = false;
    }

    //마우스 스크린좌표 > 월드좌표 변환
    private Vector2 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();

        // z축을 카메라 이용해서 2d 평면을 사용하게하기
        mouseScreenPos.z = Camera.main.nearClipPlane;

        //스크린좌표 월드좌표로 변환
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        return new Vector2(worldPoint.x, worldPoint.y);
    }

    //시간 초과시 카메라 복귀,오브젝트 파괴
    private IEnumerator HandlePostLaunch()
    {
        // 설정한 시간 기다리기
        yield return new WaitForSeconds(_followTime);

        // 목표 도달 실패시
        if (!_isSuccess)
        {
            // 카메라 복귀
            if (_projectileCam != null)
            {
                _projectileCam.Priority.Value = _originalPriority;
                _projectileCam.Follow = null;
            }

            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;
            _rb.bodyType = RigidbodyType2D.Kinematic;

            transform.position = _objPos;

            enabled = true;

            _isDragging = false;
        }
    }
    //목표 도달하면 실행시킬 코루틴
    private IEnumerator ReturnCameraAndDestroy()
    {
        //0.5초 대기
        yield return new WaitForSeconds(0.5f);

        // 카메라 복귀
        if (_projectileCam != null)
        {
            _projectileCam.Priority.Value = _originalPriority;
            _projectileCam.Follow = null;
        }

        // 투사체 파괴
        Destroy(gameObject);
    }
}
