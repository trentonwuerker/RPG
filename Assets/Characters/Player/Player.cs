using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CameraUI;
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamagable
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float damagePerHit = 10f;
        [SerializeField] float minTimeBetweenHits = .5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] Weapon weaponInUse;
        [SerializeField] GameObject weaponSocket;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
 

        float currentHealthPoints;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;

        public float healthAsPercentage
        {
            get
            {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        void Start()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
            currentHealthPoints = maxHealthPoints;
            PutWeaponInHand();
            OverrideAnimatorController();
        }

        void OverrideAnimatorController(){
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip();
        }

        void PutWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            var weapon = Instantiate(weaponPrefab, weaponSocket.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;

        }

        void OnMouseClick(RaycastHit raycastHit, int layerHit)
        {
            if (layerHit == 9)
            {
                var enemy = raycastHit.collider.gameObject;

                if ((enemy.transform.position - transform.position).magnitude > maxAttackRange)
                {
                    return;
                }

                var enemyComponent = enemy.GetComponent<Enemy>();
                if (Time.time - lastHitTime > minTimeBetweenHits)
                {
                    enemyComponent.TakeDamage(damagePerHit);
                    lastHitTime = Time.time;
                }
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }

    }
}