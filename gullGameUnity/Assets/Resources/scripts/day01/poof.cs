using Unity.VisualScripting;
using UnityEngine;

public class poof : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;
    ParticleSystem newPoof;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (newPoof != null)
        {
            if (!newPoof.isPlaying)
            {
                Destroy(newPoof);
            }
        }
    }

    public void Poofs(Vector3 pos, float scale)
    {
        newPoof = Instantiate(ps, pos, Quaternion.identity);
        newPoof.transform.localScale = Vector3.one * scale;
    }
}
