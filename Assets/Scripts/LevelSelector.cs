using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

	private int current;
    private GameObject prev;

	// Use this for initialization
	void Start () {

		int idx = 0;

		// find currently active level
		foreach(Transform child in transform) {
			if (child.gameObject.activeSelf) {
				current = idx;
			}

			idx++;
		}

		ActivateLevel (0);
	}

	public void ActivateLevel(int level, bool delayed = false) {

		if (level == -1) {
			return;
		}

        Manager.Instance.ClearDemons();

		// deactivate current level
        prev = transform.GetChild (current).gameObject;
        if(delayed) {
            Invoke("DeactivatePrevious", 1f);   
        } else {
            DeactivatePrevious();
        }

		current = level;

        // activate next level
        var next = transform.GetChild(current);
		next.gameObject.SetActive (true);

        Tweener.Instance.MoveTo(Camera.main.transform, new Vector3(next.position.x, next.position.y, -10f), 1f, 0f, TweenEasings.QuadraticEaseInOut);
	}

    void DeactivatePrevious() {
        prev.SetActive(false);
    }

	void Update() {

		if (Input.GetKeyDown (KeyCode.KeypadPlus)) {
			NextLevel ();
		}

		if (Input.GetKeyDown (KeyCode.KeypadMinus)) {
			NextLevel (-1);
		}
	}

	public void NextLevel(int dir = 1) {
        ActivateLevel(current + dir, true);
	}
}
