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
    private List<GameObject> gores;

    private EffectCamera cam;

    public GameObject pentagram;
    public SpriteRenderer pentagramRing, pentagramDots;
    private Color pentagramColor, pentagramClear;

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
        gores = new List<GameObject>();

        pentagramColor = pentagramRing.color;
        pentagramClear = new Color(1, 1, 1, pentagramColor.a);
        pentagramRing.color = Color.clear;
        pentagramDots.color = Color.clear;

        cam = Camera.main.GetComponent<EffectCamera>();
	}

	public void AddDemon() {
        CancelInvoke("CreateDemon");
        Invoke("CreateDemon", 2f);

        if (level.StillLeft())
        {
            EffectManager.Instance.AddEffectToParent(1, pentagramRing.transform.position, pentagramRing.transform);

            Tweener.Instance.ColorTo(pentagramRing, pentagramColor, 0.5f, 0f, TweenEasings.CubicEaseInOut);
            Tweener.Instance.ColorTo(pentagramDots, pentagramColor, 0.5f, 0f, TweenEasings.CubicEaseInOut);
        }
    }

	private void Update()
	{
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadSceneAsync("Main");
            }
        }

        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        pentagram.transform.position = pos;
	}

	public void CreateDemon() {
        if(level.StillLeft()) {
            Tweener.Instance.ColorTo(pentagramRing, Color.clear, 0.5f, 0f, TweenEasings.CubicEaseInOut);
            Tweener.Instance.ColorTo(pentagramDots, Color.clear, 0.5f, 0f, TweenEasings.CubicEaseInOut);

            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            var d = Instantiate(demonPrefab, pos, Quaternion.identity);
            var angles = level.GetNextDemon();
            d.AddHands(angles);
            demons.Add(d);

            EffectManager.Instance.AddEffect(0, pos);
            EffectManager.Instance.AddEffectToParent(1, pos, d.transform);   
        } else {
            level.CheckEnd();
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
            if(d)
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

    public void AddGore(GameObject g)
    {
        gores.Add(g);
    }

    public void ClearGore()
    {
        foreach (var g in gores)
        {
            var gore = g.GetComponentInParent<Gore>();
            cam.BaseEffect(1.2f);
            EffectManager.Instance.AddEffect(1, g.transform.position);
            EffectManager.Instance.AddEffect(0, g.transform.position);
            EffectManager.Instance.AddEffect(gore.goreColorIndex, g.transform.position);
            EffectManager.Instance.AddEffect(2, g.transform.position);
            Destroy(g.gameObject);
        }

        gores.Clear();
    }
}
