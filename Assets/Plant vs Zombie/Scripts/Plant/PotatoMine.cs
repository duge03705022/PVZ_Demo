using UnityEngine;
using System.Collections;

public class PotatoMine : MonoBehaviour {

    public AudioClip explodeSound;
    public GameObject effect;
    public Vector3 effectOffset;
    public float readyTime;
    public float range;
    public float destroyTime;

    private Animator animator;
    private PlantGrow grow;
    private SearchZombie search;

	void Awake () {
        animator = transform.Find("plant").GetComponent<Animator>();
        grow = GetComponent<PlantGrow>();
        search = GetComponent<SearchZombie>();
        enabled = false;
	}

    void AfterGrow() {
        animator.SetBool("isReady", false);
        Invoke("GetReady", readyTime);
    }

    void GetReady() {
        animator.SetBool("isReady", true);
        enabled = true;
    }
	
	void Update () {
        object[] zombies = search.SearchZombiesInRange(grow.row, -range, range);
        if (zombies.Length != 0) {
            foreach (GameObject zombie in zombies) {
                zombie.GetComponent<ZombieHealthy>().Eat();
            }
            animator.SetTrigger("boom");

            GameObject eff = Instantiate(effect);
            eff.transform.position = transform.position + effectOffset;
            eff.GetComponent<SpriteRenderer>().sortingOrder =
                transform.Find("plant").GetComponent<SpriteRenderer>().sortingOrder + 1;
            Destroy(eff, destroyTime);

            AudioManager.GetInstance().PlaySound(explodeSound);
            Invoke("DoDestroy", destroyTime);
            enabled = false;
        }
	}

    void DoDestroy() {
        GetComponent<PlantHealthy>().Die();
    }
}
