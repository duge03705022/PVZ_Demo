using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SearchZombie))]
public class PlantShoot : MonoBehaviour {

    public GameObject[] bullets;
    public Vector3 bulletOffset;
    public float interval;
    public float cd;
    public float range;

    private PlantGrow grow;
    private SearchZombie search;
    private float cdTime = 0;

	void Awake () {
        grow = GetComponent<PlantGrow>();
        search = GetComponent<SearchZombie>();
        enabled = false;
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
                StartCoroutine(Shoot());
                cdTime = cd;
            }
        }
	}

    IEnumerator Shoot() {
        Vector3 pos = transform.position + bulletOffset;
        foreach (GameObject bullet in bullets) {
            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.position = pos;
            newBullet.GetComponent<Bullet>().row = grow.row;
            newBullet.GetComponent<SpriteRenderer>().sortingOrder = 1000 * (grow.row + 1) + 1;
            yield return new WaitForSeconds(interval);
        }
    }
}
