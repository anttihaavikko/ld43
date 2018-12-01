using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea : MonoBehaviour {
    private bool used = false;
    private bool hovered = false;

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
