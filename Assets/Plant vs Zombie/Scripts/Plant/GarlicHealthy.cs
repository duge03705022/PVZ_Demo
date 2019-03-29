using UnityEngine;
using System.Collections;

public class GarlicHealthy : PlantHealthy {

    private SearchZombie search;

	new void Awake () {
        base.Awake();
        search = GetComponent<SearchZombie>();
	}

    public override void Damage(int val) {
        base.Damage(val);
        GameObject zombie = search.SearchClosetZombie(grow.row, 0, StageMap.GRID_WIDTH);
        if (zombie) {
            bool upward;
            switch (grow.row) {
                case 0:
                    upward = false;
                    break;
                case (StageMap.ROW_MAX - 1):
                    upward = true;
                    break;
                default:
                    upward = (Random.Range(0f, 1f) < 0.5);
                    break;
            }

            zombie.GetComponent<ZombieMove>().ChangeRow(upward);
        }
    }

    void AfterGrow() { }
}
