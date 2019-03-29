using UnityEngine;
using System.Collections;

public class CoffeeBeanGrow : PlantGrow {

    public AudioClip destroySound;
    public float effectiveTime;
    public float duration;
    public float animationTime;
    [HideInInspector]
    public GameObject sleepPlant;

    private Animator animator;

    new void Awake() {
        base.Awake();
        animator = transform.Find("plant").GetComponent<Animator>();
        display = GetComponent<PlantSpriteDisplay>();
        enabled = false;
    }

    void Update() {
        GameObject plant = model.map[row, col];
        if (!plant) {
            StopAllCoroutines();
            DoDestroy();
        }
    }  

    public override bool canGrowInMap(int row, int col) {
        GameObject plant = model.map[row, col];
        if (plant) {
            if (plant.GetComponent<PlantSleep>()) {
                return true;
            } else if (plant.GetComponent<PumpkinGrow>()) {
                PumpkinGrow pumpGrow = plant.GetComponent<PumpkinGrow>();
                if (pumpGrow.innerPlant && pumpGrow.innerPlant.GetComponent<PlantSleep>()) {
                    return true;
                } else {
                    return false;
                }
            }           
        }
        return false;
    }

    public override void grow(int _row, int _col) {
        row = _row;
        col = _col;
        if (model.map[row, col].GetComponent<PlantSleep>()) {
            sleepPlant = model.map[row, col];
        } else {
            sleepPlant = model.map[row, col].GetComponent<PumpkinGrow>().innerPlant;
        }

        display.SetOrderByRow(row);
        StartCoroutine(TakeEffect());
        enabled = true;
    }

    IEnumerator TakeEffect() {
        yield return new WaitForSeconds(effectiveTime);
        sleepPlant.GetComponent<PlantSleep>().WakeUp(duration);

        yield return new WaitForSeconds(duration);
        DoDestroy();
    }

    void DoDestroy() {
        animator.SetTrigger("destroy");
        Destroy(gameObject, animationTime);
        AudioManager.GetInstance().PlaySound(destroySound);
    }
}
