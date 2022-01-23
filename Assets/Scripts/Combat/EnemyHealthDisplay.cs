using System;
using RPG.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] private Text _text;
        private Fighter _fighter;
        private void Awake()
        {
            _fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (_fighter != null && _fighter.GetTarget() == null)
            {
                _text.text = "N/A";
            }

            Health health = _fighter.GetTarget();
            if (_fighter != null && health != null)
            {
                _text.text =     String.Format("Target Health: {0:0}/{1:0}%", health.GetHealthPoints(), health.GetMaxHealthPoints());
            }
        }
    }
}