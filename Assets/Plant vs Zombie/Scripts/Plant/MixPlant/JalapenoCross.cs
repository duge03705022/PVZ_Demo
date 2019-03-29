using UnityEngine;
using System.Collections;

public class JalapenoCross : MonoBehaviour {

    public AudioClip explodeSound;
    public GameObject effectC;
    public GameObject effectR;
    public Vector3 effectOffset;
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

        // col effect
        GameObject newEffect1 = Instantiate(effectC);
        newEffect1.transform.position = transform.position + effectOffset;
        newEffect1.GetComponent<SpriteRenderer>().sortingOrder =
            transform.Find("plant").GetComponent<SpriteRenderer>().sortingOrder + 1;
        Destroy(newEffect1, 1.5f);

        // row effect
        GameObject newEffect2 = Instantiate(effectR);
        newEffect2.transform.position = new Vector3(1.8f, transform.position.y + 0.5f, 0);
        newEffect2.GetComponent<SpriteRenderer>().sortingOrder =
            transform.Find("plant").GetComponent<SpriteRenderer>().sortingOrder + 1;
        Destroy(newEffect2, 1.2f);

        // col damage
        SearchZombie search = GetComponent<SearchZombie>();
        foreach (GameObject zombie in search.SearchZombiesInCol()) {
            zombie.GetComponent<ZombieHealthy>().BoomDie();
        }

        // row damage
        GameModel model = GameModel.GetInstance();
        int row = GetComponent<PlantGrow>().row;
        object[] zombies = model.zombieList[row].ToArray();
        foreach (GameObject zombie in zombies)
        {
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
