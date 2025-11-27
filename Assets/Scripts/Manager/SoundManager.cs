using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioMixer masterMixer;
    private const string MASTER_VOLUME_PARAM = "MasterVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        float savedVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_PARAM, 0.75f); // 기본값 0.75
        SetMasterVolume(savedVolume);
    }
    public void SetMasterVolume(float volume)
    {
        
        // 0일 때 -80dB (거의 무음), 1일 때 0dB (최대 음량)
        float dB = Mathf.Log10(volume) * 20;

        
        // volume이 0.0001f 이하일 경우, -80dB로 설정하여 무음 처리합니다.
        if (volume <= 0.0001f)
        {
            masterMixer.SetFloat(MASTER_VOLUME_PARAM, -80f);
        }
        else
        {
            masterMixer.SetFloat(MASTER_VOLUME_PARAM, dB);
        }

        // 볼륨 설정을 저장합니다.
        PlayerPrefs.SetFloat(MASTER_VOLUME_PARAM, volume);
    }

    public float GetSavedMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_PARAM, 0.75f);
    }
}
