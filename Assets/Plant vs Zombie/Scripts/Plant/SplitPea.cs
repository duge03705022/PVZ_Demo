using UnityEngine;
using System.Collections;

public class SplitPea : MonoBehaviour {

    public GameObject bullet;
    public Vector3 rightOffset;
    public Vector3 leftOffset;
    public float cd;
    public float range;

    private PlantGrow grow;
    private SearchZombie search;
    private float rightCdTime = 0;
    private float leftCdTime = 0;

    void Awake() {
        grow = GetComponent<PlantGrow>();
        search = GetComponent<SearchZombie>();
        enabled = false;
    }

    void AfterGrow() {
        enabled = true;
    }

    void Update() {
        if (rightCdTime > 0) {
            rightCdTime -= Time.deltaTime;
        } else {
            bool hasZombie = search.IsZombieInRange(grow.row, 0, range);
            if (hasZombie) {
                RightShoot();
                rightCdTime = cd;
            }
        }

        if (leftCdTime > 0) {
            leftCdTime -= Time.deltaTime;
        } else {
            bool hasZombie = search.IsZombieInRange(grow.row, -range, 0);
            if (hasZombie) {
                LeftShoot();
                leftCdTime = cd;
            }
        }
    }

    void RightShoot() {
        GameObject newBullet = Instantiate(bullet);
        newBullet.transform.position = transform.position + rightOffset;
        newBullet.GetComponent<Bullet>().row = grow.row;
        newBullet.GetComponent<SpriteRenderer>().sortingOrder = 1000 * (grow.row + 1) + 1;
    }

    void LeftShoot() {
        GameObject newBullet = Instantiate(bullet);
        newBullet.transform.position = transform.position + leftOffset;
        newBullet.GetComponent<Bullet>().row = grow.row;
        newBullet.GetComponent<Bullet>().Reverse();
        newBullet.GetComponent<SpriteRenderer>().sortingOrder = 1000 * (grow.row + 1) + 1;
    }
}
