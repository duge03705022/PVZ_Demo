using UnityEngine;
using System.Collections;

public class ZombieMove : MonoBehaviour {

    public AudioClip groanSound;
    public float speed = 0.5f;
    [HideInInspector]
    public int row;
//	public int col;

    protected GameModel model;
    protected ZombieSpriteDisplay display;
    protected AbnormalState state;

    protected void Awake() {
        model = GameModel.GetInstance();
        display = GetComponent<ZombieSpriteDisplay>();
        state = GetComponent<AbnormalState>();
        Invoke("Groan", Random.Range(5f, 10f));
    }

    void Update() {
        transform.Translate(-speed * Time.deltaTime * state.ratio, 0, 0);
//		StageMap.GetRowAndCol (this.transform.position, out row, out col);
	}

    protected void Groan() {
        AudioManager.GetInstance().PlaySound(groanSound);
        Invoke("Groan", Random.Range(5f, 10f));
    }

    public void ChangeRow(bool upward) {
        MoveBy move = gameObject.AddComponent<MoveBy>();
        Vector3 offset = new Vector3(0, StageMap.GRID_HEIGHT, 0);
        if (!upward) offset.y = -offset.y;
        move.offset = offset;
        move.time = 0.5f;
        move.Begin();

        model.zombieList[row].Remove(gameObject);
        if (upward) {
            ++row;
        } else {
            --row;
        }
        model.zombieList[row].Add(gameObject);
        display.SetOrderByRow(row);
    }
}
