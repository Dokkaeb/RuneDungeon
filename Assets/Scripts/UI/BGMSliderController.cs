
using UnityEngine;
using UnityEngine.UI;

public class BGMSliderController : MonoBehaviour
{
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();

        _slider.minValue = 0.0001f;
        _slider.maxValue = 1f;
    }
    private void Start()
    {
        if(SoundManager.Instance != null)
        {
            float savedVolume = SoundManager.Instance.GetSavedMasterVolume();
            _slider.value = savedVolume;
        }
    }
    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
    private void OnSliderValueChanged(float value)
    {
        
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetMasterVolume(value);
        }
    }
}
