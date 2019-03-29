using UnityEngine;
using System.Collections;

public class Torchwood : MonoBehaviour {

    public GameObject peaBullet;
    public GameObject fireBullet;
    public float range;

    private GameModel model;
    private PlantGrow grow;

    void Awake() {
        model = GameModel.GetInstance();
        grow = GetComponent<PlantGrow>();
        enabled = false;
    }

    void AfterGrow() {
        enabled = true;
    }
	
	void Update () {
        object[] bullets = model.bulletList[grow.row].ToArray();
        foreach (GameObject bullet in bullets) {
            float dis = transform.position.x - bullet.transform.position.x;
            if (0 < dis && dis <= range) {
                if (bullet.CompareTag("PeaBullet") || bullet.CompareTag("SnowBullet")) {
                    GameObject newBullet;
                    if (bullet.CompareTag("PeaBullet")) {
                        newBullet = Instantiate(fireBullet);
                    } else {
                        newBullet = Instantiate(peaBullet);
                    }

                    Vector3 newPos = bullet.transform.position;
                    newPos.x += dis;
                    newBullet.transform.position = newPos;
                    newBullet.GetComponent<Bullet>().row = bullet.GetComponent<Bullet>().row;
                    newBullet.GetComponent<SpriteRenderer>().sortingOrder
                        = bullet.GetComponent<SpriteRenderer>().sortingOrder;
                    if (!bullet.GetComponent<Bullet>().rightward)
                        newBullet.GetComponent<Bullet>().Reverse();
                    bullet.GetComponent<Bullet>().DoDestroy();
                }
            }
        }
	}
}
