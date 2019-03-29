using UnityEngine;
using System.Collections;

public class ScaredShroom : MonoBehaviour {

    public float scaredRange;
    public float recoverTime;

    private Animator animator;
    private SearchZombie search;
    private PlantShoot shoot;
    private bool isCrying = false;

	void Awake () {
        animator = transform.Find("plant").GetComponent<Animator>();
        search = GetComponent<SearchZombie>();
        shoot = GetComponent<PlantShoot>();
        enabled = false;
	}

    void AfterGrow() {
        enabled = true;
    }
	
	void Update () {
        bool hasZombie = search.IsZombieInRange(scaredRange);
        if (hasZombie && !isCrying) {
            if (IsInvoking("Recover")) {
                CancelInvoke("Recover");
            } else {
                isCrying = true;
                shoot.enabled = false;
                animator.SetBool("isCrying", true);
            }          
        } else if (!hasZombie && isCrying) {
            if (!IsInvoking("Recover")) {
                Invoke("Recover", recoverTime);
            }
        }
	}

    void Recover() {
        isCrying = false;
        shoot.enabled = true;
        animator.SetBool("isCrying", false);
    }
}
