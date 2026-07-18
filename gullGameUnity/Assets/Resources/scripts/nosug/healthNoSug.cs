using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections;

public class healthNoSug : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI healthTxt;
    [SerializeField] Image img;
    Image fadeOut;
    [SerializeField] screenShake ss;
    CinemachineFollow cm;
    Volume volume;
    private Vignette Vignette;
    float hp = 100;
    AudioSource plrHit;
    [SerializeField] AudioClip heavyThud;
    GameObject cam;
    float currentVignette = 0.25f;
    Vector3 FollowOffset = new Vector3(0, 1.76f, -2.11f);
    int wave;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp = 100;
        PlayerPrefs.SetInt("bossesBeaten", 0);

        fadeOut = GameObject.Find("fadeOut").transform.GetComponent<Image>();
        volume = GameObject.FindGameObjectWithTag("postProcess").GetComponent<Volume>();
        plrHit = GameObject.FindGameObjectWithTag("plr").GetComponent<AudioSource>();
        if (volume.profile.TryGet<Vignette>(out Vignette))
        {
        }
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        cm = GameObject.Find("CinemachineCamera").GetComponent<CinemachineFollow>();
    }

    // Update is called once per frame
    void Update()
    {

        Vignette.intensity.value = Mathf.Lerp(Vignette.intensity.value, currentVignette, 6.7f * Time.deltaTime);
        Vignette.color.value = new Color((Vignette.intensity.value - 0.25f) / 2, 0, 0, 0);
        if (Vignette.intensity.value > 0.9f)
        {
            currentVignette = 0.25f;
        }



        if (hp <= 0)
        {
            if (FollowOffset.z < 1.9f && FollowOffset.x != 0.01f)
            {
                FollowOffset = new Vector3(0f, 2, Mathf.Lerp(FollowOffset.z, 2.5f, 4 * Time.deltaTime));
            }
            else
            {
                FollowOffset = new Vector3(0.01f, Mathf.Lerp(FollowOffset.y, -3, Time.deltaTime), Mathf.Lerp(FollowOffset.z, 1.12f, Time.deltaTime));
            }
            if (cam.transform.position.y > 1)
            {
                fadeOut.color = new Color(0.06f, 0.06f, 0.12f, Mathf.Lerp(fadeOut.color.a, 1, 3 * Time.deltaTime));
            }
            else
            {

                SceneManager.LoadScene("noSugMain");
            }
            hp = 0;
            cm.FollowOffset = FollowOffset;
            currentVignette = 1;
        }

        img.fillAmount = Mathf.Lerp(img.fillAmount, hp / 100, 3 * Time.deltaTime);

    }


    public float getHealth()
    {
        return hp;
    }

    public void setHealth(float newHealth)
    {
        if (newHealth < hp)
        {
            plrHit.pitch = Random.Range(0.900f, 1.100f);
            plrHit.Play();
            currentVignette = 1;
            ss.ScreenShake(6, 5);
        }
        hp = Mathf.Min(newHealth, 100);
        healthTxt.text = "" + hp;

    }
}