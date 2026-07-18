using UnityEngine;

public class remove : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<ParticleSystem>().isPlaying)
        {
            Destroy(transform.gameObject);
        }
    }
}
