using UnityEngine;

public class jumpscare : MonoBehaviour
{
    [SerializeField] Animator animator;
    bool growin;
    bool shrinkin;
    float cd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cd = Random.Range(100, 600);
    }

    // Update is called once per frame
    void Update()
    {
        cd -= Time.deltaTime;
        if( cd < 0 )
        {
            jump();
        }
        if(growin)
        {
            if(transform.localScale.x > 4.9)
            {
                growin = false;
                shrinkin = true;
            }
            transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.x, 5, 20*Time.deltaTime);
        }

        if (shrinkin)
        {
            if (transform.localScale.x < 0.1)
            {
                shrinkin = false;
                animator.SetBool("start", false);
            }
            transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.x, 0, 10*Time.deltaTime);
        }
    }

   void jump()
    {
        cd = Random.Range(100, 600);
        growin = true;
        animator.SetBool("start", true);
        GetComponent<AudioSource>().Play();
   }
}
