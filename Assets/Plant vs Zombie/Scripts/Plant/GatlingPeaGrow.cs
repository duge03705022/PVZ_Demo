using UnityEngine;
using System.Collections;

public class GatlingPeaGrow : PlantGrow {

    public override bool canGrowInMap(int row, int col) {
        GameObject plant = model.map[row, col];
        if (plant) {
            if (plant.tag == "Repeater") {
                return true;
            } else if (plant.GetComponent<PumpkinGrow>()) {
                PumpkinGrow pumpGrow = plant.GetComponent<PumpkinGrow>();
                if (pumpGrow.innerPlant && pumpGrow.innerPlant.tag == "Repeater") {
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

        GameObject mapPlant = model.map[row, col];
        if (mapPlant && mapPlant.GetComponent<PumpkinGrow>()) {
            Destroy(mapPlant.GetComponent<PumpkinGrow>().innerPlant);
            mapPlant.GetComponent<PumpkinGrow>().innerPlant = gameObject;
        } else {
            Destroy(model.map[row, col]);
            model.map[row, col] = gameObject;
        }

        display.SetOrderByRow(row);

        if (shadow) {
            shadow.gameObject.SetActive(true);
        }

        if (soil) {
            GameObject temp = Instantiate(soil);
            temp.transform.position = transform.position;
            Destroy(temp, 0.2f);
        }

        gameObject.SendMessage("AfterGrow");
    }
}
