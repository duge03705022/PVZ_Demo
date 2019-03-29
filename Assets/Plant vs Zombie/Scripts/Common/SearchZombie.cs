using UnityEngine;
using System.Collections;

public class SearchZombie : MonoBehaviour {

    private GameModel model;
	void Awake () {
        model = GameModel.GetInstance();
	}

    public bool IsZombieInRange(int row, float min, float max) {
        foreach (GameObject zombie in model.zombieList[row]) {
            float dis = zombie.transform.position.x - transform.position.x;
            if (min <= dis && dis <= max) {
                return true;
            }
        }
        return false;
    }

    public bool IsZombieInRange(float range) {
        for (int row = 0; row < StageMap.ROW_MAX; ++row) {
            foreach (GameObject zombie in model.zombieList[row]) {
                float dis = Vector3.Distance(zombie.transform.position, transform.position);
                if (dis <= range) {
                    return true;
                }
            }
        }
        return false;
    }

    public GameObject SearchClosetZombie(int row, float min, float max) {
        float minDis = 10000f;
        GameObject cloestZombie = null;
        foreach (GameObject zombie in model.zombieList[row]) {
            float dis = zombie.transform.position.x - transform.position.x;
            if (min <= dis && dis <= max && Mathf.Abs(dis) < minDis) {
                minDis = Mathf.Abs(dis);
                cloestZombie = zombie;
            }
        }
        return cloestZombie;
    }

    public GameObject SearchClosetZombie(float range) {
        float minDis = 10000f;
        GameObject cloestZombie = null;
        for (int row = 0; row < StageMap.ROW_MAX; ++row) {
            foreach (GameObject zombie in model.zombieList[row]) {
                float dis = Vector3.Distance(zombie.transform.position, transform.position);
                if (dis < range && Mathf.Abs(dis) < minDis) {
                    minDis = Mathf.Abs(dis);
                    cloestZombie = zombie;
                }
            }
        }
        return cloestZombie;
    }

    public object[] SearchZombiesInRange(int row, float min, float max) {
        ArrayList zombies = new ArrayList();
        foreach (GameObject zombie in model.zombieList[row]) {
            float dis = zombie.transform.position.x - transform.position.x;
            if (min <= dis && dis <= max) {
                zombies.Add(zombie);
            }
        }
        return zombies.ToArray();
    }

    public object[] SearchZombiesInRange(float range) {
        ArrayList zombies = new ArrayList();
        for (int row = 0; row < StageMap.ROW_MAX; ++row) {
            foreach (GameObject zombie in model.zombieList[row]) {
                float dis = Vector3.Distance(zombie.transform.position, transform.position);
                if (dis <= range) {
                    zombies.Add(zombie);
                }
            }
        }           
        return zombies.ToArray();
    }

	public object[] SearchZombiesInRange(Vector3 touchPosition, int row, float range) {
		ArrayList zombies = new ArrayList();
		foreach (GameObject zombie in model.zombieList[row]) {
			float dis = Vector3.Distance(zombie.transform.position, touchPosition);
			if (dis <= range) {
				zombies.Add(zombie);
			}
		}
		return zombies.ToArray();
	}

    public object[] SearchZombiesInCol()
    {
        Vector3 tmpPos = transform.position;
        tmpPos.y = 1.78f; // init

        ArrayList zombies = new ArrayList();
        for (int row = 0; row < StageMap.ROW_MAX; ++row)
        {
            foreach (GameObject zombie in model.zombieList[row])
            {
                float dis = Vector3.Distance(zombie.transform.position, tmpPos);
                if (dis <= 0.5f)
                {
                    zombies.Add(zombie);
                }
            }
            tmpPos.y -= 1;
        }
        return zombies.ToArray();
    }
}
