using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour {

    public SpriteRenderer handPrefab;
    public SpriteRenderer sprite;

    private List<SpriteRenderer> hands;

    public bool used, blinking;
    private float blinkTimer;

    private Color offColor = new Color(0.3f, 0.3f, 0.3f);
    private Color onColor = new Color(1f, 1f, 1f);

	// Use this for initialization
	void Start () {
        used = false;
        blinking = false;

        var mod = Random.Range(0.8f, 1.1f);
        transform.localScale *= mod;

        transform.localPosition += Vector3.up * Random.Range(-0.1f, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
        float pos = used ? 1f : 0f;

        if(blinking) {
            pos = Mathf.Abs(Mathf.Sin(blinkTimer));
            blinkTimer += Time.deltaTime * 3f;
        }

        sprite.color = Color.Lerp(offColor, onColor, pos);
        foreach(var h in hands) {
            h.color = Color.Lerp(offColor, onColor, pos);    
        }
	}

    public void AddHands(float[] angles)
    {
        hands = new List<SpriteRenderer>();

        foreach (var angle in angles)
        {
            float a = angle + 90f;
            float x = Mathf.Cos(a * Mathf.Deg2Rad);
            float y = Mathf.Sin(a * Mathf.Deg2Rad);
            var pos = new Vector3(x, y, 0);
            var h = Instantiate(handPrefab, Vector3.zero, Quaternion.identity, transform);
            h.transform.localPosition = pos * 0.51f;
            hands.Add(h);
        }
    }
}
