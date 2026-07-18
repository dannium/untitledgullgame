using UnityEngine;

public class bombMovement : MonoBehaviour
{
    public float angle;
    poof poofs;
    health health;
    float dist;
    float speed = 1;
    float startDist;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed += (float)PlayerPrefs.GetInt("wave") / 30f;
        health = GameObject.FindGameObjectWithTag("gui").GetComponent<health>();
        poofs = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<poof>();
        startDist = dist = Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.z, 2));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= Time.deltaTime * new Vector3(10 * Mathf.Cos(angle) * speed, 0, 10 * Mathf.Sin(angle)*speed);
        dist = Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.z, 2));
        transform.position = new Vector3(transform.position.x, height(dist), transform.position.z);
        if(dist < 1f)
        {
            poofs.Poofs(transform.position, 1);
            health.setHealth(health.getHealth() - 5);
            Destroy(transform.gameObject);
        }
    }


    float height(float x)
    {
        float test = (-20 * Mathf.Pow(startDist, -2)) * Mathf.Pow(x, 2) + (20 / startDist) * x + 2.5f;
        return test;
    }
}
