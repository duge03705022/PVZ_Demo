using UnityEngine;
using System.Collections;

public class GameTips : MonoBehaviour {

    public Sprite[] startLabels = new Sprite[3];
    public Sprite approachingLabel;
    public Sprite finalLabel;
    public Sprite lostLabel;

    private new SpriteRenderer renderer;

	void Awake () {
        renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
	}

    public void ShowStartTip() {
        StartCoroutine(DoShowStart());
    }

    IEnumerator DoShowStart() {
        renderer.enabled = true;
        for (int i = 0; i < startLabels.Length; ++i) {
            renderer.sprite = startLabels[i];
            yield return new WaitForSeconds(0.6f);
        }
        yield return new WaitForSeconds(0.6f);
        renderer.enabled = false;
    }

    public void ShowApproachingTip() {
        StartCoroutine(DoShowLabel(approachingLabel, 3.0f));
    }

    public void ShowFinalTip() {
        StartCoroutine(DoShowLabel(finalLabel, 3.0f));
    }

    IEnumerator DoShowLabel(Sprite label, float time) {
        renderer.enabled = true;
        renderer.sprite = label;
        yield return new WaitForSeconds(time);
        renderer.enabled = false;
    }

    public void ShowLostTip() {
        renderer.enabled = true;
        renderer.sprite = lostLabel;
    }
}
