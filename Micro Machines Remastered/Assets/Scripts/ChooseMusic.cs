using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMusic : MonoBehaviour {

    private AudioSource audioSjors;
    public AudioClip[] songs;
    private AudioClip shootClip;
    
	void Start () {

        audioSjors = gameObject.GetComponent<AudioSource>();

        int index = Random.Range(0, songs.Length);
        shootClip = songs[index];
        audioSjors.clip = shootClip;
        audioSjors.Play();
		
	}
}
