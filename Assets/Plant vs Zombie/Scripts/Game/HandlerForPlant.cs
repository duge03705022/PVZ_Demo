using UnityEngine;
using System.Collections;

public class HandlerForPlant : MonoBehaviour
{

    public AudioClip seedLift;
    public AudioClip seedCancel;
    public AudioClip plantGrow;

    public GameObject[] plantsForPlayer;
    public GameObject[] serror;
    public int[] stackSensing;
    public int[] stackerror; //0, 1

    private bool[] hasPlant;
    private bool[] resetr;
    private GameObject tempPlant;

    private GameObject tempError;
    private int[] ErrorTypesOnMap; // 0, 1, 2, 3
    private GameObject[] ErrorOnMap;

    private GameModel model;

    public Animator money;

    //private int row = -1;
    //private int col = -1;

    void Start()
    {
        model = GameModel.GetInstance();

        ErrorTypesOnMap = new int[StageMap.MAP_MAX];
        ErrorOnMap = new GameObject[StageMap.MAP_MAX];
        hasPlant = new bool[StageMap.MAP_MAX];
        resetr = new bool[StageMap.MAP_MAX];
        for (int i = 0; i < resetr.Length; i++)
        {
            hasPlant[i] = false;
            resetr[i] = true;
        }

    }

    void Update()
    {
        updatePlantsOnMap();
        updateErrorOnMap();
    }

    void updatePlantsOnMap()
    {
        for (int i = 0; i < StageMap.MAP_MAX; i++)
        {
            int x = i / StageMap.COL_MAX;
            int y = i % StageMap.COL_MAX;

            if (stackSensing[i] != 0 && model.map[x, y] == null && !hasPlant[i])
            {
                if (model.sun - 150 >= 0 || (stackSensing[i] == 12 && model.sun - 50 >= 0))
                {
                    if (stackSensing[i] == 12)
                        model.sun -= 50;
                    else
                        model.sun -= 150;
                    tempPlant = Instantiate(plantsForPlayer[stackSensing[i] - 1]);
                    tempPlant.transform.position = StageMap.GetPlantPos(i / StageMap.COL_MAX, i % StageMap.COL_MAX);
                    tempPlant.GetComponent<PlantGrow>().grow(x, y);

                    AudioManager.GetInstance().PlaySound(plantGrow);
                    tempPlant = null;
                    hasPlant[i] = true;
                }
                else
                    money.SetTrigger("nomoney");
            }
            else if (stackSensing[i] == 0 && hasPlant[i] && !model.holeMap[x, y])
            {
                GameObject tempPlant = model.map[x, y];
                tempPlant.GetComponent<PlantHealthy>().Die();
                hasPlant[i] = false;
            }
            else if (stackSensing[i] == 0 && hasPlant[i] && model.holeMap[x, y])
            {
                GameObject tempPlant = model.map[x, y];
                model.map[x, y] = null;
                Destroy(tempPlant);
                hasPlant[i] = false;
            }
        }
    }


    void updateErrorOnMap()
    {
        for (int i = 0; i < ErrorOnMap.Length; i++)
        {
            if (ErrorOnMap[i] == null)
            {
                ErrorTypesOnMap[i] = 0;
            }
            if (ErrorTypesOnMap[i] == 0 && stackerror[i] == 0 && !resetr[i])
                resetr[i] = true;

            if (ErrorTypesOnMap[i] != stackerror[i])
            {
                if (stackerror[i] == 0)
                {
                    ErrorOnMap[i].GetComponent<errordie>().Die();
                    ErrorTypesOnMap[i] = stackerror[i];
                    resetr[i] = true;
                }
                else
                {
                    if (ErrorTypesOnMap[i] != 0)
                    {
                        ErrorOnMap[i].GetComponent<errordie>().Die();
                        resetr[i] = true;
                    }

                    if (resetr[i])
                    {
                        tempError = Instantiate(serror[stackerror[i] - 1]);
                        tempError.transform.position = StageMap.GetErrorIconPos(i / StageMap.COL_MAX, i % StageMap.COL_MAX);
                        tempError.GetComponent<errordie>().grow(i / StageMap.COL_MAX, i % StageMap.COL_MAX);
                        ErrorOnMap[i] = tempError;
                        tempError = null;

                        ErrorTypesOnMap[i] = stackerror[i];
                        resetr[i] = false;

                    }
                }
            }
        }
    }
    //public void stackcost()
    //{
    //    if (model.sun - 150 >= 0)
    //        model.sun -= 150;
    //    else
    //        money.SetTrigger("nomoney");
    //}
}
