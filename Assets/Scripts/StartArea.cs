using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea : MonoBehaviour {
    private bool used = false;
    private bool hovered = false;
    private Vector3 originalScale;
    private float amount = 0.05f;
    private float speed = 4f;

	private void Start()
	{
        originalScale = transform.localScale;
	}

	private void Update()
	{
        if(!used) {
            float amt = Mathf.Sin(Time.time * speed);
            amt = Mathf.Abs(amt);
            transform.localScale = originalScale + Vector3.one * amount * amt;   
        }
	}

	public void OnMouseEnter() {
        hovered = true;
        if(!used) {
            Manager.Instance.AddDemon();
            used = true;
        }
    }

	public void OnMouseExit()
	{
        hovered = false;
	}

	public void ReEnable() {
        used = false;

        if(hovered) {
            Manager.Instance.AddDemon();
            used = true;
        }
    }
}
