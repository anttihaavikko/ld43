using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gore : MonoBehaviour {

    public Rigidbody2D[] parts;
    public SpriteRenderer leftHair, leftBeard, leftHorns, rightHair, rightBeart, rightHorns;
    public int goreColorIndex;

	// Use this for initialization
	void Start () {
        foreach(var p in parts) {
            Manager.Instance.AddGore(p.gameObject);
            var amount = 10f;
            p.AddForce(new Vector2(Random.Range(-amount, amount), Random.Range(-amount, amount)), ForceMode2D.Impulse);
            p.AddTorque(Random.Range(-100f, 100f));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
