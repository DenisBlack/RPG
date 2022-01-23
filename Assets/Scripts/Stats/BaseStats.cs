using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Resources;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,100)]
        [SerializeField] private int _startingLevel = 1;
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField] private Progression _progression = null;
        [SerializeField] private GameObject _levelUpEffect;
        [SerializeField] private bool _shouldUseModifiers = false;
        private LazyValue<int> _currentLevel;

        public event Action onLevelUp;

        private Experience _experience;

        private void Awake()
        {
            _experience = GetComponent<Experience>();
            _currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            _currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (_experience != null)
            {
                _experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (_experience != null)
            {
                _experience.onExperienceGained -= UpdateLevel;
            }
        }
        
        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > _currentLevel.value)
            {
                _currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp?.Invoke();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(_levelUpEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }
        
        private float GetBaseStat(Stat stat)
        {
            return _progression.GetStat(stat, _characterClass, GetLevel());
        }

        public int GetLevel()
        {
            return _currentLevel.value;
        }
        
        private int CalculateLevel()
        {
            if (_experience == null) return _startingLevel;

            float currentXP = _experience.GetPoints();
            int penultimateLevel = _progression.GetLevels(Stat.ExpirienceToLevelUp, _characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = _progression.GetStat(Stat.ExpirienceToLevelUp, _characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
        
        private float GetAdditiveModifier(Stat stat)
        {
            if (!_shouldUseModifiers) return 0;
            
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifiers in provider.GetAdditiveModifier(stat))
                {
                    total += modifiers;
                }
            }

            return total;
        }
        
        private float GetPercentageModifier(Stat stat)
        {
            if (!_shouldUseModifiers) return 0;
            
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifiers in provider.GetPercentageModifier(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }
    }
}
