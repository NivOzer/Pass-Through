using UnityEngine;

public class AudioManagerX : MonoBehaviour
{
    [SerializeField] AudioClip miss;
    private AudioSource audioManager;

    void Start()
    {
        audioManager = GetComponent<AudioSource>();
    }
    public void PlayMissSound(){
        audioManager.PlayOneShot(miss,0.2f);
    }
}
