using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SearchZombie))]
public class StartBullet : MonoBehaviour {

    public int atk;
    public float speed;
    public float range;
    [HideInInspector]
    public Vector3 direction;

    private SearchZombie search;
	void Awake () {
        search = GetComponent<SearchZombie>();
	}
	
	void Update () {
        transform.position = transform.position + speed * Time.deltaTime * direction;

        GameObject zombie = search.SearchClosetZombie(range);
        if (zombie) {
            zombie.GetComponent<ZombieHealthy>().Damage(atk);
            Destroy(gameObject);
        }

        if (!Camera.main.pixelRect.Contains(Camera.main.WorldToScreenPoint(transform.position))) {
            Destroy(gameObject);
        }
	}
}
