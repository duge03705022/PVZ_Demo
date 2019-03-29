using UnityEngine;
using System.Collections;
using System.Linq;

public class HandlerForTouch : MonoBehaviour
{
    public bool[] touchSensing;
    public GameObject boomEffect;
    public AudioClip explodeSound;

    public GameObject[] effectLevel;

    private GameModel model;
    private int tempX, tempY;
    //private Vector3 tempTouchPos;
    private Sun tempSun;

    private int zombieTempRow, zombieTempCol;
    private ZombieHealthy tempZombie;
    //private SearchZombie search;


    public GameObject[] LVobj;
    public HandlerForPlant planthandler;
    private GameObject tmp;
    //private GameObject[] LVOnMap;
    private bool[] touchDown;
    private int[] touchCount;
    private bool[] showLevel;

    // Use this for initialization
    void Start()
    {
        model = GameModel.GetInstance();
        //search = GetComponent<SearchZombie>();
        //LVOnMap = new GameObject[StageMap.MAP_MAX];

        touchDown = new bool[touchSensing.Length];
        touchCount = new int[touchSensing.Length];
        showLevel = new bool[touchSensing.Length];
        for (int i = 0; i < touchSensing.Length; i++)
        {
            touchDown[i] = false;
            touchCount[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < touchSensing.Length; i++)
        {
            if (touchSensing[i])
            {
                tempX = i / StageMap.COL_MAX;
                tempY = i % StageMap.COL_MAX;
                //tempTouchPos = StageMap.GetTouchPos(tempX, tempY);

                if (!touchDown[i])
                {
                    touchDown[i] = true;
                }
                else
                {
                    touchCount[i] += 1;
                }

                if (touchDown[i] && touchCount[i] > 50 && !showLevel[i])
                {
                    showLevel[i] = true;
                    HoldLevel(tempX, tempY, model.plantLevel[tempX, tempY]);
                }

                //touch to show LV
                //				if (planthandler.stackSensing [i] == 1 && LVOnMap[i] == null) {
                //					Debug.Log ("1");
                //					tmp = Instantiate (LVobj [0]);
                //					tmp.transform.position = StageMap.GetLVIconPos (i / StageMap.COL_MAX, i % StageMap.COL_MAX);
                //					tmp.GetComponent<showlv> ().grow (i / StageMap.COL_MAX, i % StageMap.COL_MAX);
                //					LVOnMap[i] = tmp;
                //				} else if (planthandler.stackSensing [i] == 2 && LVOnMap[i] == null) {
                //					tmp = Instantiate (LVobj [1]);
                //					tmp.transform.position = StageMap.GetLVIconPos (i / StageMap.COL_MAX, i % StageMap.COL_MAX);
                //					tmp.GetComponent<showlv> ().grow (i / StageMap.COL_MAX, i % StageMap.COL_MAX);
                //					LVOnMap[i] = tmp;
                //				} else if (planthandler.stackSensing [i] == 3 && LVOnMap[i] == null) {
                //					tmp = Instantiate (LVobj [2]);
                //					tmp.transform.position = StageMap.GetLVIconPos (i / StageMap.COL_MAX, i % StageMap.COL_MAX);
                //					tmp.GetComponent<showlv> ().grow (i / StageMap.COL_MAX, i % StageMap.COL_MAX);
                //					LVOnMap[i] = tmp;
                //				}
                
                // boom zombie
                //if (planthandler.stackSensing[i] == 10 && model.map[tempX, tempY] != null && false)
                //{
                //    GameObject newEffect = Instantiate(boomEffect);
                //    newEffect.transform.position = tempTouchPos;
                //    foreach (GameObject zombie in search.SearchZombiesInRange(tempTouchPos, tempX, 0.5f))
                //    {
                //        tempZombie = zombie.GetComponent<ZombieHealthy>();
                //        if (!tempZombie.isBoomDie)
                //        {
                //            //							GameObject newEffect = Instantiate (boomEffect);
                //            //							newEffect.transform.position = tempTouchPos;
                //            newEffect.GetComponent<SpriteRenderer>().sortingOrder =
                //            tempZombie.transform.Find("zombie").GetComponent<SpriteRenderer>().sortingOrder + 1;
                //            //							ScaleUp scaleup = newEffect.AddComponent<ScaleUp> ();
                //            //							scaleup.Begin ();
                //            //							AudioManager.GetInstance ().PlaySound (explodeSound);
                //            //							Destroy (newEffect, 1.0f);
                //            tempZombie.BoomDie();
                //            //bumbzombie ();
                //        }
                //    }
                //    ScaleUp scaleup = newEffect.AddComponent<ScaleUp>();
                //    scaleup.Begin();
                //    AudioManager.GetInstance().PlaySound(explodeSound);
                //    Destroy(newEffect, 1.0f);
                //    model.map[tempX, tempY].GetComponent<PlantHealthy>().Die();
                //}
            }
            else
            {
                if (touchDown[i])
                {
                    if (touchCount[i] < 50) 
                    {
                        Touch(tempX, tempY);
                    }
                    else
                    {
                        DestroyHoldLevel(tempX, tempY);
                        showLevel[i] = false;
                    }

                    touchDown[i] = false;
                    touchCount[i] = 0;
                }

                //if (LVOnMap[i] != null)
                //{
                //    Debug.Log("2");
                //    LVOnMap[i].GetComponent<showlv>().Die();
                //}
            }
        }
    }
    public void Bumbzombie()
    {
        model.sun -= 50;
    }

    public void Touch(int x, int y)
    {
        // pick sun
        if (model.sunMap[x, y] != null)
        {
            tempSun = model.sunMap[x, y].GetComponent<Sun>();
            if (!tempSun.isCollected)
                tempSun.pickUp();
        }
        else if (model.plantSunMap[tempX, tempY] != null)
        {
            model.map[tempX, tempY].GetComponent<PlantSun>().Die();
            model.plantSunMap[tempX, tempY] = null;
        }
        else
        {
            if (model.map[x, y] != null)
            {
                if (model.bombName.Contains(model.map[x, y].name))
                {
                    Animator[] childAnimators = model.map[x, y].GetComponentsInChildren<Animator>();
                    for (int i = 0; i < childAnimators.Length; i++)
                    {
                        childAnimators[i].enabled = true;
                    }
                    model.map[x, y].SendMessage("AfterGrow");
                }
            }
        }
    }

    public void HoldLevel(int x, int y, int level)
    {
        if (level >= 0)
        {
            Vector3 touchPos = StageMap.GetTouchPos(x, y);

            GameObject newEffect = Instantiate(effectLevel[level]);

            Vector3 effectOffset;
            if (level == 0)
                effectOffset = new Vector3(0f, -0.1f, 0f);
            else
                effectOffset = new Vector3(1f, 0f, 0f);

            newEffect.transform.position = touchPos + effectOffset;
            model.levelEffect[x, y] = newEffect;
        }
    }

    public void DestroyHoldLevel(int x, int y)
    {
        GameObject newEffect = model.levelEffect[x, y];
        Destroy(newEffect, 0.5f);
    }


    public void ErrorEffectOn(int x, int y)
    {
        Vector3 touchPos = StageMap.GetTouchPos(x, y);
        GameObject newEffect = Instantiate(effectLevel[0]);
        Vector3 effectOffset = new Vector3(0f, -0.1f, 0f);

        newEffect.transform.position = touchPos + effectOffset;
        model.levelEffect[x, y] = newEffect;
        //Destroy(newEffect, 2f);
    }

    public void ErrorEffectOff(int x, int y)
    {
        GameObject newEffect = model.levelEffect[x, y];
        Destroy(newEffect);
    }
}
