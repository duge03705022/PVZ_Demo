using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class errordie : MonoBehaviour {
	[HideInInspector]
	public int row, col;
	[HideInInspector]
	public int price;

	protected GameModel model;
	protected void Awake() {
		model = GameModel.GetInstance();
	}

	public virtual void grow(int _row, int _col) {
		row = _row;
		col = _col;
		GameObject error = model.errorIconMap[row, col];
		if(error == null)	
			model.errorIconMap[row, col] = gameObject;

	}

	public virtual void Die() {
		model.map[row, col] = null;
		Destroy(gameObject);
	}
}
