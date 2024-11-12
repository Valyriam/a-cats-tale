using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public enum AllVolumeTypes
    { 
        master,
        music,
        sfx,
        ambiance,
        narrator
    }

    [SerializeField] AudioMixer audioMixer;

    private void Start()
    {
        SetDefaultVolumes();
    }

    public void AddToVolume(AllVolumeTypes volumeType)
    {
        if(volumeType == AllVolumeTypes.master)
        {
            //collect volume
            audioMixer.GetFloat("masterVolume", out float currentVolumeLevel);

            //set lowest volume
            if ((currentVolumeLevel + 2) <= -40)
                audioMixer.SetFloat("masterVolume", -40);

            //set volume
            else if ((currentVolumeLevel + 4) >= 0)
                audioMixer.SetFloat("masterVolume", 0);

            else audioMixer.SetFloat("masterVolume", currentVolumeLevel + 4);
        }

        else if (volumeType == AllVolumeTypes.music)
        {
            //collect volume
            audioMixer.GetFloat("musicVolume", out float currentVolumeLevel);

            //set lowest volume
            if ((currentVolumeLevel + 2) <= -40)
                audioMixer.SetFloat("musicVolume", -40);

            //set volume
            else if ((currentVolumeLevel + 4) >= 0)
                audioMixer.SetFloat("musicVolume", 0);

            else audioMixer.SetFloat("musicVolume", currentVolumeLevel + 4);
        }

        else if (volumeType == AllVolumeTypes.sfx)
        {
            //collect volume
            audioMixer.GetFloat("sfxVolume", out float currentVolumeLevel);

            //set lowest volume
            if ((currentVolumeLevel + 2) <= -40)
                audioMixer.SetFloat("sfxVolume", -40);

            //set volume
            else if ((currentVolumeLevel + 4) >= 0)
                audioMixer.SetFloat("sfxVolume", 0);

            else audioMixer.SetFloat("sfxVolume", currentVolumeLevel + 4);
        }

        else if(volumeType == AllVolumeTypes.ambiance)
        {
            //collect volume
            audioMixer.GetFloat("ambianceVolume", out float currentVolumeLevel);

            //set lowest volume
            if ((currentVolumeLevel + 2) <= -40)
                audioMixer.SetFloat("ambianceVolume", -40);

            //set volume
            else if ((currentVolumeLevel + 4) >= 0)
                audioMixer.SetFloat("ambianceVolume", 0);

            else audioMixer.SetFloat("ambianceVolume", currentVolumeLevel + 4);
        }

        else if(volumeType == AllVolumeTypes.narrator)
        {
            //collect volume
            audioMixer.GetFloat("narratorVolume", out float currentVolumeLevel);

            //set lowest volume
            if ((currentVolumeLevel + 2) <= -40)
                audioMixer.SetFloat("narratorVolume", -40);

            //set max volume
            else if ((currentVolumeLevel + 2) >= 0)
                audioMixer.SetFloat("narratorVolume", 0);

            else audioMixer.SetFloat("narratorVolume", currentVolumeLevel + 2);
        }
    }

    public void SubtractFromVolume(AllVolumeTypes volumeType)
    {
        if (volumeType == AllVolumeTypes.master)
        {
            //collect volume
            audioMixer.GetFloat("masterVolume", out float currentVolumeLevel);

            //set volume
            if ((currentVolumeLevel - 2) <= -40)
                audioMixer.SetFloat("masterVolume", -80);

            else audioMixer.SetFloat("masterVolume", currentVolumeLevel - 2);
        }

        else if (volumeType == AllVolumeTypes.music)
        {
            //collect volume
            audioMixer.GetFloat("musicVolume", out float currentVolumeLevel);

            //set volume
            if ((currentVolumeLevel - 2) <= -40)
                audioMixer.SetFloat("musicVolume", -80);

            else audioMixer.SetFloat("musicVolume", currentVolumeLevel - 2);
        }

        else if (volumeType == AllVolumeTypes.sfx)
        {
            //collect volume
            audioMixer.GetFloat("sfxVolume", out float currentVolumeLevel);

            //set volume
            if ((currentVolumeLevel - 2) <= -40)
                audioMixer.SetFloat("sfxVolume", -80);

            else audioMixer.SetFloat("sfxVolume", currentVolumeLevel - 2);
        }

        else if (volumeType == AllVolumeTypes.ambiance)
        {
            //collect volume
            audioMixer.GetFloat("ambianceVolume", out float currentVolumeLevel);

            //set volume
            if ((currentVolumeLevel - 2) <= -40)
                audioMixer.SetFloat("ambianceVolume", -80);

            else audioMixer.SetFloat("ambianceVolume", currentVolumeLevel - 2);
        }

        else if (volumeType == AllVolumeTypes.narrator)
        {
            //collect volume
            audioMixer.GetFloat("narratorVolume", out float currentVolumeLevel);

            //set volume
            if ((currentVolumeLevel - 2) <= -40)
                audioMixer.SetFloat("narratorVolume", -80);

            else audioMixer.SetFloat("narratorVolume", currentVolumeLevel - 2);
        }
    }

    public void SetDefaultVolumes()
    {
        audioMixer.SetFloat("masterVolume", -12);
        audioMixer.SetFloat("musicVolume", 0);
        audioMixer.SetFloat("sfxVolume", 0);
        audioMixer.SetFloat("ambianceVolume", 0);
        audioMixer.SetFloat("narratorVolume", 0);
    }

    public void AddToMasterVolume() => AddToVolume(AllVolumeTypes.master);
    public void AddToNarratorVolume() => AddToVolume(AllVolumeTypes.narrator);
    public void AddToAmbianceVolume() => AddToVolume(AllVolumeTypes.ambiance);
    public void AddToMusicVolume() => AddToVolume(AllVolumeTypes.music);
    public void AddToSFXVolume() => AddToVolume(AllVolumeTypes.sfx);

    public void SubtractFromMasterVolume() => SubtractFromVolume(AllVolumeTypes.master);
    public void SubtractFromNarratorVolume() => SubtractFromVolume(AllVolumeTypes.narrator);
    public void SubtractFromAmbianceVolume() => SubtractFromVolume(AllVolumeTypes.ambiance);
    public void SubtractFromMusicVolume() => SubtractFromVolume(AllVolumeTypes.music);
    public void SubtractFromSFXVolume() => SubtractFromVolume(AllVolumeTypes.sfx);
}
