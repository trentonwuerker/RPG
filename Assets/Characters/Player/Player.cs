using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CameraUI;
using RPG.Core;
using RPG.Weapons;
using System;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamagable
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float damagePerHit = 10f;
        [SerializeField] int enemyLayer = 9;
        [SerializeField] Weapon weaponInUse;
        [SerializeField] GameObject weaponSocket;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        Animator animator;
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
            HandleMouseClick();
            SetCurrentHealthPoints();
            PutWeaponInHand();
            SetupRuntimeAnimator();
        }

        private void HandleMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        }

        private void SetCurrentHealthPoints()
        {
            currentHealthPoints = maxHealthPoints;
        }

        void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
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
            if (layerHit == enemyLayer)
            {
                var enemy = raycastHit.collider.gameObject;

                if (IsTargetInRange(enemy))
                {
                    AttackTarget(enemy);
                }
            }
        }

        private void AttackTarget(GameObject target)
        {
            var enemyComponent = target.GetComponent<Enemy>();
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                animator.SetTrigger("Attack");
                enemyComponent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }

        public void TakeDamage(float damage)
        {
            if (currentHealthPoints - damage <= 0)
            {
                currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
                StartCoroutine(KillPlayer());
            }
            else
            {
                currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            }

        }

        private IEnumerator KillPlayer()
        {
            yield return new WaitForSecondsRealtime(2f);
            SceneManager.LoadScene("Death");
        }
    }
}