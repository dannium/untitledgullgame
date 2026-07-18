using Unity.VisualScripting;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public float angle;
    float speed = 3f;
    poof poof;
    health healthScript;
    healthNoSug healthScriptNoSug;
    float hp = 1;
    EnemySpawner es;
    AnimalSpawner ans;
    EnemySpawnerNoSug esns;
    bool walkAway;
    bool startWalking;
    bool noSug;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        noSug = PlayerPrefs.GetInt("mode") == 1;
        poof = GameObject.Find("scriptRunner").GetComponent<poof>();
        if(!noSug)
        {
            healthScript = GameObject.FindGameObjectWithTag("gui").GetComponent<health>();
            es = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<EnemySpawner>();
            ans = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<AnimalSpawner>();
        } else
        {
            healthScriptNoSug = GameObject.FindGameObjectWithTag("gui").GetComponent<healthNoSug>();
            esns = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<EnemySpawnerNoSug>();
        }



        walkAway = (Random.Range(1, 200) == 67) ? true : false;
        startWalking = false;
        speed += (float)PlayerPrefs.GetInt("wave") / 10f;
        if(walkAway)
        {
            print("slime will walk away");
        }
        //print("angle (rads): " + angle);
    }

    // Update is called once per frame
    void Update()
    {
        if (startWalking)
        {
            transform.position += Time.deltaTime * new Vector3(speed * Mathf.Cos(angle), 0, speed * Mathf.Sin(angle));
            Quaternion lookRot = Quaternion.LookRotation(transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }
        else
        {
            transform.position -= Time.deltaTime * new Vector3(speed * Mathf.Cos(angle), 0, speed * Mathf.Sin(angle));
        }
        if (Mathf.Abs(transform.position.x) + Mathf.Abs(transform.position.z) < 2.5f) {
            if(noSug)
            {
                healthScriptNoSug.setHealth(healthScriptNoSug.getHealth() - 10);
                esns.LoseEnemy();
            } else
            {
                healthScript.setHealth(healthScript.getHealth() - 10);
                es.LoseEnemy();
            }
            poof.Poofs(transform.position, 1);
            ///print("slime hit castle");
            Destroy(transform.gameObject);
        }

        if (Mathf.Abs(transform.position.x) + Mathf.Abs(transform.position.z) < 4f && walkAway)
        {
            startWalking = true;
        } else if (Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.z, 2) > 25*25)
        {
            print("out of bounds (" + transform.position.x + ", " + transform.position.z + ")");
            LoseHealth(999);
        }

      }

    public void LoseHealth(float damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            poof.Poofs(transform.position, 1);
            if (PlayerPrefs.GetInt("mode") == 2)
            {
                ans.LoseEnemy();
                healthScript.ratAppear();
                healthScript.qInc("animal");
            }
            else if(PlayerPrefs.GetInt("mode") == 1)
            {
                GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<EnemySpawnerNoSug>().LoseEnemy();
            } else
            {
                es.LoseEnemy();
                healthScript.ratAppear();
            }

            if(name == "hm 1")
            {
                healthScript.qInc("animal");
            } else if(name == "wht")
            {
                healthScript.qInc("wht");
            }

            Destroy(transform.gameObject);  
        } else
        {
            poof.Poofs(transform.position, 0.5f);
        }

    }
    public void SetValue(string type, float newValue)
    {
        if(type == "speed")
        {
            speed = newValue;
        } else if(type == "health")
        {
            hp = newValue;
        } else if(type == "scale")
        {
            transform.localScale = Vector3.one * newValue / 2;
        }
    }
}
