using UnityEngine;
using System.Collections;

public class Crater : MonoBehaviour {

    public float leftTime;

    private PlantGrow grow;
	void Awake () {
        grow = GetComponent<PlantGrow>();
	}

    void AfterGrow() {
        StartCoroutine(DoDestroy());
    }

    IEnumerator DoDestroy() {
        yield return new WaitForSeconds(leftTime);

        GameModel model = GameModel.GetInstance();
        model.map[grow.row, grow.col] = null;
        Destroy(gameObject);
    }
}
