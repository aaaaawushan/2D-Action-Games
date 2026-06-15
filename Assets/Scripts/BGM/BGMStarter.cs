using UnityEngine;

public class BGMStarter : MonoBehaviour
{
    [SerializeField] private AudioSource BGM;
    [SerializeField] private float startTime=2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BGM.time = startTime;
        BGM.Play();
    }

    
}
