using UnityEngine;
using Unity.Cinemachine;
using System;
public class screenShake : MonoBehaviour
{

    CinemachineBasicMultiChannelPerlin cm;
    float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cm = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cm.AmplitudeGain >= 0.01f)
        {
            cm.AmplitudeGain = Mathf.Lerp(cm.AmplitudeGain, 0, Time.deltaTime * speed);
        }
        else
        {
            cm.AmplitudeGain = 0;
        }
    }

    public void ScreenShake(float i, float s)
    {
        cm.AmplitudeGain = i;
        speed = s;
    }
}
