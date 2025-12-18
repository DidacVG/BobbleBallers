using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    void Start()
    {
        // Cargar volumen guardado
        float savedVolume = PlayerPrefs.GetFloat("Master", 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
    }

    public void SetVolume(float value)
    {
        // Conversión lineal → decibelios
        audioMixer.SetFloat(
            "Master",
            Mathf.Log10(value) * 20f
        );

        PlayerPrefs.SetFloat("Master", value);
    }
}

