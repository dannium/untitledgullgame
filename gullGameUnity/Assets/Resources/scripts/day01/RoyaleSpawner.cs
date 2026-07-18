using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoyaleSpawner : MonoBehaviour
{
    GameObject royalePrefab;
    [SerializeField] Material[] faces;
    [SerializeField] Material Invis;
    [SerializeField] Transform invSlot;
    [SerializeField] Transform waves;
    public float sugChance;
    float cd = 0;
    int wave = 1;
    int enemiesLeft;
    int spawnsLeft;
    Transform enemies;
    [SerializeField] health health;
    float waveCd = 30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if(PlayerPrefs.GetInt("mode") != 4)
        {
            transform.GetComponent<RoyaleSpawner>().enabled = false;
        }
        enemiesLeft = 4;
        spawnsLeft = 4;
        royalePrefab = Resources.Load<GameObject>("prefabs/day3/royale");
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

        waveCd -= Time.deltaTime;
        if(waveCd < 0)
        {
            NextWave();
        }

        if(Input.GetKeyDown(KeyCode.Q) && PlayerPrefs.GetInt("steak") > 0)
        {
            health.setHealth(health.getHealth() + 10);
            Material[] Meshes = new Material[2];
            Meshes[0] = faces[0];
            Meshes[1] = Invis;
            spawnRoyale(3);
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
        if(enemiesLeft != 0)
        {
            cd -= Time.deltaTime;
        }
        if(cd <= 0 && spawnsLeft > 0) {
            spawnsLeft--;
            cd = Random.Range(0.75f, 2.00f);

            spawnRoyale(1);

        }
    }

    void spawnRoyale(float size)
    {
        float angle = Random.Range(0, 360);
        GameObject newRoyale = Instantiate(royalePrefab);
        newRoyale.GetComponent<EnemyMovement>().angle = angle;
        newRoyale.transform.parent = enemies;
        newRoyale.transform.localScale = Vector3.one*0.4f*size;
        SlimeMovement sm = newRoyale.GetComponent<SlimeMovement>();
    }

    public void LoseEnemy()
    {
        enemiesLeft--;
        if(enemiesLeft <= 0)
        {
            NextWave();
        }
        waves.GetComponentsInChildren<TextMeshProUGUI>()[1].text = enemiesLeft + " enemies left";
    }

    public void NextWave()
    {
        waveCd = 30;
        cd = 4;
        wave++;
        health.qInc("wave");
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
