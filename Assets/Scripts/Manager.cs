using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    public Demon demonPrefab;

    public Sprite[] horns;
    public Sprite[] hairs;

    public Level level;

    private List<Demon> demons;

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

        demons = new List<Demon>();
	}

	public void AddDemon() {
        Invoke("CreateDemon", 2f);
    }

    public void CreateDemon() {
        if(level.StillLeft()) {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            var d = Instantiate(demonPrefab, pos, Quaternion.identity);
            var angles = level.GetNextDemon();
            d.AddHands(angles);
            demons.Add(d);

            EffectManager.Instance.AddEffect(0, pos);
            EffectManager.Instance.AddEffectToParent(1, pos, d.transform);   
        }
    }

    public Sprite GetHair() {
        return hairs[Random.Range(0, hairs.Length)];
    }

    public Sprite GetHorn() {
        return horns[Random.Range(0, horns.Length)];
    }

    public void RemoveDemon(Demon d) {
        demons.Remove(d);
    }

    public void ClearDemons() {
        foreach(var d in demons) {
            Destroy(d.gameObject);
        }

        demons.Clear();
    }

    public void KillAll() {
        foreach (var d in demons)
        {
            d.Die();
        }
    }
}
