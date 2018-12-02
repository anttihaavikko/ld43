using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnCollision : MonoBehaviour {

    public int soundIndex = 0;
    public float limit = 0.3f;

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if(collision.relativeVelocity.magnitude > limit) {
            AudioManager.Instance.PlayEffectAt(soundIndex, collision.contacts[0].point, Mathf.Clamp(collision.relativeVelocity.magnitude, 0.1f, 2f));
        }
	}
}
