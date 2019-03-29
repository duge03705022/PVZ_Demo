using UnityEngine;
using System.Collections;


public class AbnormalState : MonoBehaviour {

    public GameObject icetrap;
    [HideInInspector]
    public float ratio = 1.0f;

    private int SPEED_DOWN = 1 << 0;
    private int FREEZE_UP = 1 << 2;

    private ZombieSpriteDisplay display;
    private Animator animator;
    private int state;
    private float speedDownRatio;

	void Awake () {
        display = GetComponent<ZombieSpriteDisplay>();
        animator = transform.Find("zombie").GetComponent<Animator>();
        state = 0;
	}

    public void SpeedDown(float time, float val) {
        speedDownRatio = val;
        state |= SPEED_DOWN;
        UpdateAction();
        if (IsInvoking("RemoveSpeedDown")) {
            CancelInvoke("RemoveSpeedDown");
        }
        Invoke("RemoveSpeedDown", time);
    }

    void RemoveSpeedDown() {
        state &= ~SPEED_DOWN;
        UpdateAction();
    }

    public void FreezeUp(float time) {
        state |= FREEZE_UP;
        UpdateAction();
        GameObject ice = Instantiate(icetrap);
        ice.transform.position = transform.position;
        ice.GetComponent<SpriteRenderer>().sortingOrder =
            transform.Find("zombie").GetComponent<SpriteRenderer>().sortingOrder + 1;     
        Destroy(ice, time);

        if (IsInvoking("RemoveFreezeUp")) {
            CancelInvoke("RemoveFreezeUp");
        }
        Invoke("RemoveFreezeUp", time);
    }

    void RemoveFreezeUp() {
        state &= ~FREEZE_UP;
        UpdateAction();
    }

    void UpdateAction() {
        float val;
        if ((state & FREEZE_UP) != 0) {
            val = 0;
            display.SetColor(0.5f, 0.5f, 1f);
        } else if ((state & SPEED_DOWN) != 0) {
            val = speedDownRatio;
            display.SetColor(0.5f, 0.5f, 1f);
        } else {
            val = 1;
            display.SetColor(1f, 1f, 1f);
        }
        ratio = val;
        animator.speed = val;
    }
}
