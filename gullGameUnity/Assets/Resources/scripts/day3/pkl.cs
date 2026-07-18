using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class pkl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(run());
        }
    }

    IEnumerator run()
    {
        float ts = Random.Range(0, 100);
        if(ts < 5)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("materials/day3/p3");
        } else if(ts >= 5 &&  ts < 15)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("materials/day3/p2");
        }
        else
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("materials/day3/p1");
        }
        GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        GetComponent<Image>().enabled = false;
    }
}
