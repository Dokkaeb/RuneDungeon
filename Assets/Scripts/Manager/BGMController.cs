using UnityEngine;

public class BGMController : MonoBehaviour
{
    public static BGMController Instance { get; private set; }
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _bgmClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 새 씬에 이미 인스턴스가 존재하면 새로운 인스턴스를 파괴
            Destroy(gameObject);
            return; // 이후 코드가 실행되지 않도록 막음
        }

        _audioSource = GetComponent<AudioSource>();

        _audioSource.clip = _bgmClip;
        _audioSource.loop = true;

        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}
