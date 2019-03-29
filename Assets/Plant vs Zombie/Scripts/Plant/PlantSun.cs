using UnityEngine;
using System.Collections;

public class PlantSun : MonoBehaviour {

    public GameObject sun;
    public int sunCount;
    public float produceCd;
    private float cdTime;
	private GameObject newSun;
	private GameModel model;

    protected PlantGrow grow;

    void Awake () {
		model = GameModel.GetInstance();
        cdTime = produceCd;
        grow = GetComponent<PlantGrow>();
        enabled = false;
	}

    void AfterGrow() {
        enabled = true;
    }
	
	void Update () {
        if (cdTime >= 0) {
            cdTime -= Time.deltaTime;
        } else {
            cdTime = produceCd;
			if (newSun == null)
				NProduceSun();
            //ProduceSun();
        }
        Debug.Log(cdTime);
	}

   // void ProduceSun() {
   //     for (int i = 0; i < sunCount; ++i) {
   //         GameObject newSun = Instantiate(sun);
   //         newSun.GetComponent<SpriteRenderer>().sortingOrder = 10000;
   //         newSun.transform.position = transform.position;

			////float dis = StageMap.GRID_WIDTH;
   //         //Vector3 offset = new Vector3(Random.Range(-dis, dis), Random.Range(-dis, dis), 0);

			//Vector3 offset = new Vector3(1, 2, 0);
   //         JumpBy jump = newSun.AddComponent<JumpBy>();
   //         jump.offset = offset;
			//jump.height = 1;//Random.Range(0.3f, 0.6f);
			//jump.time = 1;Random.Range(0.4f, 0.6f);
   //         jump.Begin();
   //     }
   // }   // void ProduceSun() {
   //     for (int i = 0; i < sunCount; ++i) {
   //         GameObject newSun = Instantiate(sun);
   //         newSun.GetComponent<SpriteRenderer>().sortingOrder = 10000;
   //         newSun.transform.position = transform.position;

			////float dis = StageMap.GRID_WIDTH;
   //         //Vector3 offset = new Vector3(Random.Range(-dis, dis), Random.Range(-dis, dis), 0);

			//Vector3 offset = new Vector3(1, 2, 0);
   //         JumpBy jump = newSun.AddComponent<JumpBy>();
   //         jump.offset = offset;
			//jump.height = 1;//Random.Range(0.3f, 0.6f);
			//jump.time = 1;Random.Range(0.4f, 0.6f);
   //         jump.Begin();
   //     }
   // }

	void NProduceSun() {
        float x = grow.row;
        float y = grow.col;
        
        if (model.plantSunMap[(int)x, (int)y] == null)
        {
            for (int i = 0; i < sunCount; ++i)
            {
                Debug.Log("Aaa");
                newSun = Instantiate(sun);
                newSun.GetComponent<SpriteRenderer>().sortingOrder = 10000;
                newSun.transform.position = transform.position;

                model.plantSunMap[(int)x, (int)y] = newSun;

                //float dis = StageMap.GRID_WIDTH;
                //Vector3 offset = new Vector3(Random.Range(-dis, dis), Random.Range(-dis, dis), 0);

                Vector3 offset = new Vector3(
                    Random.Range(-0.2f, 0.2f),
                    Random.Range(0.0f, 0.6f),
                    0);

                JumpBy jump = newSun.AddComponent<JumpBy>();
                jump.offset = offset;
                jump.height = Random.Range(0.3f, 1.0f);
                jump.time = 1;
                jump.Begin();
            }
        }
    }

	public virtual void Die() {
		if (newSun != null) {
			Sun tempSun;
			tempSun = newSun.GetComponent<Sun> ();
			if (!tempSun.isCollected)
				tempSun.pickUp ();
		}
	}

}
