using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Weapons
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float projectileSpeed;
        [SerializeField] GameObject shooter;

        public float damageCaused = 10f;

        void OnCollisionEnter(Collision collision)
        {

            if (shooter && collision.gameObject.layer != shooter.layer)
            {
                Component damagableComponent = collision.gameObject.GetComponent(typeof(IDamagable));
                if (damagableComponent)
                {
                    (damagableComponent as IDamagable).TakeDamage(damageCaused);
                }
                Destroy(gameObject, 0.01f);
            }
        }

        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }
    }
}