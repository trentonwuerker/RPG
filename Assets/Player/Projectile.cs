using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float projectileSpeed;
	public float damageCaused = 10f;

	void OnTriggerEnter(Collider collider) {
		Component damagableComponent = collider.gameObject.GetComponent (typeof(IDamagable));

		if (damagableComponent) {
			(damagableComponent as IDamagable).TakeDamage (damageCaused);
		}
 	}

}
