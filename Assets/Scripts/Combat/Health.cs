using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _healthPoints = 100f;
        private bool _isDead;

        public bool IsDead()
        {
            return _isDead;
        }
        public void TakeDamage(float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            print(_healthPoints);

            if (_healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if(_isDead)
                return;

            _isDead = true;
            GetComponent<Animator>().SetTrigger("die");
        }
    }
}
