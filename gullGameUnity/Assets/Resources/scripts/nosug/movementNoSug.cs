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

public class movementNoSug : MonoBehaviour
{
    [SerializeField] screenShake ss;
    Animator anim;
    GameObject bullet;

    float currentTurn = 0;
    float cd = 0;
    Vector3 Raypos;
    RaycastHit hit;
    public float KillCount;
    public TextMeshProUGUI KillCountTxt;
    GameObject plr;
    Transform enemies;
    health health;
    EnemySpawner es;
    poof poof;

    private static float[][] upgradeAmounts = new float[][] {
    new float[] {1,0.8f,0.6f,0.5f,0.4f,0.3f,0.2f,0.15f,0.1f,0.05f,0f}, // reload time
    new float[] {1,1.2f,1.4f,1.6f,1.8f,2f,2.1f,2.2f,2.3f,2.4f,2.5f}, // rot speed
    new float[] {1,1.2f,1.4f,1.6f,1.8f,2f,2.25f,2.5f,2.75f,3f,4f}, // damage
    new float[] {1,1.1f,1.2f,1.4f,1.6f,1.8f,2f,2.25f,2.5f,2.75f,3}  // cheese tokens
        };


    float reloadVar;
    float rotationVar;
    float damageVar;
    float cheeseVar;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        plr = GameObject.FindGameObjectWithTag("plr");

        GameObject.Find("GUN").GetComponent<SkinnedMeshRenderer>().enabled = true;



        reloadVar = upgradeAmounts[0][PlayerPrefs.GetInt("reloadVar", 1)];
        rotationVar = upgradeAmounts[1][PlayerPrefs.GetInt("rotationVar", 1)];
        damageVar = upgradeAmounts[2][PlayerPrefs.GetInt("damageVar", 1)];
        cheeseVar = upgradeAmounts[3][PlayerPrefs.GetInt("cheeseVar", 1)];

        KillCount = 0;
        KillCountTxt = GameObject.Find("KillNum").GetComponent<TextMeshProUGUI>();
        health = GameObject.FindGameObjectWithTag("gui").GetComponent<health>();
        es = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<EnemySpawner>();
        poof = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<poof>();
        bullet = GameObject.Find("bullet");
        Time.timeScale = 1;
        anim = GetComponentInChildren<Animator>();
        Raypos = (transform.position + transform.forward * 2);
        enemies = GameObject.FindGameObjectWithTag("enemies").transform;
        //inRound = true;
        //hScoreText.text = "High Score: " + PlayerPrefs.GetInt("hScore");

    }

    // Update is called once per frame
    void Update()
    {

        currentTurn = Mathf.Lerp(currentTurn, Time.deltaTime * 100 * Input.GetAxisRaw("Horizontal") * rotationVar, Time.deltaTime * 8);
        transform.Rotate(0f, currentTurn, 0f);
        cd -= Time.deltaTime;

        if (cd < reloadVar * 0.9)
        {
            bullet.GetComponent<MeshRenderer>().enabled = false;
        }

        if (cd <= 0 && (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)))
        {

            anim.SetBool("shooting", true);
            cd = reloadVar;

            ss.ScreenShake(15, 15);
            bullet.GetComponent<AudioSource>().pitch = Random.Range(0.900f, 1.100f);
            bullet.GetComponent<AudioSource>().Play();

            Raypos = (transform.position + transform.forward * 2);
            Raypos = new Vector3(Raypos.x, 0.25f, Raypos.z);
            RaycastHit hit;
            bullet.GetComponent<MeshRenderer>().enabled = true;
            Vector3 rayRot = transform.forward;

            if (Physics.Raycast(Raypos, rayRot, out hit, 100))
            {
                Vector3 hitPos = hit.point;
                hit.transform.GetComponent<SlimeMovement>().LoseHealth(damageVar);

            }
            
        }
        else
        {
            anim.SetBool("shooting", false);
        }


    }
}