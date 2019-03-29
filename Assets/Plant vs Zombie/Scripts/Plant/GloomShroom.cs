using UnityEngine;
using System.Collections;

public class GloomShroom : MonoBehaviour {

    public GameObject bullet;
    public Vector3 bulletOffset;
    public float cd;
    public float range;
    public float attackTime;

    private Animator animator;
    private PlantGrow grow;
    private SearchZombie search;
    private float cdTime = 0;

    void Awake() {
        animator = transform.Find("plant").GetComponent<Animator>();
        grow = GetComponent<PlantGrow>();
        search = GetComponent<SearchZombie>();
        enabled = false;
    }

    void AfterGrow() {
        enabled = true;
    }

    void Update() {
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
        GameObject newBullet = Instantiate(bullet);
        newBullet.transform.position = pos;
        newBullet.transform.Find("bullet")
            .GetComponent<SpriteRenderer>().sortingOrder= 1000 * (grow.row + 1) + 1;

        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(attackTime);
        animator.SetBool("isAttacking", false);
    }
}
