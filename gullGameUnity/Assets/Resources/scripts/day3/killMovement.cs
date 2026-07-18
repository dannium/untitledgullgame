using UnityEngine;

public class killMovement : MonoBehaviour
{
    public float angle;
    private static string[] tagList = new string[] { "enemy", "butcher", "trio", "orb", "enemy2", "ratEnemy", "watermelon" };
    movement m;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        angle *= Mathf.Deg2Rad;
        m = GameObject.Find("plr").GetComponent<movement>();
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(-5 * Mathf.Cos(angle), 0, 5 * Mathf.Sin(angle));
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime);
        transform.LookAt(Vector3.zero);
        transform.eulerAngles += Vector3.up * 90;

        if(transform.position.magnitude > 30)
        {
            Destroy(transform.gameObject);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        foreach (string item in tagList)
        {
            if (hit.GetComponent<Collider>().tag == item)
            {
                m.KillCount++;
                m.KillCountTxt.text = "" + m.KillCount;
                print(hit.GetComponent<Collider>().tag);

                switch (item)
                {
                    case "enemy":
                        print("enemyhit");
                        PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * 3));
                        hit.transform.GetComponent<SlimeMovement>().LoseHealth(2);
                        break;
                    case "butcher":
                        PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * 3));
                        hit.transform.GetComponent<butcherMovement>().LoseHealth();
                        break;
                    case "trio":
                        PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * 3));
                        hit.transform.GetComponent<trioMovement>().LoseHealth(2);
                        break;
                    case "orb":
                        PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * 3));
                        hit.transform.GetComponent<orbMovement>().LoseHealth(2);
                        break;
                    case "enemy2":
                        PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * 3));
                        hit.transform.GetComponent<EnemyMovement>().LoseHealth(2);
                        break;
                    case "ratEnemy":
                        PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * 3));
                        hit.transform.GetComponent<EnemyMovement>().LoseHealth(2);
                        break;
                    case "watermelon":
                        PlayerPrefs.SetInt("cheeseTokensNew", PlayerPrefs.GetInt("cheeseTokensNew") + Mathf.RoundToInt(3 * 3));
                        hit.transform.GetComponent<EnemyMovement>().LoseHealth(2);
                        break;
                }

                break;
            }
        }
    }
}
