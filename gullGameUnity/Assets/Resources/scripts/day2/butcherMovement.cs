//using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using URPGlitch;
public class butcherMovement : MonoBehaviour
{
    float angle;
    float dist;
    float tpCd = 1;
    float speed = 3;
    public bool fake = true;
    poof poof;
    health healthScript;
    EnemySpawner es;
    AnimalSpawner ans;
    int damage;
    Volume volume;
    public float glitchAmount;
    AnalogGlitchVolume agv;
    DigitalGlitchVolume dgv;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed += (float)PlayerPrefs.GetInt("wave") / 10f;

        angle = Random.Range(0, 360);
        dist = 25f;
        damage = 10;
        Teleport();
        poof = GameObject.Find("scriptRunner").GetComponent<poof>();
        healthScript = GameObject.Find("mainGUI").GetComponent<health>();
        es = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<EnemySpawner>();
        ans = GameObject.FindGameObjectWithTag("scriptRunner").GetComponent<AnimalSpawner>();

        volume = GameObject.FindGameObjectWithTag("postProcess").GetComponent<Volume>();
        volume.profile.TryGet<AnalogGlitchVolume>(out agv);
        volume.profile.TryGet<DigitalGlitchVolume>(out dgv);
    }

    // Update is called once per frame
    void Update()
    {   
        if(!fake) {
            if(glitchAmount > 0.001f)
            {
                glitchAmount = Mathf.Lerp(glitchAmount, 0, 3*Time.deltaTime);
            } else
            {
                glitchAmount = 0;
            }
            agv.scanLineJitter.value = glitchAmount * 0.3f;
            agv.verticalJump.value = glitchAmount * 0.05f;
            agv.horizontalShake.value = glitchAmount * 0.6f;
            agv.colorDrift.value = glitchAmount * 0.5f;
            dgv.intensity.value = glitchAmount * 0.2f;
        }
        tpCd -= Time.deltaTime;
        if(tpCd <= 0)
        {
            Teleport();
            tpCd = 2;
        }

        if(Mathf.Abs(transform.position.x) + Mathf.Abs(transform.position.z) < 2.5f) {
            healthScript.setHealth(healthScript.getHealth()-damage);
            poof.Poofs(transform.position, 2);
            if (PlayerPrefs.GetInt("mode") == 2)
            {
                ans.LoseEnemy();
            }
            else
            {
                es.LoseEnemy();
            }
            if (damage == 100) {
                glitchAmount = 2f;
            } else
            {
                glitchAmount = 0;
            }
            agv.scanLineJitter.value = glitchAmount * 0.3f;
            agv.verticalJump.value = glitchAmount * 0.05f;
            agv.horizontalShake.value = glitchAmount * 0.6f;
            agv.colorDrift.value = glitchAmount * 0.5f;
            dgv.intensity.value = glitchAmount * 0.2f;
            Destroy(transform.gameObject);
        }
        dist -= Time.deltaTime*speed;
        transform.position = new Vector3(dist*Mathf.Cos(angle), 0, dist*Mathf.Sin(angle));
    }

    void Teleport()
    {
        angle = Random.Range(0, 360);
        if(fake)
        {
            angle = Random.Range(0, 360);
        }
        transform.position = new Vector3(dist*Mathf.Cos(angle), 0, dist*Mathf.Sin(angle));
        transform.LookAt(Vector3.zero);
        GetComponent<AudioSource>().pitch = Random.Range(0.400f, 0.600f);
        GetComponent<AudioSource>().Play();
        glitchAmount = 0.25f;
    }

    public void LoseHealth()
    {
        if(fake)
        {
            poof.Poofs(transform.position, 2);
            if (PlayerPrefs.GetInt("mode") == 2)
            {
                ans.LoseEnemy();
            }
            else
            {
                es.LoseEnemy();
            }
            healthScript.qInc("boss");
            healthScript.qInc("animal");
            GameObject realB = GameObject.Find("realButcher");
            agv.scanLineJitter.value = 0;
            agv.verticalJump.value = 0;
            agv.horizontalShake.value = 0;
            agv.colorDrift.value = 0;
            dgv.intensity.value = 0;
            poof.Poofs(realB.transform.position, 2);
            healthScript.ratAppear();
            Destroy(realB);
            Destroy(transform.gameObject);  
        } else
        {
            damage = 100;
            Teleport();
            glitchAmount = 5f;
        }
    }
}
