using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SearchZombie))]
public class PlantShootRound : MonoBehaviour
{

    public GameObject[] bullets;
    public Vector3 bulletOffset;
    public float interval;
    public float cd;
    public float range;

    public bool ifRight = false;
    public bool ifUp = false;
    public bool ifLeft = false;
    public bool ifDown = false;

    private PlantGrow grow;
    private SearchZombie search;
    private float cdTime = 0;

    void Awake()
    {
        grow = GetComponent<PlantGrow>();
        search = GetComponent<SearchZombie>();
        enabled = false;
    }

    void AfterGrow()
    {
        enabled = true;
    }

    void Update()
    {
        if (cdTime > 0)
        {
            cdTime -= Time.deltaTime;
        }
        else
        {
            bool hasZombie = search.IsZombieInRange(range);
            if (hasZombie)
            {
                StartCoroutine(Shoot());
                cdTime = cd;
            }
        }
    }

    IEnumerator Shoot()
    {
        Vector3 pos = transform.position + bulletOffset;
        foreach (GameObject bullet in bullets)
        {
            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.position = pos;
            newBullet.GetComponent<Bullet>().row = grow.row;

            if (ifUp)
            {
                newBullet.GetComponent<Bullet>().TurnUp();
            }
            else if (ifDown)
            {
                newBullet.GetComponent<Bullet>().TurnDown();
            }
            else if (ifLeft)
            {
                newBullet.GetComponent<Bullet>().Reverse();
            }

            newBullet.GetComponent<SpriteRenderer>().sortingOrder = 1000 * (grow.row + 1) + 1;
            yield return new WaitForSeconds(interval);
        }
    }
}
