using UnityEngine;
using System.Collections;

public class JalapenoCol : MonoBehaviour {

    public AudioClip explodeSound;
    public GameObject effect;
    public Vector3 effectOffset;
    public float explodeRange;
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
        newEffect.transform.position = transform.position + effectOffset;
        newEffect.GetComponent<SpriteRenderer>().sortingOrder =
            transform.Find("plant").GetComponent<SpriteRenderer>().sortingOrder + 1;
        Destroy(newEffect, 1.5f);

        SearchZombie search = GetComponent<SearchZombie>();
        foreach (GameObject zombie in search.SearchZombiesInCol()) {
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
