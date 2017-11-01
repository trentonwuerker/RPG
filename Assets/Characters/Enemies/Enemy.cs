using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using RPG.Core;
using RPG.Weapons;
using System;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamagable
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float attackRadius = 1f;
        [SerializeField] float chaseRadius = 10f;
        [SerializeField] float damagePerShot = 10f;
        [SerializeField] float secondsBetweenShots = 0.5f;
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1.5f, 0);
        //[SerializeField] AnimatorOverrideController animatorOverrideController;

        Animator animator;
        bool isDead = false;
        bool isAttacking = false;
        float currentHealthPoints;
        AICharacterControl aiCharacterControl = null;
        Player player = null;

        public float healthAsPercentage
        {
            get
            {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        void Start()
        {
            player = FindObjectOfType<Player>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            currentHealthPoints = maxHealthPoints;
            SetupRuntimeAnimator();

        }

        void Update()
        {
            if (player.healthAsPercentage <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                Destroy(this);
            }
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                InvokeRepeating("FireProjectile", 0f, secondsBetweenShots);
            }
            if (distanceToPlayer > attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }

            if (distanceToPlayer <= chaseRadius)
            {
                aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(transform);

            }
        }

        void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0)
            {
                isDead = true;
                animator.SetTrigger("Death");
                StartCoroutine(killEnemy());
            }
        }

        private IEnumerator killEnemy()
        {
            yield return new WaitForSecondsRealtime(1.5f);
            Destroy(gameObject);
        }

        void FireProjectile()
        {
            if (!isDead)
            {
                animator.SetTrigger("EnemyAttack");
                GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
                Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
                projectileComponent.damageCaused = damagePerShot;
                projectileComponent.SetShooter(gameObject);

                Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
                float projectileSpeed = newProjectile.GetComponent<Projectile>().GetDefaultLaunchSpeed();
                newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
            }
        }
    }
}