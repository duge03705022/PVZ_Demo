using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SearchZombie))]
public class Threepeater : MonoBehaviour {

    public GameObject bullet;
    public Vector3 bulletOffset;
    public float cd;
    public float range;

    private PlantGrow grow;
    private SearchZombie search;
    private float cdTime = 0;

    void Awake() {
        grow = GetComponent<PlantGrow>();
        search = GetComponent<SearchZombie>();
        enabled = false;
    }

    void Update() {
        if (cdTime > 0) {
            cdTime -= Time.deltaTime;
        } else {
            bool hasZombie = search.IsZombieInRange(grow.row, 0, range);
            if (grow.row - 1 >= 0)
                hasZombie = hasZombie ||search.IsZombieInRange(grow.row - 1, 0, range);
            if (grow.row + 1 < StageMap.ROW_MAX)
                hasZombie = hasZombie || search.IsZombieInRange(grow.row + 1, 0, range);
            if (hasZombie) {
                Shoot();
                cdTime = cd;
            }
        }
    }

    void AfterGrow() {
        enabled = true;
    }

    void Shoot() {
        Vector3 pos = transform.position + bulletOffset;
        GameObject[] bullets = new GameObject[3];
        for (int i = 0; i < 3; ++i) {
            bullets[i] = Instantiate(bullet);
            bullets[i].transform.position = pos;
            bullets[i].GetComponent<Bullet>().row = grow.row - 1 + i;
            bullets[i].GetComponent<SpriteRenderer>().sortingOrder = 1000 * (grow.row + i) + 1;
        }

        MoveBy move1 = bullets[0].AddComponent<MoveBy>();
        move1.offset.Set(0, StageMap.GRID_HEIGHT, 0);
        move1.time = 0.2f;
        move1.Begin();
        MoveBy move2 = bullets[2].AddComponent<MoveBy>();
        move2.offset.Set(0, -StageMap.GRID_HEIGHT, 0);
        move2.time = 0.2f;
        move2.Begin();
    }
}
