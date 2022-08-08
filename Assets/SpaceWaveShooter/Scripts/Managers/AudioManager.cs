using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Ship")]
    public AudioClip shipShoot;
    public AudioClip shipHit;
    public AudioClip shipExplode;

    [Header("UI")]
    public AudioClip buttonHover;
    public AudioClip buttonClick;

    [Header("Audio Sources")]
    public AudioSource defaultAudioSource;
    public GameObject audioSourcePrefab;

    // Instance
    public static AudioManager inst;

    void Awake ()
    {
        // Set the instance to this script.
        inst = this;
    }

    // Play a sound effect through a temporary audio source.
    public void PlayWithTempAudioSource (AudioClip sfx, Vector3 soundPosition, bool randomPitch = false)
    {
        AudioSource tempAudioSource = Pool.inst.Spawn(audioSourcePrefab, soundPosition, Quaternion.identity).GetComponent<AudioSource>();
        Pool.inst.Destroy(tempAudioSource.gameObject, sfx.length);

        Play(sfx, tempAudioSource, randomPitch);
    }

    // Plays a sound effect.
    public void Play (AudioClip sfx, AudioSource audioSource, bool randomPitch = false)
    {
        float pitch = randomPitch ? Random.Range(0.9f, 1.1f) : 1.0f;
        audioSource.pitch = pitch;

        audioSource.PlayOneShot(sfx);
    }
}