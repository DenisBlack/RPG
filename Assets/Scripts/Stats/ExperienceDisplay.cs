using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Resources;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] private Text _text;
        private Experience _experience;

        private void Awake()
        {
            _experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            _text.text = String.Format("Experience: {0:0}", _experience.GetPoints());
        }
    }
}

