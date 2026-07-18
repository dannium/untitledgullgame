using UnityEngine;

public class bombSpawner : MonoBehaviour
{
    Animator animator;
    int animFrame;
    GameObject bombPrefab;
    [SerializeField] Vector3 offset;
    bool canSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bombPrefab = Resources.Load<GameObject>("prefabs/day3/bombPrefab");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animFrame = Mathf.RoundToInt(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime * 121) % 121;
        if(animFrame > 100 && canSpawn)
        {
            canSpawn = false;
            GameObject newBomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            newBomb.transform.position += offset;
            newBomb.GetComponent<bombMovement>().angle = GetComponent<EnemyMovement>().angle;
        } else if(animFrame < 100) {
            canSpawn = true;
        }
    }
}
