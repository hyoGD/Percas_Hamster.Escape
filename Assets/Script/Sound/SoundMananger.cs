using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMananger : MonoBehaviour
{
    public static SoundMananger instance;
    [SerializeField] public AudioSource audioSource, audioSourceMusic;
    [SerializeField] public AudioClip[] sound;

    private void Awake()
    {
       if(instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundAction(int num)
    {
        audioSource.clip = sound[num];
        audioSource.Play();
        audioSource.loop = false;

    }
}
