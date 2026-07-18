//using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using URPGlitch;
public class trioMovement : MonoBehaviour
{
    public int birdType;
    float angle;
    poof poof;
    health healthScript;
    EnemySpawner es;
    AnimalSpawner ans;
    float speed;
    float hp;
    public float poofScale;
    int animFrame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed += (float)PlayerPrefs.GetInt("wave") / 10f;

        switch (birdType)
        {
            //small
            case 0:
                hp = 1;
                speed = 3;
                poofScale = 0.75f;
                transform.position = new Vector3(0, 0.44f, 0);
                GetComponent<Animator>().speed = 2;
            break;
            //big
            case 1:
                hp = 4;
                speed = 1.75f;
                poofScale = 1.5f;
                transform.position = new Vector3(0, 1.1f, 0);
                GetComponent<Animator>().speed = 1;
            break;
            //long
            case 2:
                hp = 2;
                speed = 2;
                poofScale = 1.5f;
                transform.position = new Vector3(0, 0.67f, 0);
                GetComponent<Animator>().speed = 1.5f;
            break;
            //boss
            case 3:
                hp = 8;
                speed = 1.4f;
                poofScale = 2.5f;
                transform.position = new Vector3(0, 1.81f, 0);
                GetComponent<Animator>().speed = 0.75f;
            break;
        }

        angle = Random.Range(0, 360);
        poof = GameObject.Find("scriptRunner").GetComponent<poof>();
        healthScript = GameObject.Find("mainGUI").GetComponent<health>();
        es = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<EnemySpawner>();
        ans = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<AnimalSpawner>();

        transform.position = new Vector3(24.2f*Mathf.Cos(angle), transform.position.y, 24.2f*Mathf.Sin(angle));
        transform.LookAt(Vector3.zero);
        transform.eulerAngles += new Vector3(0, 90, 0);
    }

    // Update is called once per frame
    void Update()
    {   
        if(Mathf.Abs(transform.position.x) + Mathf.Abs(transform.position.z) < 2.5f) {
            if(birdType == 3)
            {
                healthScript.setHealth(healthScript.getHealth()-50);
            } else
            {
                healthScript.setHealth(healthScript.getHealth()-20);
            }
            poof.Poofs(transform.position, poofScale);

            foreach(Transform t in transform.parent)
            {
                poof.Poofs(t.position, t.GetComponent<trioMovement>().poofScale);
                if (PlayerPrefs.GetInt("mode") == 2)
                {
                    ans.LoseEnemy();
                }
                else
                {
                    es.LoseEnemy();
                }
            }
            Destroy(transform.parent.gameObject);
        }
        transform.position -= Time.deltaTime * new Vector3(speed*Mathf.Cos(angle-0), 0, speed*Mathf.Sin(angle-0));
        animFrame = Mathf.RoundToInt(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime * 81) % 81;
        // if animation reaches frame when step occurs
        if(animFrame == 10 || animFrame == 50)
        {
            GetComponent<AudioSource>().Play();
        }
    }


    public void LoseHealth(float damage)
    {
        hp -= damage;
        if(hp <= 0) {
            poof.Poofs(transform.position, poofScale);
            if (PlayerPrefs.GetInt("mode") == 2)
            {
                ans.LoseEnemy();
            }
            else
            {
                es.LoseEnemy();
            }
            healthScript.ratAppear();
            healthScript.qInc("animal");
            Destroy(transform.gameObject);  
        } else
        {
            poof.Poofs(transform.position, poofScale/2);
        }
    }
}
