using UnityEngine;
using System.Collections;

public class IceShroom : MonoBehaviour {

    public AudioClip explodeSound;
    public GameObject effect;
    public Vector3 effectOffset;
    public int atk;
    public float frozenTime;
    public float speedDownTime;
    public float delayTime;

    void AfterGrow() {
        transform.Find("plant").GetComponent<Animator>().Rebind();
        StartCoroutine(Explode());
    }

    IEnumerator Explode() {
        yield return new WaitForSeconds(delayTime);

        GameObject newEffect = Instantiate(effect);
        newEffect.transform.position = transform.position + effectOffset;
        Destroy(newEffect, 1.5f);

        GameModel model = GameModel.GetInstance();
        for (int row = 0; row < StageMap.ROW_MAX; ++row) {
            object[] zombies = model.zombieList[row].ToArray();
            foreach (GameObject zombie in zombies) {
                zombie.GetComponent<ZombieHealthy>().Damage(atk);
                zombie.GetComponent<AbnormalState>().FreezeUp(frozenTime);
                zombie.GetComponent<AbnormalState>().SpeedDown(frozenTime + speedDownTime, 0.5f);
            }
        }

        AudioManager.GetInstance().PlaySound(explodeSound);
        GetComponent<PlantHealthy>().Die();
    }
}
