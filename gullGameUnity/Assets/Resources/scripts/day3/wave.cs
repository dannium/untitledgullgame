using System;
using Unity.VisualScripting;
using UnityEngine;

public class wave : MonoBehaviour
{
    private static float wavelength = 8;
    private static float speed = 0.4f;
    private float time;
    private int neg;
    private Material mat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        if (PlayerPrefs.GetInt("mode") != 1) {
            mat.SetFloat(Shader.PropertyToID("_choc"), UnityEngine.Random.Range(0, 2));
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime*speed;
        neg = (time % (2 * wavelength) < wavelength) ? 1: -1;
        transform.position = new Vector3(transform.position.x, neg*((float)Math.Tanh((time % wavelength)-wavelength/2)) - 1.5f, transform.position.z);

        mat.SetFloat(Shader.PropertyToID("_tests"), -75*transform.position.y + 212.5f);
        mat.SetVector(Shader.PropertyToID("_offset"), new Vector2(time * 0.15f, time * 0.6f));
    }
}
