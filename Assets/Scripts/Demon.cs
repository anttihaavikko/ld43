using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Demon : MonoBehaviour {

    public GameObject coll;
    public Hand handPrefab;
    public float[] handAngles;

    private float rotationSpeed = 200f;
    private bool tracking = true;

    private List<Hand> hands;

	// Use this for initialization
	void Start () {
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

            if (Input.GetMouseButtonUp(0))
            {
                Shoot();
            }

            var hits = Physics2D.OverlapCircleAll(transform.position, 1);

            if (hits.Length > 0)
            {
                foreach (var h in hits)
                {
                    if (h.gameObject != coll)
                    {
                        // HIT WALL
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
}
