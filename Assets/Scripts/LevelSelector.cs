using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

	private int current;
    private GameObject prev;
    public Transform camHolder;



	// Use this for initialization
	void Start () {

		int idx = 0;

		// find currently active level
		foreach(Transform child in transform) {
			if (child.gameObject.activeSelf) {
				current = idx;
                break;
			}

			idx++;
		}

		ActivateLevel (current);
	}

	public void ActivateLevel(int level, bool delayed = false) {

		if (level == -1) {
			return;
		}

        prev = transform.GetChild(current).gameObject;

        Manager.Instance.ClearDemons();

		// deactivate current level
        if(delayed) {
            Invoke("DeactivatePrevious", 2f);   
        } else {
            DeactivatePrevious();
        }

		current = level;

        // activate next level
        var next = transform.GetChild(current);
		next.gameObject.SetActive (true);

        var p = new Vector3(next.position.x, next.position.y, -10f);
        Tweener.Instance.MoveTo(camHolder, p, 0.7f, 0f, TweenEasings.QuadraticEaseInOut);

        Manager.Instance.level = next.GetComponent<Level>();
	}

    void DeactivatePrevious() {
        if(prev) 
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
