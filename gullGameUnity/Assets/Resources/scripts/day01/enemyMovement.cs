using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovement : MonoBehaviour
{
    public float angle;
    public float speed = 3f;
    poof poof;
    health healthScript;
    [SerializeField] float hp = 1;
    EnemySpawner es;
    RoyaleSpawner rs;
    AnimalSpawner ans;
    [SerializeField] float startOffset;
    [SerializeField] byte yrotOffset;
    [SerializeField] float xrotOffset;
    [SerializeField] bool jester;
    [SerializeField] bool octo;
    [SerializeField] bool mirror;
    public byte stage;
    public int angOffset;
    bool isRoyale;
    float royaleY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        speed += (float)PlayerPrefs.GetInt("wave") / 10f;
        //minesweeper texture
        if (hp == 7 && Random.Range(1,2) == 1)
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("materials/day3/minesweeper");
        }

        poof = GameObject.Find("scriptRunner").GetComponent<poof>();
        healthScript = GameObject.FindGameObjectWithTag("gui").GetComponent<health>();
        if (PlayerPrefs.GetInt("mode") == 4)
        {
            rs = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<RoyaleSpawner>();
        } else if(PlayerPrefs.GetInt("mode") == 2)
        {
            ans = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<AnimalSpawner>();
        }
        else
        {
            es = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<EnemySpawner>();
        }
        transform.position = new Vector3(23 * Mathf.Cos(angle), 2.4f + startOffset, 23 * Mathf.Sin(angle));
        transform.LookAt(Vector3.zero);
        transform.eulerAngles += new Vector3(xrotOffset, yrotOffset, 0);

        if (PlayerPrefs.GetInt("mode") == 4)
        {
            transform.eulerAngles += new Vector3(90, 0, 0);
            isRoyale = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoyale)
        {
            royaleY = Mathf.Sin(Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.z, 2)));
        }
        transform.position -= Time.deltaTime * new Vector3(speed*Mathf.Cos(angle+(angOffset*Mathf.PI/180)), royaleY, speed*Mathf.Sin(angle + (angOffset * Mathf.PI / 180)));
        if(Mathf.Abs(transform.position.x) + Mathf.Abs(transform.position.z) < 2.5f) {
            healthScript.setHealth(healthScript.getHealth()-10);
            poof.Poofs(new Vector3(transform.position.x, 2.4f + startOffset, transform.position.z), 2);
            if (transform.gameObject.tag == "watermelon" && stage < 3)
            {
                GameObject watermelon = Instantiate(Resources.Load<GameObject>("prefabs/day3/watermelon"));
                watermelon.transform.parent = GameObject.Find("enemies").transform;
                watermelon.GetComponent<EnemyMovement>().stage = (byte)(stage + 1);

            } else
            {
                if (PlayerPrefs.GetInt("mode") == 4)
                {
                    rs.LoseEnemy();
                }
                else if (PlayerPrefs.GetInt("mode") == 2)
                {
                    ans.LoseEnemy();
                }
                else
                {
                    es.LoseEnemy();
                }
            }
            Destroy(transform.gameObject);
        }
    }

    public void LoseHealth(float damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            poof.Poofs(new Vector3(transform.position.x, 2.4f + startOffset, transform.position.z), 2);
            if(jester || name == "DarkSouls" || name == "Minesweeper")
            {
                healthScript.qInc("boss");
            }


            if (stage != 0 && stage < 3)
            {
                //print("newone");
                GameObject watermelon = Instantiate(Resources.Load<GameObject>("prefabs/day3/watermelon"));
                watermelon.transform.parent = GameObject.Find("enemies").transform;
                watermelon.GetComponent<EnemyMovement>().stage = (byte)(stage + 1);

            }
            else
            {
                if (PlayerPrefs.GetInt("mode") == 4)
                {
                    rs.LoseEnemy();
                }
                else if (PlayerPrefs.GetInt("mode") == 2)
                { 
                    ans.LoseEnemy();
                } 
                else
                {
                    es.LoseEnemy();
                }
            }
            healthScript.ratAppear();
            if (octo)
            {
                healthScript.showClone();
                healthScript.qInc("animal");
            }
            else if (mirror)
            {
                SceneManager.LoadScene("mirror");
                healthScript.qInc("animal");
            } else if(transform.tag == "ratEnemy" || transform.name == "pj(Clone)")
            {
                healthScript.qInc("animal");
            }
            Destroy(transform.gameObject);  
            
        } else
        {
            poof.Poofs(new Vector3(transform.position.x, 2.4f + startOffset, transform.position.z), 1.5f);
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
