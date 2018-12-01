using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
    }

    public void Grab(float delay) {
        Invoke("DoGrab", delay);
    } 

    private void DoGrab() {
        gameObject.SetActive(false);
        EffectManager.Instance.AddEffect(3, transform.position);
        EffectManager.Instance.AddEffect(2, transform.position);
    }
}
