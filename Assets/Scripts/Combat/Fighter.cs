using System;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float _timeBetweenAttacks = 1f;
        [SerializeField] private Transform _rightHandTransform = null;
        [SerializeField] private Transform _leftHandTransform = null;
        [SerializeField] private Weapon _defaultWeapon = null;
      
        private Health _target;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private Weapon _currentWeapon = null;
        private GameObject _currentWeaponGO = null;
        private void Start()
        {
            if (_currentWeapon == null)
            {
                EquipWeapon(_defaultWeapon);
            }
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
            return Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.GetAttackRange;
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

        public void EquipWeapon(Weapon weapon)
        {
            if (_currentWeapon != null)
            {
                if(_currentWeaponGO != null)
                {
                    Destroy(_currentWeaponGO);
                }
            }
            
            _currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            _currentWeaponGO = weapon.Spawn(weapon.GetCurrentHandType == Weapon.HandsType.LeftHand ? _leftHandTransform : _rightHandTransform, animator);
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
                if (_currentWeapon.HasProjectile())
                {
                    _currentWeapon.LaunchProjectile(_currentWeapon.GetCurrentHandType == Weapon.HandsType.LeftHand ? _leftHandTransform : _rightHandTransform, _target);
                }
                else
                {
                    _target.TakeDamage(_currentWeapon.GetWeaponDamage);
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
            return _currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}