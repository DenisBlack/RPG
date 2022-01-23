using System;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] private float _timeBetweenAttacks = 1f;
        [SerializeField] private Transform _rightHandTransform = null;
        [SerializeField] private Transform _leftHandTransform = null;
        [SerializeField] private Weapon _defaultWeapon = null;
      
        private Health _target;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private LazyValue<Weapon> _currentWeapon = null;
        private GameObject _currentWeaponGO = null;

        private void Awake()
        {
            _currentWeapon = new LazyValue<Weapon>(SetupDefaoultWeapon);
        }

        private Weapon SetupDefaoultWeapon()
        {
            AttachWeapon(_defaultWeapon);
            return _defaultWeapon;
        }

        private void Start()
        {
            _currentWeapon.ForceInit();
        }
        
        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if(_target == null) return;
            if(_target.IsDead()) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            
            if (_timeSinceLastAttack > _timeBetweenAttacks)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.value.GetAttackRange;
        }
        
        public void Attack(GameObject target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = target.GetComponent<Health>();
        }
        
        public void Cancel()
        {
            StopAttack();
            _target = null;
            _timeSinceLastAttack = 0;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
        
        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.value.GetWeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.value.GetPercentageBonus;
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (_currentWeapon.value != null)
            {
                if(_currentWeaponGO != null)
                {
                    Destroy(_currentWeaponGO);
                }
            }
            
            _currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
            _currentWeaponGO =
                weapon.Spawn(weapon.GetCurrentHandType == Weapon.HandsType.LeftHand ? _leftHandTransform : _rightHandTransform,
                    animator);
        }

        public Health GetTarget()
        {
            return _target;
        }
        
        void Shoot()
        {
            Hit();
        }
        
        //Animation Trigger
        void Hit()
        {
            if (_target != null)
            {
                float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
                
                if (_currentWeapon.value.HasProjectile())
                {
                    _currentWeapon.value.LaunchProjectile(_currentWeapon.value.GetCurrentHandType == Weapon.HandsType.LeftHand ? _leftHandTransform : _rightHandTransform, _target, gameObject,damage);
                }
                else
                {
                    _target.TakeDamage(gameObject, damage);
                }
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
                return false;
            
            Health targetHealth = combatTarget.GetComponent<Health>();
            return targetHealth != null && !targetHealth.IsDead();
        }

        public object CaptureState()
        {
            return _currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}