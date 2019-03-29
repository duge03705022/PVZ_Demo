using UnityEngine;
using System.Collections;

public class PoleZombieMove : ZombieMove {

    public float walkSpeed = 0.1f;

    private Animator animator;
    private ZombieAttack attack;
    private bool hasJumped = false;

    new void Awake() {
        base.Awake();
        animator = transform.Find("zombie").GetComponent<Animator>();
        attack = GetComponent<ZombieAttack>();
        attack.enabled = false;
    }

    void Update() {
        transform.Translate(-speed * Time.deltaTime * state.ratio, 0, 0);
        if (!hasJumped) {
            GameObject plant = attack.SearchPlant();
            if (plant) {
                if (plant.tag == "UnableJump") {
                    animator.SetTrigger("giveUp");
                    attack.enabled = true;
                } else {
                    animator.SetTrigger("jump");
                }
                speed = walkSpeed;
                hasJumped = true;
            }
        }
    }
}
