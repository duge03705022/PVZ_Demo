using UnityEngine;
using System.Collections;

public class Starfruit : MonoBehaviour {

    public GameObject bullet;
    public Vector3 bulletOffset;
    public Vector3[] direction = new Vector3[5];
    public float cd;
    public float range;

    private SearchZombie search;
    private float cdTime;
	void Awake () {
        search = GetComponent<SearchZombie>();
        enabled = false;
        for (int i = 0; i < 5; ++i) {
            direction[i].Normalize();
        }
	}

    void AfterGrow() {
        enabled = true;
    }
	
	void Update () {
        if (cdTime > 0) {
            cdTime -= Time.deltaTime;
        } else {
            bool hasZombie = search.IsZombieInRange(range);
            if (hasZombie) {
                Shoot();
                cdTime = cd;
            }
        }
	}

    void Shoot() {
        for (int i = 0; i < 5; ++i) {
            GameObject star = Instantiate(bullet);
            star.transform.position = transform.position + bulletOffset;
            star.GetComponent<StartBullet>().direction = direction[i];
        }
    }
}
