using UnityEngine;
using System.Collections;

public class HypnoShroomHealthy : PlantHealthy {

    private SearchZombie search;

    new void Awake() {
        base.Awake();
        search = GetComponent<SearchZombie>();
    }

    public override void Die() {
        base.Die();
        GameObject zombie = search.SearchClosetZombie(grow.row, 0, StageMap.GRID_WIDTH);
        if (zombie) {
            // TODO
            zombie.GetComponent<ZombieHealthy>().BoomDie();
        }
    }

    void AfterGrow() { }
}
