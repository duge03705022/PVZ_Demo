using UnityEngine;
using System.Collections;

public class Jalapeno : MonoBehaviour {

    public AudioClip explodeSound;
    public GameObject effect;
    public float delayTime;

    public GameObject hole;
    protected PlantGrow grow;

    void AfterGrow() {
        transform.Find("plant").GetComponent<Animator>().Rebind();
        grow = GetComponent<PlantGrow>();
        StartCoroutine(Explode());
    }

    IEnumerator Explode() {
        yield return new WaitForSeconds(delayTime);

        GameObject newEffect = Instantiate(effect);
        newEffect.transform.position = new Vector3(1.8f, transform.position.y + 0.5f, 0);
        newEffect.GetComponent<SpriteRenderer>().sortingOrder =
            transform.Find("plant").GetComponent<SpriteRenderer>().sortingOrder + 1;
        Destroy(newEffect, 1.2f);

        GameModel model = GameModel.GetInstance();
        int row = GetComponent<PlantGrow>().row;
        object[] zombies = model.zombieList[row].ToArray();
        foreach (GameObject zombie in zombies) {
            zombie.GetComponent<ZombieHealthy>().BoomDie();
        }

        AudioManager.GetInstance().PlaySound(explodeSound);

        GetComponent<PlantHealthy>().Die();

        // Create hole
        GameObject tempPlant = Instantiate(hole);
        tempPlant.transform.position = StageMap.GetPlantPos(grow.row, grow.col);
        tempPlant.GetComponent<PlantGrow>().grow(grow.row, grow.col);
    }
}
