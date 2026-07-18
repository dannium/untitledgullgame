using UnityEngine;

public class testGunMovement : MonoBehaviour
{

    Transform bone;
    public float x;
    public float y;
    public float z;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bone = GameObject.FindGameObjectWithTag("BONE").transform;

    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = bone.eulerAngles + new Vector3(x, y, z);
    }
}
