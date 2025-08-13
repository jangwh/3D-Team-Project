using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MonsterAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private int nextAttackSoundIndex = 0;

    [Header("오디오 클립")]
    public AudioClip ChaseSound;
    public AudioClip[] attackSounds;
    public AudioClip deathSound;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayChaseSound()
    {
        if (ChaseSound != null)
        {
            audioSource.PlayOneShot(ChaseSound);
        }
    }

    public void PlayAttackSound()
    {
        if (attackSounds == null || attackSounds.Length == 0)
        {
            return;
        }

        AudioClip clipToPlay = attackSounds[nextAttackSoundIndex];
        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }

        nextAttackSoundIndex = (nextAttackSoundIndex + 1) % attackSounds.Length;
    }

    public void PlayDeathSound()
    {
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
}
