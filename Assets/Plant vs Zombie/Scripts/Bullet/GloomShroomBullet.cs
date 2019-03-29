using UnityEngine;
using System.Collections;

public class GloomShroomBullet : MonoBehaviour {

    public int atk;
    public float attackRadius;
    public float destroyTime;

    private SearchZombie search;

    void Awake() {
        search = GetComponent<SearchZombie>();
    }

    void Start() {
        object[] zombies = search.SearchZombiesInRange(attackRadius);
        foreach (GameObject zombie in zombies) {
            zombie.GetComponent<ZombieHealthy>().Damage(atk);
        }
        Destroy(gameObject, destroyTime);
    }
}
