using UnityEngine;
using System.Collections;

public class PumpkinGrow : PlantGrow {

    [HideInInspector]
    public GameObject innerPlant;

    private PumpkinSpriteDisplay pumpDisplay;

    new void Awake() {
        model = GameModel.GetInstance();
        pumpDisplay = GetComponent<PumpkinSpriteDisplay>();
    }

    public override bool canGrowInMap(int row, int col) {
        GameObject plant = model.map[row, col];
        if (plant && plant.GetComponent<PumpkinGrow>()) {
            return false;
        }
        return true;
    }

    public override void grow(int _row, int _col) {
        row = _row;
        col = _col;

        innerPlant = model.map[row, col];
        model.map[row, col] = gameObject;

        pumpDisplay.SetOrderByRow(row);
    }
}
