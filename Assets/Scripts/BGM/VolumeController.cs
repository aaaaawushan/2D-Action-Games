using UnityEngine;
using UnityEngine.UI;
public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = savedVolume;
        volumeSlider.value = savedVolume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
