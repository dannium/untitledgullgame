using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections; //

public class health : MonoBehaviour
{
    [SerializeField] GameObject plrClone;

    [SerializeField] Image img;
    [SerializeField] TextMeshProUGUI healthTxt;
    Image fadeOut;
    [SerializeField] screenShake ss;
    CinemachineFollow cm;
    Volume volume;
    private Vignette Vignette;
    float hp = 100;
    AudioSource plrHit;
    [SerializeField] AudioClip heavyThud;
    GameObject cam;
    TextMeshProUGUI questName;
    Image questGreen;
    Image ratPng;
    float currentVignette = 0.25f;
    Vector3 FollowOffset = new Vector3(0, 1.76f, -2.11f);
    float ratpngCd = 0;
    int questNum;
    int questAmount;
    int questGoal;
    int wave;
    //0 = beat 10 whts, 1 = last to wave 10, 2 = last to wave 50, 3 = beat 5 bosses, 4 = kill 20 animals
    static int[] questAmounts = new int[] { 50000, 10000, 30000, 5000, 7500 };

    EnemySpawner es;
    RoyaleSpawner rs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp = 100;
        PlayerPrefs.SetInt("bossesBeaten", 0);
        questName = GameObject.Find("questName").GetComponent<TextMeshProUGUI>();
        questGreen = GameObject.Find("questGreen").GetComponent<Image>();
        //GameObject.Find("quest").SetActive(PlayerPrefs.GetInt("quest") != 0);
        setQuestName(PlayerPrefs.GetInt("quest"));
        questNum = PlayerPrefs.GetInt("quest");
        questGoal = PlayerPrefs.GetInt("questGoal");

        fadeOut = GameObject.Find("fadeOut").transform.GetComponent<Image>();
        volume = GameObject.FindGameObjectWithTag("postProcess").GetComponent<Volume>();
        plrHit = GameObject.FindGameObjectWithTag("plr").GetComponent<AudioSource>();
        if (volume.profile.TryGet<Vignette>(out Vignette)){
        }
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        cm = GameObject.Find("CinemachineCamera").GetComponent<CinemachineFollow>();
        ratPng = GameObject.Find("ratPng").GetComponent<Image>();
        ratPng.enabled = false;

        es = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<EnemySpawner>();
        rs = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<RoyaleSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ratpngCd > 0)
        {
            ratpngCd -= Time.deltaTime;
            if (ratpngCd <= 0)
            {
                ratPng.enabled = false;
            }
        }
        img.fillAmount = Mathf.Lerp(img.fillAmount, hp / 100, 3 * Time.deltaTime);
            
        float fillAmount = (float)questAmount / (float)questGoal;
        questGreen.fillAmount = Mathf.Lerp(questGreen.fillAmount, fillAmount, 3 * Time.deltaTime);

        Vignette.intensity.value = Mathf.Lerp(Vignette.intensity.value, currentVignette, 6.7f*Time.deltaTime);
        Vignette.color.value = new Color((Vignette.intensity.value-0.25f)/2, 0, 0, 0);
        if(Vignette.intensity.value > 0.9f) {
            currentVignette = 0.25f;
        }



        if(hp <= 0) {
            if(FollowOffset.z < 1.9f && FollowOffset.x != 0.01f) {
                FollowOffset = new Vector3(0f, 2, Mathf.Lerp(FollowOffset.z, 2.5f, 4*Time.deltaTime));
            } else {
                FollowOffset = new Vector3(0.01f, Mathf.Lerp(FollowOffset.y, -3, Time.deltaTime), Mathf.Lerp(FollowOffset.z, 1.12f, Time.deltaTime));
            }
            if(cam.transform.position.y > 1)
            {
                fadeOut.color = new Color(0.06f, 0.06f, 0.12f, Mathf.Lerp(fadeOut.color.a, 1, 3*Time.deltaTime));
            } else
            {
                if((float)questAmount/(float)questGoal >= 1)
                {
                    PlayerPrefs.SetInt("questAmount", questAmounts[questNum]);
                } else
                {
                    PlayerPrefs.SetInt("questAmount", 0);
                }

                SceneManager.LoadScene("shop");
            }
            hp = 0;
            cm.FollowOffset = FollowOffset;
            currentVignette = 1;
        }
    }

    public float getHealth() {
        return hp;
    }

    public void setHealth(float newHealth) {
        if(newHealth < hp) {
            plrHit.pitch = Random.Range(0.900f, 1.100f);
            plrHit.Play();
            currentVignette = 1;
            ss.ScreenShake(6, 5);
        }
        hp = Mathf.Min(newHealth, 100);
        healthTxt.text = "" + hp;


        if (hp <= 0)
        {
            if (wave < 50)
            {
                PlayerPrefs.SetInt("pgTokens", 3);
            }
            else
            {
                PlayerPrefs.SetInt("pgTokens", 0);
            }
            PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + 3);
        }
    
    }

    public void rubberduck()
    {
        GameObject.Find("gull").SetActive(false);
        GameObject.Find("duckGull").transform.GetComponent<MeshRenderer>().enabled = true;
    }

    public void ratAppear()
    {
        if (Random.Range(0, 2000) == 67)
        {
            ratPng.enabled = true;
            ratpngCd = 0.3f;
        }
    }

    public void showClone()
    {
        plrClone.transform.position = new Vector3(1.773f, 2.306f, -0.849f);
    }

    void setQuestName(int n)
    {
       if(n == -1)
       {
            GameObject.Find("quest").SetActive(false);
       } else
       {
            //0 = beat 10 whts, 1 = last to wave 10, 2 = last to wave 50, 3 = beat 5 bosses, 4 = kill 20 animals
            string[] names = new string[] { "Beat 10 William Howard Tafts", "Last 10 waves", "Last 50 waves", "Beat 5 bosses", "Kill 20 animals, you monster." };
            questName.text = "Quest: " + names[n];
            int[] goals = new int[] { 10, 10, 50, 5, 20 };
            PlayerPrefs.SetInt("questGoal", goals[n]);
       }
    }

    public void qInc(string name)
    {
        if(name == "wave")
        {
            wave++;
            if(questNum == 1 || questNum == 2)
            {
                questAmount++;
            }
        } else if(name == "boss")
        {
            PlayerPrefs.SetInt("bossesBeaten", PlayerPrefs.GetInt("bossesBeaten") + 1);
            if(questNum == 3)
            {
                questAmount++;
            }
        } else if( (name == "animal" && questNum == 4) )
        {
            questAmount++;
        } else if(name == "wht" && questNum == 0)
        {
            questAmount++;
        }
    }

}
