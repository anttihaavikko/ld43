﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    public Demon demonPrefab;

    public Sprite[] horns;
    public Sprite[] hairs;
    public Sprite[] runes;
    public Color[] runeColors;

    public Level level;

    private List<Demon> demons;
    private List<GameObject> gores;

    private EffectCamera cam;

    public GameObject pentagram;
    public SpriteRenderer pentagramRing, pentagramDots;
    private Color pentagramColor, pentagramClear;

    public Indicator indicatorPrefab;

    public Transform quitMessage;
    private float quitTimer = 0f;

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
            level.ToggleIndicator(-1, true, true);

            AudioManager.Instance.PlayEffectAt(16, pentagramRing.transform.position, 0.5f);
            AudioManager.Instance.PlayEffectAt(6, pentagramRing.transform.position, 0.5f);
            AudioManager.Instance.PlayEffectAt(11, pentagramRing.transform.position, 0.5f);

            EffectManager.Instance.AddEffectToParent(1, pentagramRing.transform.position, pentagramRing.transform);

            Tweener.Instance.ColorTo(pentagramRing, pentagramColor, 0.5f, 0f, TweenEasings.CubicEaseInOut);
            Tweener.Instance.ColorTo(pentagramDots, pentagramColor, 0.5f, 0f, TweenEasings.CubicEaseInOut);
        } else {
            level.DoEndSounds();
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

        if(Input.GetKeyDown(KeyCode.Escape)) {
            Tweener.Instance.ScaleTo(quitMessage, Vector3.one, 0.4f, 0f, TweenEasings.BounceEaseOut);
            CancelInvoke("HideQuitMessage");
            Invoke("HideQuitMessage", 3f);
            quitTimer = 0f;
            AudioManager.Instance.PlayEffectAt(3, Manager.instance.level.transform.position - new Vector3(10f, 5f, 0f), 1f);
            AudioManager.Instance.PlayEffectAt(11, Manager.instance.level.transform.position - new Vector3(10f, 5f, 0f), 0.6f);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            quitTimer += Time.deltaTime;

            if(quitTimer > 1.2f) {
                Application.Quit();
            }
        }

        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        pentagram.transform.position = pos;
	}

    void HideQuitMessage() {
        Tweener.Instance.ScaleTo(quitMessage, new Vector3(0, 1f, 1f), 0.2f, 0f, TweenEasings.QuarticEaseIn);
        AudioManager.Instance.PlayEffectAt(3, Manager.instance.level.transform.position - new Vector3(10f, 5f, 0f), 1f);
        AudioManager.Instance.PlayEffectAt(11, Manager.instance.level.transform.position - new Vector3(10f, 5f, 0f), 0.6f);
        Cursor.visible = false;
    }

    public void CancelCreate() {
        CancelInvoke("CreateDemon");
        Tweener.Instance.ColorTo(pentagramRing, Color.clear, 0.5f, 0f, TweenEasings.CubicEaseInOut);
        Tweener.Instance.ColorTo(pentagramDots, Color.clear, 0.5f, 0f, TweenEasings.CubicEaseInOut);
    }

	public void CreateDemon() {
        if(level.StillLeft()) {
            level.ToggleIndicator(-1, true, false);

            AudioManager.Instance.PlayEffectAt(19, pentagramRing.transform.position, 1f);
            AudioManager.Instance.PlayEffectAt(20, pentagramRing.transform.position, 1f);
            AudioManager.Instance.PlayEffectAt(11, pentagramRing.transform.position, 0.75f);
            AudioManager.Instance.PlayEffectAt(2, pentagramRing.transform.position, 0.75f);

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

    public Sprite GetRune()
    {
        return runes[Random.Range(0, runes.Length)];
    }

    public Color GetRuneColor()
    {
        return runeColors[Random.Range(0, runeColors.Length)];
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
            EffectManager.Instance.AddEffect(7, g.transform.position);
            EffectManager.Instance.AddEffect(0, g.transform.position);
            EffectManager.Instance.AddEffect(gore.goreColorIndex, g.transform.position);
            EffectManager.Instance.AddEffect(2, g.transform.position);

            AudioManager.Instance.DoExplosion(g.transform.position, 0.6f);

            Destroy(g.gameObject);

            cam.BaseEffect(1.5f);
        }

        gores.Clear();
    }
}
