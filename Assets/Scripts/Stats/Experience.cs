using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _experiencePoints = 0;

        public event Action onExperienceGained;
        
        public float GetPoints()
        {
            return _experiencePoints; 
        }
        
        public void GainExperience(float experience)
        {
            _experiencePoints += experience;
            onExperienceGained.Invoke();
        }
        
        public object CaptureState()
        {
            return _experiencePoints;
        }

        public void RestoreState(object state)
        {
            _experiencePoints = (float)state;
        }
    }
}