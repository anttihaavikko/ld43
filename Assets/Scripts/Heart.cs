using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

    private EffectCamera cam;

	// Use this for initialization
	void Start () {
        cam = Camera.main.GetComponent<EffectCamera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Grab(float delay) {
        Invoke("DoGrab", delay);
    } 

    private void DoGrab() {
        gameObject.SetActive(false);
        EffectManager.Instance.AddEffect(3, transform.position);
        EffectManager.Instance.AddEffect(2, transform.position);

        AudioManager.Instance.DoExplosion(transform.position, 0.5f);
        AudioManager.Instance.PlayEffectAt(14, transform.position, 1.2f);

        cam.BaseEffect(0.75f);
    }
}
