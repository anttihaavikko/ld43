using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public LineRenderer arm;
    public EdgeCollider2D armBody;
    public Vector2 direction;
    public Vector2[] points;
    public LayerMask mask, heartMask;
    public GameObject aim;
    public Demon demon;

    private Vector3 pos;

	// Use this for initialization
	void Start () {
        arm.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(arm.enabled) {
            arm.SetPosition(0, transform.position);
            arm.SetPosition(1, transform.parent.position);

            var p = transform.InverseTransformPoint(transform.parent.position);
            points[1] = new Vector2(p.x, p.y);
            armBody.points = points;
        }
	}

    public void Shoot() {

        arm.SetPosition(0, transform.position);
        arm.SetPosition(1, transform.parent.position);

        arm.enabled = true;

        var v = direction;
        var degrees = transform.parent.rotation.eulerAngles.z;

        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);

        var hit = Physics2D.Raycast(transform.position, v, 100f, mask);

        pos = new Vector3(hit.point.x, hit.point.y, transform.position.z);

        var delay = Mathf.Min(hit.distance, 5f) * 0.04f;
        Tweener.Instance.MoveTo(transform, pos, delay, 0, TweenEasings.BounceEaseOut, 0);
        Invoke("HandEffect", delay - 0.1f);

        if(hit.collider.gameObject.tag == "Spikes") {
            Invoke("Die", delay);
        }

        // check for heart
        var heartHits = Physics2D.RaycastAll(transform.position, v, 100f, heartMask);

        foreach(var h in heartHits) {
            if (h && h.collider.gameObject.tag == "Heart")
            {
                var d = Mathf.Min(h.distance, 5f) * 0.04f;
                var heart = h.collider.gameObject.GetComponent<Heart>();
                heart.Grab(d);
            } else {
                break;
            }
        }

    }

    private void HandEffect() {
        gameObject.layer = 0;
        EffectManager.Instance.AddEffect(0, pos);
        EffectManager.Instance.AddEffect(1, pos);
    }

    void Die() {
        demon.DieAndReset();
    }
}
