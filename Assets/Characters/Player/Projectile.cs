using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float projectileSpeed;
	public float damageCaused = 10f;

	void OnCollisionEnter(Collision collision) {
		Component damagableComponent = collision.gameObject.GetComponent (typeof(IDamagable));

		if (damagableComponent) {
			(damagableComponent as IDamagable).TakeDamage (damageCaused);
		}
		Destroy (gameObject, 0.01f);
 	}

}
