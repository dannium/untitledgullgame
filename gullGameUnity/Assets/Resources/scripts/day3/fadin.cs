using UnityEngine;
using UnityEngine.UI;

public class fadin : MonoBehaviour
{
    Image fadinImg;
    bool done;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        done = false;
        fadinImg = transform.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            fadinImg.color = new Color(0.06f, 0.06f, 0.12f, Mathf.Lerp(fadinImg.color.a, 0, 3 * Time.deltaTime));
            if (fadinImg.color.a < 0.01f)
            {
                done = true;
                fadinImg.gameObject.SetActive(false);
                Destroy(transform.gameObject.GetComponent<fadin>());
            }
        }
    }
}
