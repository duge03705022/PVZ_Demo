using UnityEngine;
using System.Collections;

public class LostPaperAction : StateMachineBehaviour {

    public AudioClip angrySound;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.transform.parent.GetComponent<PaperZombieMove>().speed = 0;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {    
        PaperZombieMove move = animator.transform.parent.GetComponent<PaperZombieMove>();
        move.speed = move.angrySpeed;

        PaperZombieAttack attack = animator.transform.parent.GetComponent<PaperZombieAttack>();
        attack.cd = attack.angryCd;

        AudioManager.GetInstance().PlaySound(angrySound);
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
