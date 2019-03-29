using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SearchZombie))]
public class Chomper : MonoBehaviour {

    public AudioClip chompSound;
    public float eatRange;
    public float cd;

    private Animator animator;
    private PlantGrow grow;
    private GameModel model;
    private SearchZombie search;
    private float cdTime = 0;
    private bool isReady = true;

	void Awake () {
        animator = transform.Find("plant").GetComponent<Animator>();
        grow = GetComponent<PlantGrow>();
        model = GameModel.GetInstance();
        search = GetComponent<SearchZombie>();
        enabled = false;
	}
	
	void Update () {
        if (cdTime > 0) {
            cdTime -= Time.deltaTime;
        } else {
            if (!isReady) {
                isReady = true;
                animator.SetTrigger("ready");
            }
            bool hasZombie = search.IsZombieInRange(grow.row, 0, eatRange);
            if (hasZombie) {
                animator.SetTrigger("eat");
                cdTime = cd;
                Invoke("ChompSound", 0.6f);
            }
        }
	}

    void AfterGrow() {
        enabled = true;
    }

    void ChompSound() {
        AudioManager.GetInstance().PlaySound(chompSound);
    }

    void DoEat() {
        bool hasEaten = false;
        object[] zombies = model.zombieList[grow.row].ToArray();
        foreach (GameObject zombie in zombies) {
            float dis = zombie.transform.position.x - transform.position.x;
            if (0 <= dis && dis <= eatRange) {
                zombie.GetComponent<ZombieHealthy>().Eat();
                hasEaten = true;
            }
        }

        if (hasEaten) {
            cdTime = cd;
            isReady = false;
        } else {
            animator.SetTrigger("cancelEat");
            cdTime = 0;
        }
    }
}
