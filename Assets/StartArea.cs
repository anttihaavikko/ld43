using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea : MonoBehaviour {
    private bool used = false;
    public void OnMouseEnter() {
        if(!used) {
            Manager.Instance.AddDemon();
            used = true;
        }
    }
}
