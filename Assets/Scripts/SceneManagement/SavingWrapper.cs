using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFile = "save";
        [SerializeField] private float _fadeInTimer = 0.2f;

        private IEnumerator Start()
        {
            CanvasFader fader = FindObjectOfType<CanvasFader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                GetComponent<SavingSystem>().Delete(defaultSaveFile);
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
           GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}
