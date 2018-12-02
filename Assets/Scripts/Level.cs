using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

    public string[] demons;
    public Heart[] hearts;
    public LevelSelector holder;
    public StartArea area;
    public Transform indicatorArea;

    public bool deaths = false;

    private int current = 0;
    private List<Indicator> indicators = new List<Indicator>();

	// Use this for initialization
	void Start () {
        Cursor.visible = false;

        for (int j = 0; j < demons.Length; j++) {
            string[] anglesStr = demons[j].Split(',');
            float[] angles = new float[anglesStr.Length];
            for (int i = 0; i < anglesStr.Length; i++)
            {
                angles[i] = float.Parse(anglesStr[i]);
            }

            var ind = Instantiate(Manager.Instance.indicatorPrefab, Vector3.zero, Quaternion.identity, indicatorArea);
            ind.transform.localPosition = new Vector3(j * 0.7f, 0, 0);
            ind.AddHands(angles);
            indicators.Add(ind);
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

    public void ToggleIndicator(int i, bool u, bool b) {
        int idx = i != -1 ? i : current;
        indicators[idx].blinking = b;
        indicators[idx].used = u;
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

    public void DoEndSounds() {
        Invoke("EndSounds", 0.5f);
    }

    public void EndSounds() {

        foreach (var h in hearts)
        {
            if (h && h.gameObject.activeInHierarchy)
            {
                AudioManager.Instance.PlayEffectAt(22, Camera.main.transform.position, 1.2f);
                AudioManager.Instance.PlayEffectAt(25, Camera.main.transform.position, 1.5f);
                return;
            };
        }

        if(deaths) {
            return;
        }

        AudioManager.Instance.PlayEffectAt(22, Camera.main.transform.position, 1.2f);
        AudioManager.Instance.PlayEffectAt(24, Camera.main.transform.position, 1.5f);
    }

	public void Reset()
	{
        Invoke("FailSounds", 0.6f);
        Invoke("DestroyDemons", 0.3f);
        Invoke("ResetHearts", 0.75f);
        Invoke("DestroyGores", 1.75f);
        Invoke("StartAgain", 1.25f);
	}

    void FailSounds() {
        AudioManager.Instance.PlayEffectAt(22, Camera.main.transform.position, 1.2f);
        AudioManager.Instance.PlayEffectAt(25, Camera.main.transform.position, 1.5f);
    }

    void DestroyDemons() {
        Manager.Instance.KillAll();
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

        for (int i = 0; i < indicators.Count; i++)
        {
            ToggleIndicator(i, false, false);
        }
    }

    void StartAgain() {
        deaths = false;
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
