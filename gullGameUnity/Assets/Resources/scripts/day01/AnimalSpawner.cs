using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalSpawner : MonoBehaviour
{
    GameObject slimePrefab;
    GameObject butcherPrefab;
    GameObject ponyPrefab;
    GameObject darkSoulsPrefab;
    GameObject orbPrefab;
    GameObject ratPrefab;
    GameObject octoPrefab;
    GameObject mirrorGeorgePrefab;
    [SerializeField] Material Invis;
    [SerializeField] Transform invSlot;
    [SerializeField] Transform waves;
    float cd = 0;
    int wave = 1;
    int enemiesLeft;
    int spawnsLeft;
    Transform enemies;
    [SerializeField] health health;
    float nextWavecd = 30;
    bool trioAlive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetInt("mode") != 2)
        {
            transform.GetComponent<AnimalSpawner>().enabled = false;
        }

        trioAlive = false;
        enemiesLeft = 4;
        spawnsLeft = 4;
        butcherPrefab = Resources.Load<GameObject>("prefabs/day2/butcherPrefab");
        ponyPrefab = Resources.Load<GameObject>("prefabs/day2/pj");
        ratPrefab = Resources.Load<GameObject>("prefabs/day3/ratEnemyPrefab");
        slimePrefab = Resources.Load<GameObject>("prefabs/day3/animalSlimePrefab");
        octoPrefab = Resources.Load<GameObject>("prefabs/day3/octoPrefab");
        mirrorGeorgePrefab = Resources.Load<GameObject>("prefabs/day3/mirrorGeorgePrefab");

        enemies = GameObject.FindGameObjectWithTag("enemies").transform;
        
        if (PlayerPrefs.GetInt("steak", 0) <= 0)
        {
            invSlot.GetComponentInChildren<TextMeshProUGUI>().text = "";
            invSlot.GetComponentsInChildren<Image>()[1].enabled = false;
        }
        else
        {
            invSlot.GetComponentInChildren<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("steak");
        }
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Q) && PlayerPrefs.GetInt("steak") > 0)
        {
            health.setHealth(health.getHealth() + 10);
            NewSlime(4, 4, 1);
            enemiesLeft++;

            PlayerPrefs.SetInt("steak", Mathf.Max(PlayerPrefs.GetInt("steak") - 1, 0));
            if(PlayerPrefs.GetInt("steak") <= 0)
            {
                invSlot.GetComponentInChildren<TextMeshProUGUI>().text = "";
                invSlot.GetComponentsInChildren<Image>()[1].enabled = false;
            } else
            {
                invSlot.GetComponentInChildren<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("steak");
            }
        }

        nextWavecd -= Time.deltaTime;
        if(nextWavecd < 0)
        {
            NextWave();
        }

        if(enemiesLeft != 0)
        {
            cd -= Time.deltaTime;
        }
        if(cd <= 0 && spawnsLeft > 0) {
            spawnsLeft--;
            cd = Random.Range(0.75f, 2.00f);
                //spawn boss
                if(spawnsLeft == 0 && Random.Range(1, 100) <= 67)
                {
                    int boss = Random.Range(0, 3);
                    if (boss == 2)
                    {
                        boss = Random.Range(0, 3);
                        if(boss == 2)
                        {
                            boss = Random.Range(0, 3);
                        }
                    }
                
                    switch(boss)
                    {
                        case 0:
                            //butcher
                            GameObject realButcher = Instantiate(butcherPrefab);
                            realButcher.transform.parent = enemies;
                            realButcher.name = "realButcher";
                            GameObject fakeButcher = Instantiate(butcherPrefab);
                            fakeButcher.transform.parent = enemies;
                            fakeButcher.GetComponentInChildren<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("prefabs/day2/butcherTPOSE");
                            fakeButcher.GetComponent<butcherMovement>().fake = true;
                            break;
                        case 1:
                            //3 birds
                            enemiesLeft += 3;
                            trioAlive = true;
                            GameObject trioLarge = Instantiate(Resources.Load<GameObject>("prefabs/day2/3birdsLarge"));
                            trioLarge.name = "trioLarge";
                            GameObject trioSmall = Instantiate(Resources.Load<GameObject>("prefabs/day2/3birdsSmall"));
                            trioSmall.name = "trioSmall";
                            GameObject trioLong = Instantiate(Resources.Load<GameObject>("prefabs/day2/3birdsLong"));
                            trioLong.name = "trioLong";
                            GameObject trio = new GameObject();
                            trio.name = "trio";
                            trio.transform.parent = enemies;
                            trioLarge.transform.SetParent(trio.transform);
                            trioSmall.transform.SetParent(trio.transform);
                            trioLong.transform.SetParent(trio.transform);
                            break;
                        case 2:
                            //mirror george
                            float angle = Random.Range(0, 2 * Mathf.PI);
                            GameObject mirror = Instantiate(mirrorGeorgePrefab);
                            mirror.GetComponent<EnemyMovement>().angle = angle;
                            mirror.transform.parent = enemies;
                            break;
                }
                    
                } else {
                    //non-boss suggestions
                    int faceNum = Random.Range(1, 5);
                    if (faceNum == 1)
                    {
                        //pony
                        float angle = Random.Range(0, 2 * Mathf.PI);
                        GameObject newPony = Instantiate(ponyPrefab);
                        newPony.transform.parent = enemies;
                        newPony.transform.position = new Vector3(24.2f * Mathf.Cos(angle), 0, 24.2f * Mathf.Sin(angle));
                        newPony.transform.LookAt(Vector3.zero);
                        newPony.transform.eulerAngles -= new Vector3(0, 90, 0);
                        SlimeMovement sm = newPony.GetComponent<SlimeMovement>();
                        sm.angle = angle;
                        sm.SetValue("health", 1);
                        sm.SetValue("scale", 2);
                        sm.SetValue("speed", 1);
                    }
                    else if (faceNum == 2 && Random.Range(1, 20) == 7)
                    {
                        //rat
                        float angle = Random.Range(0, 2 * Mathf.PI);
                        GameObject newRat = Instantiate(ratPrefab);
                        newRat.transform.parent = enemies;
                        newRat.GetComponent<EnemyMovement>().angle = angle;
                    }
                    else if(faceNum == 3)
                    {
                        //octo
                        float angle = Random.Range(0, 2 * Mathf.PI);
                        GameObject octo = Instantiate(octoPrefab);
                        octo.transform.parent = enemies;

                        EnemyMovement sm = octo.GetComponent<EnemyMovement>();
                        sm.angle = angle;
                    }
                    else
                    {

                        NewSlime(1, 1, 3);
                    }
                }

        }
    }

    void NewSlime(int hp, float scale, float speed)
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        GameObject newSlime = Instantiate(slimePrefab);
        newSlime.transform.parent = enemies;
        newSlime.transform.position = new Vector3(24.2f*Mathf.Cos(angle), 0, 24.2f*Mathf.Sin(angle));
        newSlime.transform.LookAt(Vector3.zero);
        newSlime.transform.eulerAngles -= new Vector3(0, 90, 0);
        SlimeMovement sm = newSlime.GetComponent<SlimeMovement>();
        sm.angle = angle;
        sm.SetValue("health", hp);
        sm.SetValue("scale", scale);
        sm.SetValue("speed", speed);
    }

    public void LoseEnemy()
    {
        enemiesLeft--;
        if(enemiesLeft == 0)
        {
            NextWave();
        }

        else if(enemiesLeft == 1 && trioAlive)
        {
            GameObject trioBoss = Instantiate(Resources.Load<GameObject>("prefabs/day2/3birdsBoss"));
            trioBoss.name = "trioBoss";
            trioBoss.transform.SetParent(GameObject.Find("trio").transform);
            trioAlive = false;
        }

        waves.GetComponentsInChildren<TextMeshProUGUI>()[1].text = enemiesLeft + " enemies left";
    }

    public void NextWave()
    {

        nextWavecd = 30;
        trioAlive = false;
        cd = 4;
        wave++;
        waves.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Wave " + wave;
        enemiesLeft = 2 * wave + Random.Range(0, 3);
        spawnsLeft = enemiesLeft;
        waves.GetComponentsInChildren<TextMeshProUGUI>()[1].text = enemiesLeft + " enemies left";
    }

}
