using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showlv : MonoBehaviour {
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
		GameObject LV = model.LVIconMap[row, col];
		if(LV == null)	
			model.LVIconMap[row, col] = gameObject;

	}

	public virtual void Die() {
		model.LVIconMap[row, col] = null;
		Destroy(gameObject);
	}
}
