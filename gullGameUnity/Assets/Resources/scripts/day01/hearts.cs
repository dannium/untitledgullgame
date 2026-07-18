using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using System;
using System.Threading;

public class hearts : MonoBehaviour
{
    public int maxHearts = 5;
    public int heartCount;
    float cd = 0;
    [SerializeField] TextMeshProUGUI heartText;
    [SerializeField] Canvas loseScreen;
    [SerializeField] GameObject barrier;
    screenShake ss;
    AudioSource plrHit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ss = GameObject.Find("CinemachineCamera").GetComponent<screenShake>();
        plrHit = GameObject.FindWithTag("plr").GetComponent<AudioSource>();
        heartCount = maxHearts;
    }

    // Update is called once per frame
    void Update()
    {
        if (cd > 0)
        {
            cd -= Time.deltaTime;
        }
        if (cd < 0)
        {
            cd = 0;
        }

        if (heartCount == 0)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0, 0.01f);
        }
    }

    public void LoseHealth()
    {
        if (cd <= 0)
        {
            ss.ScreenShake(50, 15f);
            heartCount--;
            plrHit.pitch = UnityEngine.Random.Range(0.900f, 1.100f);
            plrHit.Play();
            heartText.text = $"Hearts: {heartCount}/{maxHearts}";
            if (heartCount == 0)
            {
                loseScreen.enabled = true;
                barrier.SetActive(true);
            }
            cd = 0.5f;
        }
    }

    public void GainHealth()
    {
        if (heartCount < maxHearts)
        {
            heartCount++;
            heartText.text = $"Hearts: {heartCount}/{maxHearts}";
        }
    }
    public void SetHealth(int a)
    {
        heartCount = a;
        if (heartCount == 0)
        {
            loseScreen.enabled = true;
        }
    }
    
}
