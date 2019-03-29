using UnityEngine;
using System.Collections;

public enum ZombieType {
    Zombie1,
    Zombie2,
    FlagZombie,
    ConeheadZombie,
    BucketheadZombie,
    NewspaperZombie,
    PoleVaultingZombie
}

[System.Serializable]
public struct Wave {
    [System.Serializable]
    public struct Data {
        public ZombieType zombieType;
        public uint count;
    }

    public bool isLargeWave;
    [Range(0f, 1f)]
    public float percentage;
    public Data[] zombieData;
}

public class GameController : MonoBehaviour {
	public Animator hit;
	public GameObject heart1, heart2, heart3;
	int counthit = 0;
    [Space(10)]
    public AudioClip bgmMusic;

    [Space(10)]
    public AudioClip loseMusic;
    public AudioClip winMusic;
    public AudioClip readySound;
    public AudioClip zombieComing;
    public AudioClip hugeWaveSound;
    public AudioClip finalWaveSound;  

    [Space(10)]
    public GameObject Zombie1;
    public GameObject Zombie2;
    public GameObject FlagZombie;
    public GameObject ConeheadZombie;
    public GameObject BucketheadZombie;
    public GameObject NewspaperZombie;
    public GameObject PoleVaultingZombie;

    [Space(10)]
    public GameObject gameLabel;
    public GameObject sunLabel;

    [Space(10)]
    public GameObject sunPrefab;
    public float sunInterval;

    [Space(10)]
    public int initSun;
    public bool inDay = true;

    [Space(10)]
    public Wave[] waves;

    [Space(10)]
    public float readyTime;
    public float playTime;

    private GameModel model;
    private float elapsedTime = 0;
    private bool hasLostGame = false;
	private int moneycount = 0;
    private int i = 0;
	//private bool tag = false;

    void Awake() {
        model = GameModel.GetInstance();
    }

    void Start() {
        model.Clear();
        model.sun = initSun;
        model.inDay = inDay;

        ArrayList flags = new ArrayList();
        for (int i = 0; i < waves.Length; ++i) {
            if (waves[i].isLargeWave) {
                flags.Add(waves[i].percentage);
            }
        }      

        sunLabel.SetActive(false);

        GetComponent<HandlerForPlant>().enabled = false;
		GetComponent<HandlerForTouch>().enabled = false;

        AudioManager.GetInstance().PlayMusic(bgmMusic);
    }

	void Update () {
        if (Input.GetKeyDown("space"))
        {
            StartGame();//
        }

        if (!hasLostGame) {
            for (int row = 0; row < model.zombieList.Length; ++row) {
                foreach (GameObject zombie in model.zombieList[row]) {
                    if (zombie.transform.position.x < (StageMap.GRID_LEFT - 0.5f)) {
						model.zombieList[row].Remove(zombie);
						Destroy (zombie);
						hit.SetTrigger ("hit");
						if (counthit == 0) {
							Destroy (heart1);
							counthit++;
						} else if (counthit == 1) {
							Destroy (heart2);
							counthit++;
						} else if (counthit == 2) {
							Destroy (heart3);
							counthit++;
							LoseGame();
							hasLostGame = true;
							return;
						}
                        
                    }
                }
            }
        } else if (Input.GetMouseButtonDown(0)) {
            GameObject.Find("btn_menu")
                .GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
        }
	}

    public void StartGame() {
        GetComponent<HandlerForPlant>().enabled = true;
		GetComponent<HandlerForTouch>().enabled = true;
		sunLabel.SetActive(true);

        StartCoroutine(Workflow());
        if (inDay) {
            InvokeRepeating("ProduceSun", 8000f, 15f);
        }
    }

    IEnumerator Workflow() {
        gameLabel.GetComponent<GameTips>().ShowStartTip();
        AudioManager.GetInstance().PlaySound(readySound);
        yield return new WaitForSeconds(readyTime);

		StartCoroutine("UpdateProgress");
        AudioManager.GetInstance().PlaySound(zombieComing);

        for (int i = 0; i < waves.Length; ++i) {
            yield return StartCoroutine(WaitForWavePercentage(waves[i].percentage));

            if (waves[i].isLargeWave) {
                StopCoroutine("UpdateProgress");
                yield return StartCoroutine(WaitForZombieClear());
                yield return new WaitForSeconds(3.0f);

                gameLabel.GetComponent<GameTips>().ShowApproachingTip();
                AudioManager.GetInstance().PlaySound(hugeWaveSound);

                yield return new WaitForSeconds(3.0f);
                StartCoroutine("UpdateProgress");
            }
            if (i + 1 == waves.Length) {
                //gameLabel.GetComponent<GameTips>().ShowFinalTip();
                AudioManager.GetInstance().PlaySound(finalWaveSound);
            }

            CreateZombies(ref waves[i]);
        }

        yield return StartCoroutine(WaitForZombieClear());
        yield return new WaitForSeconds(3.0f);
        WinGame();
    }

