using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SearchZombie))]
public class Bullet : MonoBehaviour {

    public GameObject effect;
    public int atk;
    public float speed;
    public float range;
    public bool rightward = true;

    protected GameModel model;
    protected SearchZombie search;
    protected GameObject target;

    protected int _row;
    public int row {
        get { return _row; }
        set {
            _row = value;
            if (0 <= _row && _row < StageMap.ROW_MAX) {
                model.bulletList[_row].Add(gameObject);
            }
        }
    }

    void Awake() {
        model = GameModel.GetInstance();
        search = GetComponent<SearchZombie>();
        _row = -1;
    }

	void Update () {
        transform.Translate(speed * Time.deltaTime, 0, 0);

        if (_row < 0 || StageMap.ROW_MAX <= _row) {
            target = null;
        } else if (rightward) {
            target = search.SearchClosetZombie(_row, 0, range);
        } else {
            target = search.SearchClosetZombie(range);
        }

        if (target) {
            target.GetComponent<ZombieHealthy>().Damage(atk);
            HitEffect();
        }

        if (!Camera.main.pixelRect.Contains(Camera.main.WorldToScreenPoint(transform.position))) {
            HitEffect();
        }
	}

    protected virtual void HitEffect() {
        if (effect) {
            GameObject newEffect = Instantiate(effect);
            newEffect.transform.position = transform.position;
            Destroy(newEffect, 0.2f);
        }

        DoDestroy();
    }

    public void DoDestroy() {
        if (0 <= _row && _row < StageMap.ROW_MAX) {
            model.bulletList[row].Remove(gameObject);
        }
        Destroy(gameObject);
    }

    public void Reverse() {
        rightward = !rightward;
        transform.Rotate(0, 180, 0);
    }

    public void TurnUp()
    {
        rightward = !rightward;
        transform.Rotate(0, 180, 90);
    }
    public void TurnDown()
    {
        rightward = !rightward;
        transform.Rotate(0, 180, -90);
    }
}
