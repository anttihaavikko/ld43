using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour {

    public Rotator rotator;

    public void ChangeSpeed(float s) {
        rotator.ChangeSpeed(s);
    }
}
