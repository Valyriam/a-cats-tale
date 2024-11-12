using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public GameObject audioSourcePrefab;
    public void PlaySFX(SFXObject sfxObject)
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity).GetComponent<AudioSource>();
       
        audioSource.outputAudioMixerGroup = sfxObject.mixerGroup;
        audioSource.volume = sfxObject.volume;
        audioSource.spatialBlend = 0; //2d 
        audioSource.loop = sfxObject.loop;

        //randomize clip
        if(sfxObject.randomizeClips)
        {
            int totalAudioClipOptions = sfxObject.audioClips.Count;
            audioSource.clip = sfxObject.audioClips[Random.Range(0, (totalAudioClipOptions - 1))];
        }

        else
        {
            audioSource.clip = sfxObject.clip;
        }


        //randomize pitch
        if(sfxObject.randomizePitch)
        {
            audioSource.pitch = Random.Range(sfxObject.pitchRangeLowerValue, sfxObject.pitchRangeHigherValue);
        }

        audioSource.Play();
    }

    public void Play3DSFX(SFXObject sfxObject, Transform audioSourceLocation)
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity).GetComponent<AudioSource>();

        audioSource.clip = sfxObject.clip;
        audioSource.outputAudioMixerGroup = sfxObject.mixerGroup;
        audioSource.volume = sfxObject.volume;
        audioSource.loop = sfxObject.loop;
        audioSource.spatialBlend = 1; //3d

        if (sfxObject.randomizePitch)
        {
            audioSource.pitch = Random.Range(sfxObject.pitchRangeLowerValue, sfxObject.pitchRangeHigherValue);
        }

        audioSource.Play();
    }
}
