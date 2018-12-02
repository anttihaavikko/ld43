using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour {
    public SpriteRenderer sprite, amplifier;
    public bool canColor = true;

	// Use this for initialization
	void Start () {
        var s = Manager.Instance.GetRune();
        sprite.sprite = s;
        amplifier.sprite = s;

        int dir = Random.value < 0.5f ? 1 : -1;
        sprite.transform.localScale = new Vector3(dir * sprite.transform.localScale.x, sprite.transform.localScale.y, sprite.transform.localScale.z);
        dir = Random.value < 0.5f ? 1 : -1;
        amplifier.transform.localScale = new Vector3(dir * amplifier.transform.localScale.x, amplifier.transform.localScale.y, amplifier.transform.localScale.z);

        if(canColor) {
            sprite.color = Manager.Instance.GetRuneColor();
        }

        transform.localScale *= Random.Range(0.8f, 1.2f);
	}
}
