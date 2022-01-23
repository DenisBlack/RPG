using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG Project/Weapons", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private GameObject _weaponPrefab = null;
        [SerializeField] private float _weaponDamage = 5f;
        [SerializeField] private float _percentageBonusDamage = 0f;
        [SerializeField] private float _attackRange = 2f;
        [SerializeField] private Projectile _projectile = null;
        public enum HandsType
        {
            LeftHand,
            RightHand
        }
        [SerializeField] private HandsType HandTypeWeapon;
        public HandsType GetCurrentHandType => HandTypeWeapon;
        
        public float GetAttackRange => _attackRange;
        public float GetWeaponDamage => _weaponDamage;

        public float GetPercentageBonus => _percentageBonusDamage;
        public GameObject Spawn(Transform handTransform, Animator animator)
        {
            GameObject weaponGO = null;
            if (_weaponPrefab != null)
            {
                weaponGO = Instantiate(_weaponPrefab, handTransform);    
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (_animatorOverrideController != null)
            {
                animator.runtimeAnimatorController = _animatorOverrideController;
            }
            else if(overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weaponGO;
        }

        public void LaunchProjectile(Transform handTransform, Health health, GameObject instigator, float calculateDamage)
        {
            Projectile projectile = Instantiate(_projectile, handTransform.position, Quaternion.identity);
            projectile.SetTarget(health, instigator, calculateDamage);
        }

        public bool HasProjectile()
        {
            return _projectile != null;
        }
    }
}
