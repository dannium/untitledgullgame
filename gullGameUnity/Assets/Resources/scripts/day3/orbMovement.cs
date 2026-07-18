using UnityEngine;

public class orbMovement : MonoBehaviour
{
    public float angle;
    float speed = 6f;
    poof poof;
    health healthScript;
    float hp = 1;
    EnemySpawner es;
    bool walkAway;
    bool startWalking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed += (float)PlayerPrefs.GetInt("wave") / 5f;
        poof = GameObject.Find("scriptRunner").GetComponent<poof>();
        healthScript = GameObject.FindGameObjectWithTag("gui").GetComponent<health>();
        es = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<EnemySpawner>();
        walkAway = (Random.Range(1, 200) == 67) ? true : false;
        startWalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startWalking)
        {
            transform.position += Time.deltaTime * new Vector3(speed * Mathf.Cos(angle), 0, speed * Mathf.Sin(angle));
            Quaternion lookRot = Quaternion.LookRotation(transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
            print(transform.rotation);
        }
        else
        {
            transform.position -= Time.deltaTime * new Vector3(speed * Mathf.Cos(angle), 0, speed * Mathf.Sin(angle));
        }

        if (Mathf.Abs(transform.position.x) + Mathf.Abs(transform.position.z) < 2.5f)
        {
            poof.Poofs(transform.position, 1);
            es.LoseEnemy();
            LoseHealth(999);
            Destroy(transform.gameObject);
        } else if (Mathf.Abs(transform.position.x) + Mathf.Abs(transform.position.z) < 7.5f && walkAway)
        {
            startWalking = true;
        }
    }

    public void LoseHealth(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            healthScript.setHealth(healthScript.getHealth() + 10);
            poof.Poofs(transform.position, 2);
            es.LoseEnemy();
            Destroy(transform.gameObject);
        }
        else
        {
            poof.Poofs(transform.position, 1);

        }

    }
    public void SetValue(string type, float newValue)
    {
        if (type == "speed")
        {
            speed = newValue;
        }
        else if (type == "health")
        {
            hp = newValue;
        }
        else if (type == "scale")
        {
            transform.localScale = Vector3.one * newValue / 2;
        }
    }
}
