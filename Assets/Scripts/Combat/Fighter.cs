using System;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private float _attackRange = 2f;
        private Transform _target;
        private void Update()
        {
            if(_target == null)
                return;
            
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.position);
            }
            else
            {
                GetComponent<Mover>().StopMove();
            }
        }
        
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.position) < _attackRange;
        }
        
        public void Attack(CombatTarget target)
        {
            _target = target.transform;
        }

        public void CancelTarget()
        {
            _target = null;
        }
    }
}