using UnityEngine;
using System.Collections;

public class ZombieHealthy : MonoBehaviour {

    public AudioClip damageSound;
    public GameObject head;
    public Vector3 headOffset;
    public int hp = 20; //change outside
    public int lostHeadHp = 10; //40
	[HideInInspector]
	public bool isBoomDie = false;

    protected Animator animator;
    protected Blink blink;
    protected bool hasHead = true;

	protected void Awake () {
        animator = transform.Find("zombie").GetComponent<Animator>();
        blink = transform.Find("zombie").GetComponent<Blink>();
	}

    public virtual void Damage(int val) {
        if (hp <= 0) return;
        AudioManager.GetInstance().PlaySound(damageSound);

        hp -= val;
        animator.SetInteger("hp", hp);
        blink.Begin(0.15f);
        if (hp <= lostHeadHp && hasHead) {
            LostHead();
        }
        if (hp <= 0) Die();
    }

    protected void Die() {
        ZombieMove move = GetComponentInChildren<ZombieMove>();
        GameModel.GetInstance().zombieList[move.row].Remove(gameObject);
        move.enabled = false;
        GetComponentInChildren<ZombieAttack>().StopAttack();

        Destroy(gameObject, 3.0f);
    }

    public void BoomDie() {
        if (hp <= 0) return;
		isBoomDie = true;
        animator.SetTrigger("boomDie");
        Die();
    }

    protected virtual void LostHead() {
        GameObject newHead = Instantiate(head);
        newHead.transform.position = transform.position + headOffset;
        Destroy(newHead, 3f);
        hasHead = false;
    }

    public void Eat() {
        ZombieMove move = GetComponentInChildren<ZombieMove>();
        GameModel.GetInstance().zombieList[move.row].Remove(gameObject);
        move.enabled = false;
        Destroy(gameObject);
    }
}
