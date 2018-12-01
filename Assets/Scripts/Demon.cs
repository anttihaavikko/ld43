using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Demon : MonoBehaviour {

    public GameObject coll;
    public Hand handPrefab;
    public float[] handAngles;
    public SpriteRenderer hair, beard, horns;
    public LayerMask currentMask;

    private float rotationSpeed = 200f;
    private bool tracking = true;
    private List<Hand> hands;
    private bool dead = false;
    private float scaleMod;

	// Use this for initialization
	void Start () {

        horns.sprite = Manager.Instance.GetHorn();
        hair.sprite = Manager.Instance.GetHair();
        beard.sprite = Manager.Instance.GetHair();

        scaleMod = Random.Range(0.8f, 1.0f);
        transform.localScale *= scaleMod;

        hands = new List<Hand>();

        foreach(var angle in handAngles) {
            float x = Mathf.Sin(angle * Mathf.Deg2Rad);
            float y = Mathf.Cos(angle * Mathf.Deg2Rad);
            var pos = new Vector3(x, y, 0);
            var h = Instantiate(handPrefab, transform.position + pos * 0.6f, Quaternion.identity, transform);
            h.transform.Rotate(new Vector3(0, 0, angle - 90f));
            h.direction = new Vector2(x, y);
            hands.Add(h);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(Application.isEditor) {
            if(Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadSceneAsync("Main");
            }
        }

        if(tracking) {
            transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));

            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);   

            if(Input.GetMouseButton(0)) {
                rotationSpeed = Mathf.MoveTowards(rotationSpeed, 50f, 500f * Time.deltaTime);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Shoot();
            }

            var hits = Physics2D.OverlapCircleAll(transform.position, 0.5f, currentMask);

            if (hits.Length > 0)
            {
                foreach (var h in hits)
                {
                    if (h.gameObject != coll)
                    {
                        Die();
                    }
                }
            }
        }
	}

    void Shoot() {

        rotationSpeed = 0f;
        tracking = false;

        foreach(var h in hands) {
            h.Shoot();
        }

        Manager.Instance.AddDemon();
    }

    void Die() {
        if(dead) return;

        dead = true;

        EffectManager.Instance.AddEffect(Random.Range(3, 6), transform.position);
        EffectManager.Instance.AddEffect(2, transform.position);

        var gore = EffectManager.Instance.AddEffect(6, transform.position).GetComponent<Gore>();

        gore.leftHair.sprite = hair.sprite;
        gore.rightHair.sprite = hair.sprite;
        gore.leftBeard.sprite = beard.sprite;
        gore.rightBeart.sprite = beard.sprite;
        gore.leftHorns.sprite = horns.sprite;
        gore.rightHorns.sprite = horns.sprite;

        gore.transform.localScale *= scaleMod;

        Destroy(gameObject);

        Manager.Instance.AddDemon();
    }
}
