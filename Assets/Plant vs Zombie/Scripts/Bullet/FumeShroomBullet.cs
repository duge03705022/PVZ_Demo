using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SearchZombie))]
public class FumeShroomBullet : MonoBehaviour {

    public int atk;
    public float attackLength;
    public float destroyTime;
    [HideInInspector]
    public int row;

    private SearchZombie search;   

	void Awake () {
        search = GetComponent<SearchZombie>();
	}
	
	void Start () {
        object[] zombies = search.SearchZombiesInRange(row, -attackLength / 2, attackLength / 2);
        foreach (GameObject zombie in zombies) {
            zombie.GetComponent<ZombieHealthy>().Damage(atk);
        }
        Destroy(gameObject, destroyTime);
	}
}
