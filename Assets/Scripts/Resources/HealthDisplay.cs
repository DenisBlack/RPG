using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Text _text;
        private Health _health;
        
        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            _text.text = String.Format("Health: {0:0}/{1:0}%", _health.GetHealthPoints(), _health.GetMaxHealthPoints());
        }
    }
}
