using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {

    public GameObject flagPrefab;

    private const float leftX = -0.69f;
    private const float rightX = 0.69f;
    private Material fullMaterial;
    private GameObject head;

	void Awake () {
        fullMaterial = transform.Find("full").GetComponent<SpriteRenderer>().material;
        head = transform.Find("head").gameObject;
	}

    public void InitWithFlag(float[] percentage) {
        for (int i = 0; i < percentage.Length; ++i) {
            GameObject flag = Instantiate(flagPrefab);
            flag.transform.parent = transform;
            float val = Mathf.Clamp(percentage[i], 0f, 1f);
            float x = Mathf.Lerp(rightX, leftX, val);
            flag.transform.localPosition = new Vector3(x, 0.06f, 0);
        }
    }

    public void SetProgress(float ratio) {
        ratio = Mathf.Clamp(ratio, 0f, 1f);
        fullMaterial.SetFloat("_Progress", ratio);
        float x = Mathf.Lerp(rightX, leftX, ratio);
        head.transform.localPosition = new Vector3(x, 0.03f, 0);
    }
}
