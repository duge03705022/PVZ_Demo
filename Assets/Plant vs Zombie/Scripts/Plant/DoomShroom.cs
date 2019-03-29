using UnityEngine;
using System.Collections;

public class DoomShroom : MonoBehaviour {

    public AudioClip explodeSound;
    public GameObject crater;
    public GameObject effect;
    public Vector3 effectOffset;
    public float delayTime;

    void AfterGrow() {
        transform.Find("plant").GetComponent<Animator>().Rebind();
        StartCoroutine(Explode());
    }

    IEnumerator Explode() {
        yield return new WaitForSeconds(delayTime);

        GameObject newEffect = Instantiate(effect);
        newEffect.transform.position = transform.position + effectOffset;
        newEffect.GetComponent<SpriteRenderer>().sortingOrder =
            transform.Find("plant").GetComponent<SpriteRenderer>().sortingOrder + 1;
        Destroy(newEffect, 1.5f);

        GameModel model = GameModel.GetInstance();
        for (int row = 0; row < StageMap.ROW_MAX; ++row) {
            object[] zombies = model.zombieList[row].ToArray();
            foreach (GameObject zombie in zombies) {
                zombie.GetComponent<ZombieHealthy>().BoomDie();
            }
        }

        GetComponent<PlantHealthy>().Die();
        AudioManager.GetInstance().PlaySound(explodeSound);

        GameObject newCrater = Instantiate(crater);
        newCrater.transform.position = transform.position;
        PlantGrow grow = GetComponent<PlantGrow>();
        newCrater.GetComponent<PlantGrow>().grow(grow.row, grow.col);
    }
}
