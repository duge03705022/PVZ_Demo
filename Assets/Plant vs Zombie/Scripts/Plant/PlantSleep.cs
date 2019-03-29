using UnityEngine;
using System.Collections;

public class PlantSleep : MonoBehaviour {

    public MonoBehaviour[] stopBehaviour;

    private GameModel model;
    private Animator animator;

	void Awake () {
        model = GameModel.GetInstance();
        animator = transform.Find("plant").GetComponent<Animator>();    
	}

    public void WakeUp(float time) {
        animator.SetBool("isSleeping", false);
        EnableBehaviour(true);
        Invoke("Sleep", time);
    }

    void AfterGrow() {
        if (model.inDay) {
            Invoke("Sleep", 0);
        }      
    }

    void Sleep() {
        animator.SetBool("isSleeping", true);
        EnableBehaviour(false);
    }

    void EnableBehaviour(bool enabled) {
        foreach (MonoBehaviour script in stopBehaviour) {
            script.enabled = enabled;
        }
    }
}
