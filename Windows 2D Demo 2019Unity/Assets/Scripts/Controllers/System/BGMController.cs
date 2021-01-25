using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    private AudioSource AudioPlayer;
    public float volume = 1f;
    public AudioClip BGM;
    public AudioClip BGM2; //TODO
    public AudioClip BGM3; //TODO
    public bool ChangeBGM;
    // Start is called before the first frame update
    void Start()
    {
        AudioPlayer = this.gameObject.AddComponent<AudioSource>();
        AudioPlayer.clip = BGM;
        AudioPlayer.Play();
        AudioPlayer.volume = volume;
        AudioPlayer.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        AudioPlayer.volume = volume;
    }
}
