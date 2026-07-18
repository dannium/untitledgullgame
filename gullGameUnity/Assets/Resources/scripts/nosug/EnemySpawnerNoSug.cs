using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class EnemySpawnerNoSug : MonoBehaviour
{
    GameObject slimePrefab;
    [SerializeField] Transform waves;
    float cd = 0;
    float nextWavecd = 100;
    int wave = 1;
    int enemiesLeft;
    int spawnsLeft;
    Transform enemies;
    [SerializeField] health health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        PlayerPrefs.SetInt("wave", 0);
        enemiesLeft = 4;
        spawnsLeft = 4;
        slimePrefab = Resources.Load<GameObject>("prefabs/day1/slimePrefab");
        enemies = GameObject.FindGameObjectWithTag("enemies").transform;
    }

    void Update() {

        nextWavecd -= Time.deltaTime;
        if(nextWavecd < 0)
        {
            print("wave timeout");
            NextWave();
        }

        if (enemiesLeft != 0)
        {
            cd -= Time.deltaTime;
        }

        if(cd <= 0 && spawnsLeft > 0) {
            spawnsLeft--;
            print("spawnsLeft = " + spawnsLeft);
            cd = Random.Range(0.75f, 2.00f);
            //spawn reuglar slime
            print("normal slime spawned");
            NewSlime(null, 1, 1, 3);
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
        }

    }

    public void LoseEnemy()
    {
        enemiesLeft--;
        print("enemies left--, = " + enemiesLeft);
        if(enemiesLeft == 0)
        {
            NextWave();
        }

        waves.GetComponentsInChildren<TextMeshProUGUI>()[1].text = enemiesLeft + " enemies left";
    }

    public void NextWave()
    {
        print("next wave");
        cd = 4;
        nextWavecd = 100;
        wave++;
        PlayerPrefs.SetInt("wave", wave);

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
