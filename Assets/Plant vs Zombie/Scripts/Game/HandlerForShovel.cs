using UnityEngine;
using System.Collections;

public class HandlerForShovel : MonoBehaviour {

    public AudioClip shovelLift;
    public AudioClip shovelCancel;
    public GameObject shovelBg;

    private GameObject shovel;
    private GameObject selectedPlant;

    void Update() {
        HandleMouseMoveForShovel();
        HandleMouseDownForShovel();
    }

    void HandleMouseDownForShovel() {
        if (Input.GetMouseButtonDown(0)) {

            if (shovelBg.GetComponent<Collider2D>().OverlapPoint(Utility.GetMouseWorldPos())) {
                CancelSelectedShovel();
                shovel = shovelBg.GetComponent<Shovel>().shovel.gameObject;
                shovelBg.SendMessage("OnSelect");
                AudioManager.GetInstance().PlaySound(shovelLift);
            } else if (shovel) {
                if (selectedPlant) {
                    selectedPlant.GetComponent<PlantGrow>().Sell();
                    selectedPlant = null;
                } else {
                    CancelSelectedShovel();
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            CancelSelectedShovel();
        }
    }

    void HandleMouseMoveForShovel() {
        if (shovel) {
            Vector3 pos = Utility.GetMouseWorldPos();
            Vector3 shovelPos = pos;
            shovelPos.x += 0.1f;
            shovelPos.y += 0.1f;
            shovel.transform.position = shovelPos;

            if (StageMap.IsPointInMap(pos)) {
                int row, col;
                StageMap.GetRowAndCol(pos, out row, out col);
                GameObject plant = GameModel.GetInstance().map[row, col];
                if (selectedPlant != plant) {
                    if (selectedPlant) {
                        selectedPlant.GetComponent<SpriteDisplay>().SetAlpha(1f);
                    }                
                    
                    if (plant && plant.tag != "UnableSell") {
                        selectedPlant = plant;
                        selectedPlant.GetComponent<SpriteDisplay>().SetAlpha(0.6f);
                    } else {
                        selectedPlant = null;
                    }            
                }
            }
        }
    }

    void CancelSelectedShovel() {
        if (shovel) {
            shovelBg.GetComponent<Shovel>().CancelSelected();
            shovel = null;
            AudioManager.GetInstance().PlaySound(shovelCancel);
            if (selectedPlant) {
                selectedPlant.GetComponent<SpriteDisplay>().SetAlpha(1f);
                selectedPlant = null;
            }
        }
    }
}
