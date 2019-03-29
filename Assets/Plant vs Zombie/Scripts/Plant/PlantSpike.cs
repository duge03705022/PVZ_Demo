using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SearchZombie))]
public class PlantSpike : MonoBehaviour {

    public int atk;
    public int attackCount;
    public float cd;
    public float range;

    private PlantGrow grow;
    private SearchZombie search;
    private float cdTime;
	void Awake () {
        grow = GetComponent<PlantGrow>();
        search = GetComponent<SearchZombie>();
        enabled = false;
	}

    void AfterGrow() {
        enabled = true;
    }
	
	void Update () {
        if (cdTime >= 0) {
            cdTime -= Time.deltaTime;
        } else {
            object[] zombies = search.SearchZombiesInRange(grow.row, -range, range);
            if (zombies.Length != 0) {
                foreach (GameObject zombie in zombies) {
                    zombie.GetComponent<ZombieHealthy>().Damage(atk);
                    --attackCount;
                }
                cdTime = cd;

                if (attackCount <= 0) {
                    GameModel.GetInstance().map[grow.row, grow.col] = null;
                    Destroy(gameObject);
                }
            }
        }
	}
}
