using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        private LazyValue<float> _healthPoints;
        private bool _isDead;
        
        public bool IsDead()
        {
            return _isDead;
        }

        private void Awake()
        {
            _healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            _healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage: " + damage);
            
            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0);
    
            if (_healthPoints.value == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }
        
        private void Die()
        {
            if(_isDead)
                return;

            _isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        
        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if(experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExpirienceReward));
        }
        
        private void RegenerateHealth()
        {
            _healthPoints.value = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetHealthPoints()
        {
            return _healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        public float GetPercentage()
        {
            return 100 * (_healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }
        
        public object CaptureState()
        {
            return _healthPoints;
        }

        public void RestoreState(object state)
        {
            _healthPoints.value = (float)state;
            if(_healthPoints.value == 0)
                Die();
        }
    }
}
