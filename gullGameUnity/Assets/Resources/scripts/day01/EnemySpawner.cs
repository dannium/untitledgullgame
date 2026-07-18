using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class EnemySpawner : MonoBehaviour
{
    GameObject slimePrefab;
    GameObject butcherPrefab;
    GameObject ponyPrefab;
    GameObject darkSoulsPrefab;
    GameObject orbPrefab;
    GameObject ratPrefab;
    GameObject watermelonPrefab;
    GameObject purpleGuyPrefab;
    GameObject karlsonPrefab;
    GameObject btPrefab;
    GameObject jesterPrefab;
    GameObject octoPrefab;
    GameObject mirrorGeorgePrefab;
    [SerializeField] Material[] faces;
    [SerializeField] Material Invis;
    [SerializeField] Transform invSlot;
    [SerializeField] Transform waves;
    public float sugChance;
    float cd = 0;
    float nextWavecd = 100;
    int wave = 1;
    int enemiesLeft;
    int spawnsLeft;
    Transform enemies;
    [SerializeField] health health;

    bool trioAlive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetInt("mode") == 4 || PlayerPrefs.GetInt("mode") == 2)
        {
            transform.GetComponent<EnemySpawner>().enabled = false;
        }

        PlayerPrefs.SetInt("wave", 0);
        trioAlive = false;
        enemiesLeft = 4;
        spawnsLeft = 4;
        slimePrefab = Resources.Load<GameObject>("prefabs/day1/slimePrefab");
        butcherPrefab = Resources.Load<GameObject>("prefabs/day2/butcherPrefab");
        ponyPrefab = Resources.Load<GameObject>("prefabs/day2/pj");
        darkSoulsPrefab = Resources.Load<GameObject>("prefabs/day3/darkSoulsPrefab");
        orbPrefab = Resources.Load<GameObject>("prefabs/day3/orb");
        ratPrefab = Resources.Load<GameObject>("prefabs/day3/ratEnemyPrefab");
        watermelonPrefab = Resources.Load<GameObject>("prefabs/day3/watermelon");
        purpleGuyPrefab = Resources.Load<GameObject>("prefabs/day3/purpleGuyPrefab");
        karlsonPrefab = Resources.Load<GameObject>("prefabs/day3/karlsonPrefab");
        btPrefab = Resources.Load<GameObject>("prefabs/day3/btPrefab");
        jesterPrefab = Resources.Load<GameObject>("prefabs/day3/jesterPrefab");
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
        if(Input.GetKeyDown(KeyCode.Q) && health.getHealth() > 0 && PlayerPrefs.GetInt("steak") > 0)
        {
            PlayerPrefs.SetInt("steak", PlayerPrefs.GetInt("steak") - 1);
            health.setHealth(health.getHealth() + 10);
            Material[] Meshes = new Material[2];
            Meshes[0] = faces[0];
            Meshes[1] = Invis;
            NewSlime(Meshes, 4, 4, 1);
            enemiesLeft++;

            //PlayerPrefs.SetInt("steak", Mathf.Max(PlayerPrefs.GetInt("steak") - 1, 0));
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
            //print("wave timeout");
            NextWave();
        }

        if (enemiesLeft != 0)
        {
            cd -= Time.deltaTime;
        }

        if(cd <= 0 && spawnsLeft > 0) {
            spawnsLeft--;
            //print("spawnsLeft = " + spawnsLeft);
            cd = Random.Range(0.75f, 2.00f);

            if(Random.Range(1, 100) <= sugChance)
            {
                //spawn boss
                if(spawnsLeft == 0 && Random.Range(1, 100) <= 67)
                {
                    int boss = Random.Range(0, 5);
                    if(boss == 4)
                    {
                        boss = Random.Range(0, 5);
                    }
                    //print("boss spawning, index = " +  boss);
                    switch(boss)
                    {
                        case 0:
                            //butcher
                            //print("butcher spawned");
                            GameObject realButcher = Instantiate(butcherPrefab);
                            realButcher.transform.parent = enemies;
                            realButcher.name = "realButcher";
                            GameObject fakeButcher = Instantiate(butcherPrefab);
                            fakeButcher.transform.parent = enemies;
                            fakeButcher.GetComponentInChildren<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("prefabs/day2/butcherTPOSE");
                            fakeButcher.GetComponent<butcherMovement>().fake = true;
                            break;
                        case 1:
                            //print("trio spawned");
                            //3 birds
                            enemiesLeft += 3;

                            waves.GetComponentsInChildren<TextMeshProUGUI>()[1].text = enemiesLeft + " enemies left";

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
                            //dark souls
                            //print("darksouls spawned");
                            float angle = Random.Range(0, 2 * Mathf.PI);
                            GameObject ds = Instantiate(darkSoulsPrefab);
                            ds.GetComponent<EnemyMovement>().angle = angle;
                            ds.name = "DarkSouls";
                            ds.transform.parent = enemies;
                            break;
                        case 3:
                            //jester
                            //print("jester spawned");
                            angle = Random.Range(0, 2 * Mathf.PI);
                            GameObject jester = Instantiate(jesterPrefab);
                            jester.name = "jester";
                            jester.GetComponent<EnemyMovement>().angle = angle;
                            jester.transform.parent = enemies;
                            break;
                        case 4:
                            //mirror george
                            //print("mirror spawned");
                            angle = Random.Range(0, 2 * Mathf.PI);
                            GameObject mirror = Instantiate(mirrorGeorgePrefab);
                            mirror.GetComponent<EnemyMovement>().angle = angle;
                            mirror.transform.parent = enemies;
                            break;
                    }

                }
                else {
                    //non-boss suggestions
                    int faceNum = Random.Range(2, faces.Length+6);
                    if(faceNum == faces.Length + 1)
                    {
                        faceNum = Random.Range(2, faces.Length + 6);
                    }
                    //print("non-boss at index " + faceNum);
                    if (faceNum == faces.Length)
                    {
                        //pony
                        //print("pony spawned");
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
                    else if (faceNum == faces.Length + 1)
                    {
                        //rat
                        //print("rat spawned");
                        float angle = Random.Range(0, 2 * Mathf.PI);
                        GameObject newRat = Instantiate(ratPrefab);
                        newRat.transform.parent = enemies;
                        newRat.GetComponent<EnemyMovement>().angle = angle;
                    }
                    else if (faceNum == faces.Length + 2)
                    {
                        //heal orb
                        //print("heal orb spawned");
                        float angle = Random.Range(0, 2 * Mathf.PI);
                        GameObject newOrb = Instantiate(orbPrefab);
                        newOrb.transform.parent = enemies;
                        newOrb.transform.position = new Vector3(24.2f * Mathf.Cos(angle), 0.67f, 24.2f * Mathf.Sin(angle));
                        newOrb.transform.LookAt(Vector3.zero);
                        newOrb.transform.eulerAngles -= new Vector3(0, 90, 0);
                        orbMovement sm = newOrb.GetComponent<orbMovement>();
                        sm.angle = angle;
                        sm.SetValue("health", 1);
                        sm.SetValue("scale", 2);
                        sm.SetValue("speed", 1);
                    } else if (faceNum == faces.Length + 3)
                    {
                        //watermelon
                        //print("watermelon spawned");
                        float angle = Random.Range(0f, 2 * Mathf.PI);
                        GameObject wmln = Instantiate(watermelonPrefab);
                        wmln.transform.parent = enemies;
                        wmln.transform.position = new Vector3(24.2f * Mathf.Cos(angle), 0.67f, 24.2f * Mathf.Sin(angle));
                        wmln.transform.LookAt(Vector3.zero);
                        EnemyMovement sm = wmln.GetComponent<EnemyMovement>();
                        sm.angle = angle;
                        sm.stage = 1;
                    } else if(faceNum == faces.Length + 4)
                    {
                        //karlson
                        //print("karlson spawned");
                        float angle = Random.Range(0, 2 * Mathf.PI);
                        GameObject karlson = Instantiate(karlsonPrefab);
                        karlson.transform.parent = enemies;

                        EnemyMovement sm = karlson.GetComponent<EnemyMovement>();
                        sm.angle = angle;

                    } else if(faceNum == faces.Length +5){

                        //bt
                        //print("bt spawned");
                        float angle = Random.Range(0, 2 * Mathf.PI);
                        GameObject bt = Instantiate(btPrefab);
                        bt.transform.parent = enemies;

                        EnemyMovement sm = bt.GetComponent<EnemyMovement>();
                        sm.angle = angle;
                    } else if(faceNum == faces.Length + 6)
                    {
                        //octo
                        //print("octo spawned");
                        float angle = Random.Range(0, 2 * Mathf.PI);
                        GameObject octo = Instantiate(octoPrefab);
                        octo.transform.parent = enemies;

                        EnemyMovement sm = octo.GetComponent<EnemyMovement>();
                        sm.angle = angle;
                    }
                    else
                    {
                        Material[] Meshes = new Material[2];
                        Meshes[0] = faces[faceNum-1];
                        //print(Meshes[0].name + " slime spawned");
                        Meshes[1] = Invis;
                        NewSlime(Meshes, 1, 1, 3);
                    }
                }
            } else
            {
                //spawn reuglar slime
                //print("normal slime spawned");
                NewSlime(null, 1, 1, 3);
            }

        }
    }

    void NewSlime(Material[] Meshes, int hp, float scale, float speed)
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
        if(Meshes != null)
        {
            newSlime.GetComponentInChildren<SkinnedMeshRenderer>().materials = Meshes;
            newSlime.name = Meshes[0].name;
        }

    }

    public void LoseEnemy()
    {
        enemiesLeft--;
        //print("enemies left--, = " + enemiesLeft);
        if(enemiesLeft == 0)
        {
            NextWave();
        }

        else if(enemiesLeft == 1 && trioAlive)
        {
            print("trio boss spawned");
            GameObject trioBoss = Instantiate(Resources.Load<GameObject>("prefabs/day2/3birdsBoss"));
            trioBoss.name = "trioBoss";
            trioBoss.transform.SetParent(GameObject.Find("trio").transform);
            trioAlive = false;
            health.qInc("boss");
        }

        waves.GetComponentsInChildren<TextMeshProUGUI>()[1].text = enemiesLeft + " enemies left";
    }

    public void NextWave()
    {
        //print("next wave");
        trioAlive = false;
        cd = 4;
        nextWavecd = 100;
        wave++;
        PlayerPrefs.SetInt("wave", wave);
        health.qInc("wave");

        if (wave == 50 || wave == 100)
        {
            float angle = transform.eulerAngles.y * Mathf.Deg2Rad;
            GameObject pg = Instantiate(purpleGuyPrefab);
            print("yurt");
            pg.transform.position = new Vector3(-12 * Mathf.Cos(angle + (Mathf.PI / 2)), 12, 12 * Mathf.Sin(angle + (Mathf.PI / 2)));
        }

        waves.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Wave " + wave;
        enemiesLeft = 2 * wave + Random.Range(0, 3);
        spawnsLeft = enemiesLeft;
        waves.GetComponentsInChildren<TextMeshProUGUI>()[1].text = enemiesLeft + " enemies left";
    }

    public int getWave()
    {
        return wave;
    }

}
