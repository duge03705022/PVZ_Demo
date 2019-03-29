using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Card : MonoBehaviour {

    public enum State {
        NORMAL = 0,
        SELECTED = 1 << 0,
        NO_SUN = 1 << 1,
        CD = 1 << 2
    }

    public Sprite enableSprite;
    public Sprite disableSprite;
    public int price;
    public float cd;
    public GameObject plant;
    [HideInInspector]
    public State state;

    private GameModel model;
    private HandlerForPlant plantHandler;
    //private SpriteRenderer renderer;
    private float cdTime = 0;
    public float CdTime { get { return cdTime; } }

	void Awake () {
        model = GameModel.GetInstance();
        plantHandler = GameObject.Find("GameController").GetComponent<HandlerForPlant>();
        //renderer = GetComponent<SpriteRenderer>();
        state = State.NORMAL;
        //renderer.sprite = enableSprite;
        plant.GetComponent<PlantGrow>().price = price;
	}
	
	void Update () {
        UpdateUI();
        if ((state & State.CD) != 0) {
            cdTime -= Time.deltaTime;
            if (cdTime <= 0) {
                state &= ~State.CD;
            }
        }
	}

    public void OnSelect() {
        if (state == State.NORMAL) {
//            plantHandler.SetSelectedCard(this);
        }       
    }

    void UpdateUI() {
        CheckSun();
        if (state == State.NORMAL) {
            //renderer.sprite = enableSprite;
        } else {
            //renderer.sprite = disableSprite;
        }
    }

    void CheckSun() {
        if (model.sun < price) {
            state |= State.NO_SUN;
        } else {
            state &= ~State.NO_SUN;
        }
    }

    public void Select() {
        state |= State.CD;
        state &= ~State.SELECTED;
        cdTime = cd;
        model.sun -= price;
        UpdateUI();
    }

    public void SetSprite(bool enable) {
        if (enable) {
            //renderer.sprite = enableSprite;
        } else {
            //renderer.sprite = disableSprite;
        }
    }
}