    IEnumerator UpdateProgress() {
        while (true) {
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
    }

    void CreateZombies(ref Wave wave) {
        foreach (Wave.Data data in wave.zombieData) {
            for (int i = 0; i < data.count; ++i) {
                CreateOneZombie(data.zombieType);
            }
        }
    }

    void CreateOneZombie(ZombieType type) {
        GameObject zombie;
        switch (type) {
            case ZombieType.Zombie1:
                zombie = Instantiate(Zombie1);
                break;
            case ZombieType.Zombie2:
                zombie = Instantiate(Zombie2);
                break;
            case ZombieType.FlagZombie:
                zombie = Instantiate(FlagZombie);
                break;
            case ZombieType.ConeheadZombie:
                zombie = Instantiate(ConeheadZombie);
                break;
            case ZombieType.BucketheadZombie:
                zombie = Instantiate(BucketheadZombie);
                break;
            case ZombieType.PoleVaultingZombie:
                zombie = Instantiate(PoleVaultingZombie);
                break;
            case ZombieType.NewspaperZombie:
                zombie = Instantiate(NewspaperZombie);
                break;
            default:
                throw new System.Exception("Wrong zombie type");
        }
        int row = Random.Range(0, StageMap.ROW_MAX - 1);
        zombie.transform.position = StageMap.GetZombiePos(row);
		zombie.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
        zombie.GetComponent<ZombieMove>().row = row;
        zombie.GetComponent<SpriteDisplay>().SetOrderByRow(row);
        model.zombieList[row].Add(zombie);
    }

    IEnumerator WaitForZombieClear() {
        while (true) {
            bool hasZombie = false;
            for (int row = 0; row < StageMap.ROW_MAX; ++row) {
                if (model.zombieList[row].Count != 0) {
                    hasZombie = true;
                    break;
                }
            }
            if (hasZombie) {
                yield return new WaitForSeconds(0.1f);
            } else {
                break;
            }
        }           
    }

    IEnumerator WaitForWavePercentage(float percentage) {
        while (true) {
            if ((elapsedTime / playTime) >= percentage) {
                break;
            } else {
                yield return 0;
            }
        }
    }

    void ProduceSun() {
		if (moneycount <= 0) {
            //int x = Random.Range(0, StageMap.ROW_MAX);
            //int y = Random.Range(0, StageMap.COL_MAX);
            //int y = Random.Range(4, 7);
            int[] x_array = new int[5] { 0, 2, 3, 1, 4 };
            int[] y_array = new int[5] { 2, 8, 3, 0, 5 };

            if (i < 5)
            {
                int x = x_array[i];
                int y = y_array[i];

                if (model.sunMap[x, y] == null)
                {
                    GameObject sun = Instantiate(sunPrefab);
                    sun.transform.position = StageMap.GetSunPos(x, y);

                    ScaleUp scaleup = sun.AddComponent<ScaleUp>();
                    scaleup.Begin();
                    model.sunMap[x, y] = sun;
                }

                i++;
            }

            //int x = Random.Range(0, StageMap.ROW_MAX);
            //int y = Random.Range(0, StageMap.COL_MAX);
            //if (model.sunMap[x, y] == null)
            //{
            //    GameObject sun = Instantiate(sunPrefab);
            //    sun.transform.position = StageMap.GetSunPos(x, y);

            //    ScaleUp scaleup = sun.AddComponent<ScaleUp>();
            //    scaleup.Begin();
            //    model.sunMap[x, y] = sun;
            //}
        }
    }

    void LoseGame() {
        gameLabel.GetComponent<GameTips>().ShowLostTip();
        GetComponent<HandlerForPlant>().enabled = false;
        CancelInvoke("ProduceSun");
        AudioManager.GetInstance().PlayMusic(loseMusic, false);
    }

    void WinGame() {
        CancelInvoke("ProduceSun");
        AudioManager.GetInstance().PlayMusic(winMusic, false);
	}
}
