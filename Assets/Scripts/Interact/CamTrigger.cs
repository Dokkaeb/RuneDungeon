using UnityEngine;
using Unity.Cinemachine;

public class CamTrigger : MonoBehaviour
{
    [SerializeField] CinemachineCamera _cam1;
    [SerializeField] CinemachineCamera _cam2;
    private const int ActivePriority = 20; // 활성화할 카메라의 우선순위
    private const int InactivePriority = 5;
    private const float Cam1DisplayTime = 2f;

    private void Start()
    {
        if (_cam1 != null) _cam1.Priority.Value = InactivePriority;
        if (_cam2 != null) _cam2.Priority.Value = InactivePriority;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_cam1 != null)
            {
                _cam1.Priority.Value = ActivePriority;
            }
            Invoke("SwitchToCam2", Cam1DisplayTime);

            GetComponent<Collider2D>().enabled = false;
        }
    }
    private void SwitchToCam2()
    {
        if (_cam2 != null)
        {
            // 1. cam1 비활성화 및 cam2 활성화
            if (_cam1 != null) _cam1.Priority.Value = InactivePriority;
            _cam2.Priority.Value = ActivePriority;
        }

        // 2. 카메라 전환 후, 오브젝트 파괴를 위한 함수를 호출
        // cam2가 표시되는 시간이 필요하다면 이 대기 시간을 추가할 수 있습니다.
        // 요구사항에 '다 보고난 뒤엔'이라고 하셨으므로, cam2를 잠깐 보여줄 시간을 가정하고 1초로 설정합니다.
        Invoke("DestroyTrigger", 4f);
    }
    private void DestroyTrigger()
    {
        // 1. cam2 비활성화 (자동으로 기본 카메라로 복귀)
        if (_cam2 != null)
        {
            _cam2.Priority.Value = InactivePriority;
        }

        // 2. 이 Trigger 오브젝트 파괴
        Destroy(gameObject);
    }
}
