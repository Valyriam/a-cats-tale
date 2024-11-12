using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    enum AllCameraShakeTypes { regularCam, cinemachineCam }
    [SerializeField] AllCameraShakeTypes cameraShakeType;

    [Header("Regular Camera")]
    [SerializeField] Transform camTransform;
    [SerializeField] float defaultShakeDuration = 0.5f;
    [SerializeField] float defaultShakeAmount = 0.7f;
    [SerializeField] float decreaseFactor = 1.0f;

    [Header("Cinemachine Camera")]
    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField] CinemachineBasicMultiChannelPerlin virtualCamNoise;
    [SerializeField] float defaultCinemachineShakeDuration = 0.5f;
    [SerializeField] float defaultCinemachineAmplitude = 0.1f;

    Vector3 originalPos;
    float shakeTimer;

    void Awake()
    {
        Instance = this;

        if (cameraShakeType == AllCameraShakeTypes.regularCam)
        {
            if (camTransform == null)
            {
                camTransform = GetComponent(typeof(Transform)) as Transform;
            }
        }

        else
        {
            if (virtualCam == null)
            {
                virtualCam = GetComponent<CinemachineVirtualCamera>();
            }

            if (virtualCamNoise == null)
            {
                virtualCamNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
        }
    }

    private void Update()
    {
        //regular cam
        if (cameraShakeType == AllCameraShakeTypes.regularCam)
        {
            if (shakeTimer > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * defaultShakeAmount;
                shakeTimer -= Time.deltaTime * decreaseFactor;
            }

            else
            {
                camTransform.localPosition = originalPos;
                shakeTimer = 0;
            }
        }

        //cinemachine cam
        else
        {
            if(shakeTimer > 0)
            {
                virtualCamNoise.m_AmplitudeGain = defaultCinemachineAmplitude;
                shakeTimer -= Time.deltaTime;
            }

            else
            {
                virtualCamNoise.m_AmplitudeGain = 0;
                shakeTimer = 0;
            }
        }
    }

    #region Cinemachine Cameras

    public void ShakeCustomTime(float shakeTime) => shakeTimer = shakeTime;
    public void ShakeCinemachineDefaultTime() => shakeTimer = defaultCinemachineShakeDuration;

    #endregion

    #region Regular Cameras

    public void ShakeCameraDefaultTime() => shakeTimer = defaultCinemachineShakeDuration;

    #endregion
}
