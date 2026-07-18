using UnityEngine;

public class tornadoMovement : MonoBehaviour
{
    public float angle;
    bool summonFire;
    GameObject firePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.eulerAngles = Vector3.up*angle;
        transform.localScale = Vector3.one * 0.1f;
        transform.position = Vector3.up * 3.5f;

        firePrefab = Resources.Load<GameObject>("prefabs/day3/firePrefab");
        summonFire = Random.Range(0, 5) == 2;

        if (Random.Range(0, 100) == 67)
        {
            GameObject.Find("minion").GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }

        angle *= Mathf.Deg2Rad;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime);
        transform.position += Time.deltaTime * new Vector3(-12 * Mathf.Cos(angle+(Mathf.PI/2)), 0, 12 * Mathf.Sin(angle+(Mathf.PI / 2)));
        
        if(summonFire && transform.position.magnitude > 5)
        {
            summonFire = false;
            GameObject newFire = Instantiate(firePrefab);
            newFire.transform.LookAt(Vector3.zero);
            newFire.GetComponent<killMovement>().angle = Random.Range(0, 360);
            newFire.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            newFire.transform.localScale = Vector3.one * 0.1f;
        }
        
        if(transform.position.magnitude > 30)
        {
            Destroy(transform.gameObject);
        }
    }
}
