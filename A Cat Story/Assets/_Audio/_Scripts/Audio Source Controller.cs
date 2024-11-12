using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioSourceController : MonoBehaviour
{

    AudioSource myAudioSource;

    [SerializeField] float presetFadeDuration = 3;

    [Range(0f, 1f)]
    [SerializeField] float presetVolume = 1;

    [Header("Events")]
    public UnityEvent afterFadeIn = new();
    public UnityEvent afterFadeOut = new();

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    IEnumerator PresetFadeIntoNewClipCoroutine(AudioClip newAudioClip)
    {
        PresetFadeOut();

        float fadeTimer = 0;

        while (fadeTimer < presetFadeDuration)
        {
            fadeTimer += Time.deltaTime;
            yield return null;
        }

        myAudioSource.clip = newAudioClip;

        PresetFadeIn();
    }

    public void PresetFadeIntoNewClip(AudioClip newAudioClip) => StartCoroutine(PresetFadeIntoNewClipCoroutine(newAudioClip));


    public void PresetFadeIn() => StartCoroutine(StartFadeIn(myAudioSource, presetFadeDuration, presetVolume));

    public void PresetFadeOut() => StartCoroutine(StartFadeOut(myAudioSource, presetFadeDuration));



    public void FadeIn(float duration, float targetVolume) => StartCoroutine(StartFadeIn(myAudioSource, duration, targetVolume));

    IEnumerator StartFadeIn(AudioSource audioSource, float duration, float targetVolume)
    {
        audioSource.Play();

        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        afterFadeIn.Invoke();

        yield break;
    }

    public void FadeOut(float duration, float targetVolume) => StartCoroutine(StartFadeOut(myAudioSource, duration));

    IEnumerator StartFadeOut(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
            yield return null;
        }

        audioSource.Stop();

        afterFadeOut.Invoke();

        yield break;
    }
}
