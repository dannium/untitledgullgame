using UnityEngine;

public class purpleguy : MonoBehaviour
{
    bool goDown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goDown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(goDown)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 4.6f, 2*Time.deltaTime), transform.position.z);
            if(transform.position.y < 4.62f)
            {
                goDown = false;
            }
        } else
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 16f, 2*Time.deltaTime), transform.position.z);
        }
        if(transform.position.y > 15)
        {
            Destroy(gameObject);
        }

        transform.LookAt(Vector3.zero);
        transform.eulerAngles = transform.eulerAngles.y*Vector3.up;
        transform.eulerAngles = transform.eulerAngles + Vector3.forward * 90;

    }
}
