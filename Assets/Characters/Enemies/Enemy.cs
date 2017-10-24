using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamagable {

	[SerializeField] float maxHealthPoints = 100f;
	[SerializeField] float attackRadius = 1f;
	[SerializeField] float chaseRadius = 10f;
	[SerializeField] float damagePerShot = 10f;
	[SerializeField] float secondsBetweenShots = 0.5f;
	[SerializeField] GameObject projectileToUse;
	[SerializeField] GameObject projectileSocket;
	[SerializeField] Vector3 aimOffset = new Vector3 (0, 1.5f, 0);

	bool isAttacking = false;


	float currentHealthPoints;
	AICharacterControl aiCharacterControl = null;
	GameObject player = null;

	public float healthAsPercentage {
		get {
			return currentHealthPoints / maxHealthPoints;
		}
	}

	public void TakeDamage (float damage) {
		currentHealthPoints = Mathf.Clamp (currentHealthPoints - damage, 0f, maxHealthPoints);
		if (currentHealthPoints <= 0) { Destroy (gameObject); }
	}

	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player");
		aiCharacterControl = GetComponent<AICharacterControl> ();
		currentHealthPoints = maxHealthPoints;
	}

	void Update(){
		float distanceToPlayer = Vector3.Distance (player.transform.position, transform.position);

		if (distanceToPlayer <= attackRadius && !isAttacking) {
			isAttacking = true;
			InvokeRepeating ("SpawnProjectile", 0f, secondsBetweenShots);
		}
		if (distanceToPlayer > attackRadius) {
			isAttacking = false;
			CancelInvoke ();
		}

		if (distanceToPlayer <= chaseRadius) {
			aiCharacterControl.SetTarget (player.transform);
		} else {
			aiCharacterControl.SetTarget (transform);

		}
	}

	void SpawnProjectile() {
		GameObject newProjectile = Instantiate (projectileToUse, projectileSocket.transform.position, Quaternion.identity);
		newProjectile.GetComponent<Projectile> ().damageCaused = damagePerShot;

		Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
		float projectileSpeed = newProjectile.GetComponent<Projectile> ().projectileSpeed;
		newProjectile.GetComponent<Rigidbody> ().velocity = unitVectorToPlayer * projectileSpeed;

	}

}
