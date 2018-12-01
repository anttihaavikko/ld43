using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

    public string[] demons;
    public Heart[] hearts;
    public LevelSelector holder;

    private int current = 0;

	// Use this for initialization
	void Start () {
        Manager.Instance.level = this;
        Manager.Instance.AddDemon();
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadSceneAsync("Main");
            }
        }
	}

    public float[] GetNextDemon() {
        string[] anglesStr = demons[current].Split(',');
        float[] angles = new float[anglesStr.Length];
        for (int i = 0; i < anglesStr.Length; i++) {
            angles[i] = float.Parse(anglesStr[i]);
        }

        current++;

        return angles;
    }

    public bool StillLeft() {
        var r = current < demons.Length;

        if(!r) {
            Invoke("CheckEnd", 1f);
        }

        return r;
    }

    public void CheckEnd() {

        if(hearts.Length == 0) {
            NextLevel();
        }

        foreach(var h in hearts) {
            if (h.gameObject.activeInHierarchy) {
                Reset();
                return;
            };
        }

        NextLevel();
    }

	public void Reset()
	{
        Debug.Log("Reset");
	}

	public void NextLevel() {
        Debug.Log("Next");
        holder.NextLevel();
    }
}
