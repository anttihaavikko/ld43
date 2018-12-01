using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

    public string[] demons;
    public Heart[] hearts;
    public LevelSelector holder;
    public StartArea area;

    private int current = 0;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
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
        return r;
    }

    public void CheckEnd() {

        if(hearts.Length == 0) {
            NextLevel();
            return;
        }

        foreach(var h in hearts) {
            if (h && h.gameObject.activeInHierarchy) {
                Reset();
                return;
            };
        }

        NextLevel();
    }

	public void Reset()
	{
        Manager.Instance.KillAll();
        Invoke("ResetHearts", 0.75f);
        Invoke("DestroyGores", 1.75f);
        Invoke("StartAgain", 1.25f);
	}

    void DestroyGores() {
        Manager.Instance.ClearGore();
    }

    void ResetHearts() {
        foreach (var h in hearts)
        {
            if (h && !h.gameObject.activeInHierarchy)
            {
                h.gameObject.SetActive(true);
                EffectManager.Instance.AddEffect(1, h.transform.position);
            };
        }
    }

    void StartAgain() {
        current = 0;
        area.ReEnable();
    }

	public void NextLevel() {
        Camera.main.GetComponent<EffectCamera>().DoZoom();
        Invoke("ChangeLevel", 0.5f);
    }

    void ChangeLevel() {
        holder.NextLevel();
    }
}
