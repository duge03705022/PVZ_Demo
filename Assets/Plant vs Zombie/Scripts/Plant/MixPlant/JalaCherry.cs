using UnityEngine;
using System.Collections;

public class JalaCherry : MonoBehaviour
{
    // Cherry
    public AudioClip explodeSound1;
    public GameObject effect1;
    public Vector3 effectOffset1;
    public float explodeRange1;

    // Jalapeno
    public AudioClip explodeSound2;
    public GameObject effect2;

    public float delayTime;

    public GameObject hole;
    protected PlantGrow grow;

    void AfterGrow()
    {
        transform.Find("plant").GetComponent<Animator>().Rebind();
        grow = GetComponent<PlantGrow>();
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(delayTime);

        // Cherry Effect
        GameObject newEffect = Instantiate(effect1);
        newEffect.transform.position = transform.position + effectOffset1;
        newEffect.GetComponent<SpriteRenderer>().sortingOrder =
            transform.Find("plant").GetComponent<SpriteRenderer>().sortingOrder + 1;
        Destroy(newEffect, 1.5f);

        // Jalapeno Effect
        GameObject newEffect2 = Instantiate(effect2);
        newEffect2.transform.position = new Vector3(1.8f, transform.position.y + 0.5f, 0);
        newEffect2.GetComponent<SpriteRenderer>().sortingOrder =
            transform.Find("plant").GetComponent<SpriteRenderer>().sortingOrder + 1;
        Destroy(newEffect2, 1.2f);

        // Cherry Damage
        SearchZombie search = GetComponent<SearchZombie>();
        foreach (GameObject zombie in search.SearchZombiesInRange(explodeRange1))
        {
            zombie.GetComponent<ZombieHealthy>().BoomDie();
        }

        // Jalapeno Damage
        GameModel model = GameModel.GetInstance();
        int row = GetComponent<PlantGrow>().row;
        object[] zombies = model.zombieList[row].ToArray();
        foreach (GameObject zombie in zombies)
        {
            zombie.GetComponent<ZombieHealthy>().BoomDie();
        }

        AudioManager.GetInstance().PlaySound(explodeSound1);
        AudioManager.GetInstance().PlaySound(explodeSound2);
        GetComponent<PlantHealthy>().Die();

        // Create hole
        GameObject tempPlant = Instantiate(hole);
        tempPlant.transform.position = StageMap.GetPlantPos(grow.row, grow.col);
        tempPlant.GetComponent<PlantGrow>().grow(grow.row, grow.col);
    }
}
