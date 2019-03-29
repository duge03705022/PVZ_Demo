using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour {

    private GameModel model;
    private PlantGrow grow;
    void Awake()
    {
        grow = GetComponent<PlantGrow>();
        model = GameModel.GetInstance();
    }

    void AfterGrow()
    {
        model.holeMap[grow.row, grow.col] = true;
    }
}
