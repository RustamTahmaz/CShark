using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("UI Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // Initialize slider positions from AudioManager
        musicSlider.value = AudioManager.Instance.musicVolume;
        sfxSlider.value   = AudioManager.Instance.sfxVolume;

        // Wire up callbacks
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged  .AddListener(OnSfxChanged);
    }

    public void OnMusicChanged(float v)
    {
        AudioManager.Instance.SetMusicVolume(v);
    }

    public void OnSfxChanged(float v)
    {
        AudioManager.Instance.SetSfxVolume(v);
    }
}
