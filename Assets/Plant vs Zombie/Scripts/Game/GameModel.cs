using UnityEngine;
using System.Collections;

public class GameModel {

    public GameObject[,] map;
    public ArrayList[] zombieList;
    public ArrayList[] bulletList;
    public GameObject[,] sunMap;
    public GameObject[,] plantSunMap;
    public GameObject[,] errorIconMap;
	public GameObject[,] LVIconMap;
    public int sun;
    public bool inDay;

    public bool[,] holeMap;
    public int[,] plantLevel;
    public GameObject[,] levelEffect;

    public readonly string[] bombName = {
        "Jalapeno(Clone)",
        "JalapenoCol(Clone)",
        "JalapenoCross(Clone)",
        "JalaCherry(Clone)",
        "CherryBomb(Clone)",
        "CherryBombM(Clone)",
        "CherryBombL(Clone)"};

    private GameModel() {
        Clear();
    }

    public void Clear() {
        map = new GameObject[StageMap.ROW_MAX, StageMap.COL_MAX];
        sunMap = new GameObject[StageMap.ROW_MAX, StageMap.COL_MAX];
        plantSunMap = new GameObject[StageMap.ROW_MAX, StageMap.COL_MAX];
        errorIconMap = new GameObject[StageMap.ROW_MAX, StageMap.COL_MAX];
		LVIconMap = new GameObject[StageMap.ROW_MAX, StageMap.COL_MAX];
        holeMap = new bool[StageMap.ROW_MAX, StageMap.COL_MAX];

        plantLevel = new int[StageMap.ROW_MAX, StageMap.COL_MAX];
        levelEffect = new GameObject[StageMap.ROW_MAX, StageMap.COL_MAX];
        for (int i = 0; i < StageMap.ROW_MAX; i++)
        {
            for (int j = 0; j < StageMap.COL_MAX; j++)
            {
                plantLevel[i, j] = 0;
                holeMap[i, j] = false;
            }
        }

        zombieList = new ArrayList[StageMap.ROW_MAX];
        bulletList = new ArrayList[StageMap.ROW_MAX];
        for (int i = 0; i < StageMap.ROW_MAX; ++i) {
            zombieList[i] = new ArrayList();
            bulletList[i] = new ArrayList();
        }
    }

    private static GameModel instance;
    public static GameModel GetInstance() {
        if (instance == null)
            instance = new GameModel();
        return instance;
    }
}
