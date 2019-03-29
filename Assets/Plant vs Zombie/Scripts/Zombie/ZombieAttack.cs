using UnityEngine;
using System.Collections;

public class ZombieAttack : MonoBehaviour {

    public AudioClip attackSound;
    public int atk = 100;
    public float cd = 1f;
    public float range = 0.8f;

    protected Animator animator;
    protected AudioSource audioSource;
    protected GameModel model;
    protected ZombieMove move;
    protected AbnormalState state;
    protected GameObject target;

    void Awake() {
        animator = transform.Find("zombie").GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        model = GameModel.GetInstance();
        move = GetComponent<ZombieMove>();
        state = GetComponent<AbnormalState>();
    }

    void Update() {
        if (null == target) {
            target = SearchPlant();
        }

        if (target && move.enabled) {
            move.enabled = false;
            animator.SetBool("isAttacking", true);
            audioSource = AudioManager.GetInstance().PlaySound(attackSound, true);
            Invoke("DoAttack", cd);
        } else if (!target && !move.enabled) {
            move.enabled = true;
            animator.SetBool("isAttacking", false);
            AudioManager.GetInstance().StopSound(audioSource);
            CancelInvoke("DoAttack");
        }
		target = null;
	}

    public void DoAttack() {
        if (target) {
            target.GetComponent<PlantHealthy>().Damage(atk);
        }
        Invoke("DoAttack", cd);
    }

    public void StopAttack() {
        AudioManager.GetInstance().StopSound(audioSource);
        enabled = false;
    }

    public GameObject SearchPlant() {
        GameObject target = null;
        float minDis = 100000;
        for (int col = 0; col < StageMap.COL_MAX; ++col) {
            GameObject plant = model.map[move.row, col];
            if (plant && plant.GetComponent<PlantHealthy>()) {
                float dis = transform.position.x - plant.transform.position.x;
                if (0 <= dis && dis <= range) {
                    if (minDis > dis) {
                        minDis = dis;
                        target = plant;
                    }
                }
            }
        }
        return target;
    }
}
