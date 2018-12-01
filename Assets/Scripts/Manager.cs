using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    public Demon demonPrefab;

    public Sprite[] horns;
    public Sprite[] hairs;

	private static Manager instance = null;
	public static Manager Instance {
		get { return instance; }
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}
	}

	private void Start()
	{
        //AddDemon();
	}

	public void AddDemon() {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        Instantiate(demonPrefab, pos, Quaternion.identity);
    }

    public Sprite GetHair() {
        return hairs[Random.Range(0, hairs.Length)];
    }

    public Sprite GetHorn() {
        return horns[Random.Range(0, horns.Length)];
    }
}
