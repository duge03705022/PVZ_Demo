using UnityEngine;
using System.Collections;

public class SunShroom : MonoBehaviour {

    public GameObject littleSun;
    public GameObject normalSun;
    public float produceCd;
    public float growTime;

    private Animator animator;
    private GameObject sun;
    private float cdTime;
    private float curGrowTime = 0;
    private bool hasGrown = false;
    void Awake() {
        animator = transform.Find("plant").GetComponent<Animator>();
        cdTime = produceCd / 4;
        sun = littleSun;
        enabled = false;
    }

    void AfterGrow() {     
        enabled = true;
    }

    void Update() {
        if (cdTime >= 0) {
            cdTime -= Time.deltaTime;
        } else {
            cdTime = produceCd;
            ProduceSun();
        }

        if (curGrowTime < growTime) {
            curGrowTime += Time.deltaTime;
        } else if (!hasGrown) {
            hasGrown = true;
            sun = normalSun;
            animator.SetTrigger("growUp");
            Destroy(GetComponent<PlantSleep>());
        }
    }

    void ProduceSun() {
        GameObject newSun = Instantiate(sun);
        newSun.GetComponent<SpriteRenderer>().sortingOrder = 10000;
        newSun.transform.position = transform.position;

        float dis = StageMap.GRID_WIDTH;
        Vector3 offset = new Vector3(Random.Range(-dis, dis), Random.Range(-dis, dis), 0);
        JumpBy jump = newSun.AddComponent<JumpBy>();
        jump.offset = offset;
        jump.height = Random.Range(0.3f, 0.6f);
        jump.time = Random.Range(0.4f, 0.6f);
        jump.Begin();
    }
}
