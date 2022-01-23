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
    public class LevelDisplay: MonoBehaviour
    {
        [SerializeField] private Text _text;
        private BaseStats _baseStats;

        private void Awake()
        {
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            _text.text = String.Format("Level: {0:0}", _baseStats.GetLevel());
        }
    }
}