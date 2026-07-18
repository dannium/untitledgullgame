using System;
using System.Collections;
using UnityEngine;

public class ricochetBullet : MonoBehaviour
{
    float time;
    Material mat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        mat.SetFloat(Shader.PropertyToID("_alpha"), (2-time)/2);
        if (time > 2)
        {
            Destroy(transform.gameObject);
        }
    }

    IEnumerator die()
    {
        yield return new WaitForSeconds(1);
        Destroy(transform.gameObject);
    }
}
