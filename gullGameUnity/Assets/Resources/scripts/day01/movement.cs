//using System;
//using NUnit.Framework;
using TMPro;
//using Unity.Mathematics;
//using Unity.Mathematics.Geometry;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class movement : MonoBehaviour
{

    /*[SerializeField] Rigidbody rb;
    public float speed = 7;
    [SerializeField] float jumpSpeed = 100;
    [SerializeField] shop shop;
    [SerializeField] timer timer;
    [SerializeField] GameObject barrier;
    [SerializeField] hearts hearts;
    [SerializeField] GameObject winArea;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI hScoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] tileSpawner ts;
    [SerializeField] AudioSource wobble;
    [SerializeField] AudioSource thud;
    [SerializeField] AudioSource walk;*/
    [SerializeField] screenShake ss;
    Animator anim; 
    GameObject bullet;
    /*public bool inRound;
    Vector3 targetRot;
    int score;

    AnimatorStateInfo stateInfo;
    AnimatorClipInfo[] clipInfo;
    int currentFrame;
    int timerSub = 6;*/

    float currentTurn = 0;
    float cd = 0;
    Vector3 Raypos;
    RaycastHit hit;
    public float KillCount;
    public TextMeshProUGUI KillCountTxt;
    GameObject tornadoPrefab;
    GameObject plr;
    Transform enemies;
    bool godmode;
    health health;
    EnemySpawner es;
    bool ricochet = true;
    bool tornadoShoot = true;
    poof poof;
    public AudioSource nukeAudio;
    Sprite[] flags;

    private static float[][] upgradeAmounts = new float[][] {
    new float[] {1,0.8f,0.6f,0.5f,0.4f,0.3f,0.2f,0.15f,0.1f,0.05f,0f}, // reload time
    new float[] {1,1.2f,1.4f,1.6f,1.8f,2f,2.1f,2.2f,2.3f,2.4f,2.5f}, // rot speed
    new float[] {1,1.2f,1.4f,1.6f,1.8f,2f,2.25f,2.5f,2.75f,3f,4f}, // damage
    new float[] {1,1.1f,1.2f,1.4f,1.6f,1.8f,2f,2.25f,2.5f,2.75f,3}  // cheese tokens
        };

    private static string[] tagList = new string[] { "enemy", "butcher", "trio", "orb", "enemy2", "ratEnemy", "watermelon"};


    float reloadVar;
    float rotationVar;
    float damageVar;
    float cheeseVar;
    bool nuke = false;
    bool nuked = false; //if already nuked

    int gunType;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt("nuked", 0);
        gunType = PlayerPrefs.GetInt("gunType");

        plr = GameObject.FindGameObjectWithTag("plr");

        if(PlayerPrefs.GetInt("mode") != 3)
        {
            GameObject.Find("altf4").SetActive(false);
        }

        GameObject.Find("GUN").GetComponent<SkinnedMeshRenderer>().enabled = (gunType == 0);
        GameObject.Find("gunC").GetComponent<MeshRenderer>().enabled = (gunType == 1);
        GameObject.Find("gunR").GetComponent<MeshRenderer>().enabled = (gunType == 2);
        GameObject.Find("gunS").GetComponent<MeshRenderer>().enabled = (gunType == 3);
        GameObject.Find("gunT").GetComponent<MeshRenderer>().enabled = (gunType == 4);

        ricochet = gunType == 2;
        tornadoShoot = gunType == 4;


        reloadVar = upgradeAmounts[0][PlayerPrefs.GetInt("reloadVar", 0)] * ((gunType == 3) ? 1.5f : 1);
        rotationVar = upgradeAmounts[1][PlayerPrefs.GetInt("rotationVar", 0)];
        damageVar = upgradeAmounts[2][PlayerPrefs.GetInt("damageVar", 0)] * ((gunType == 3) ? 1.5f : 1);
        cheeseVar = upgradeAmounts[3][PlayerPrefs.GetInt("cheeseVar", 0)];

        GetComponentInChildren<Animator>().SetFloat("animSpeed", (reloadVar != 0) ? 1/reloadVar : 100);


        godmode = false;
        KillCount = 0;
        KillCountTxt = GameObject.Find("KillNum").GetComponent<TextMeshProUGUI>();
        health = GameObject.FindGameObjectWithTag("gui").GetComponent<health>();
        es = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<EnemySpawner>();
        poof = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<poof>();
        bullet = GameObject.Find("bullet");
        tornadoPrefab = Resources.Load<GameObject>("prefabs/day3/tornadoPrefab");
        Time.timeScale = 1;
        anim = GetComponentInChildren<Animator>();
        Raypos = (transform.position + transform.forward*2);
        enemies = GameObject.FindGameObjectWithTag("enemies").transform;
        flags = Resources.LoadAll<Sprite>("materials/day3/flags");
        //inRound = true;
        //hScoreText.text = "High Score: " + PlayerPrefs.GetInt("hScore");

        Resources.Load<Material>("materials/day2/chocMilk").SetFloat(Shader.PropertyToID("_colorAmount"), 0.98f*Random.Range(0, 2));
    }

    // Update is called once per frame
    void Update()
    {

        if((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.Alpha4) && PlayerPrefs.GetInt("mode") == 3)
        {
            godmode = true;
            health.setHealth(9999999);
        }

        currentTurn = Mathf.Lerp(currentTurn, Time.deltaTime*100*Input.GetAxisRaw("Horizontal")*rotationVar*0.75f, Time.deltaTime*8);
        transform.Rotate(0f, currentTurn, 0f);
        cd -= Time.deltaTime;

        if(cd < reloadVar-0.1f)
        {
            bullet.GetComponent<MeshRenderer>().enabled = false;
        }

        if(cd <= 0 && (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))) {

            anim.SetBool("shooting", true);
            cd = reloadVar;
            if(godmode)
            {
                foreach(Transform t in enemies.GetComponentInChildren<Transform>())
                {
                    Destroy(t.gameObject);
                    poof.Poofs(t.position, 1.5f);
                }
                es.NextWave();
            }

            if(tornadoShoot)
            {
                GameObject tornado = Instantiate(tornadoPrefab);
                tornado.GetComponent<tornadoMovement>().angle = Mathf.RoundToInt(plr.transform.eulerAngles.y);
            }

            ss.ScreenShake(15, 15);
            bullet.GetComponent<AudioSource>().pitch = Random.Range(0.900f, 1.100f);
            bullet.GetComponent<AudioSource>().Play();

            Raypos = (transform.position + transform.forward*2);
            Raypos = new Vector3(Raypos.x, 0.25f, Raypos.z);
            RaycastHit hit;            
            bullet.GetComponent<MeshRenderer>().enabled = true;
            Vector3 rayRot = transform.forward;

            float jic = (gunType == 2) ? 0 : 2;
            while(Physics.Raycast(Raypos, rayRot, out hit, 100) && jic < 3)
            {
                //Debug.DrawRay(Raypos, rayRot*100, Color.green, 5f);
                jic++;
                if (jic == 2)
                {
                    print("maybe work");
                }
                if(nuke && !nuked)
                {
                    nuked = true;
                    nukeAudio.PlayOneShot(nukeAudio.clip);
                    StartCoroutine(Nuke());
                }

                bool hitEnemy = false;
                Vector3 hitPos = hit.point;
                foreach(string item in tagList)
                {
                    if(hit.collider.tag == item)
                    {
                        hitEnemy = true;
                        KillCount++;
                        KillCountTxt.text = "" + KillCount;

                        switch(item)
                        {
                            case "enemy":
                                PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * cheeseVar));
                                hit.transform.GetComponent<SlimeMovement>().LoseHealth(damageVar);
                                if((hit.transform.name == "DarkSouls") || (hit.transform.name == "jester"))
                                {
                                    health.qInc("boss");
                                }
                                break;
                            case "butcher":
                                PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * cheeseVar));
                                hit.transform.GetComponent<butcherMovement>().LoseHealth();
                                break;
                            case "trio":
                                PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * cheeseVar));
                                hit.transform.GetComponent<trioMovement>().LoseHealth(damageVar);
                                break;
                            case "orb":
                                PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * cheeseVar));
                                hit.transform.GetComponent<orbMovement>().LoseHealth(damageVar);
                                break;
                            case "enemy2":
                                PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * cheeseVar));
                                hit.transform.GetComponent<EnemyMovement>().LoseHealth(damageVar);

                                if(hit.transform.name == "btPrefab(Clone)")
                                {
                                    StartCoroutine(btpfp());
                                }

                                break;
                            case "ratEnemy":
                                PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * cheeseVar));
                                hit.transform.GetComponent<EnemyMovement>().LoseHealth(damageVar);
                                nuke = true;
                                break;
                            case "watermelon":
                                PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * cheeseVar));
                                hit.transform.GetComponent<EnemyMovement>().LoseHealth(damageVar);
                                break;
                        }
                             
                        break;
                    }
                }

                if(hitEnemy && ricochet)
                {
                    GameObject shot = Instantiate(Resources.Load<GameObject>("prefabs/day3/rBullet"));
                    shot.transform.position = new Vector3(hit.transform.position.x, 0.8f, hit.transform.position.z);
                    rayRot = Vector3.Reflect(rayRot, hit.normal)*180/Mathf.PI;
                    rayRot = new Vector3(0, rayRot.y, rayRot.z);
                    shot.transform.eulerAngles = rayRot;
                    Raypos = hitPos;

                    print(rayRot);
                } else
                {
                    break;
                }
            }
            
        } else {
            anim.SetBool("shooting", false);
        }

        if(Input.GetKeyDown(KeyCode.Equals))
        {
            PlayerPrefs.SetInt("flag", (PlayerPrefs.GetInt("flag") < 253) ? (PlayerPrefs.GetInt("flag") + 1) : 0);
            setFlag();
        } else if(Input.GetKeyDown(KeyCode.Minus))
        {
            PlayerPrefs.SetInt("flag", (PlayerPrefs.GetInt("flag") > 0) ? (PlayerPrefs.GetInt("flag") - 1) : 253);
            setFlag();
        }
    }

    void setFlag()
    {
        Resources.Load<Material>("materials/day0/australia").SetTexture(Shader.PropertyToID("_Image"), flags[PlayerPrefs.GetInt("flag")].texture);
    }

    IEnumerator Nuke()
    {
        yield return new WaitForSeconds(6.5f);
        GameObject.Find("nuke").transform.GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt("nuked", 1);
        SceneManager.LoadScene("shop");
    }

    IEnumerator btpfp()
    {
        GameObject.Find("btPfp").transform.GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("btPfp").transform.GetComponent<Image>().enabled = false;
    }
} 
