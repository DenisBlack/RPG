using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class CanvasFader : MonoBehaviour
    {
        [SerializeField] private float _faderTimer = 1f;
        private CanvasGroup _canvasGroup;
        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public IEnumerator FadeOut()
        {
            while (_canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += Time.deltaTime /  _faderTimer ;
                yield return null;
            }
        }
        
        public IEnumerator FadeIn()
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= Time.deltaTime /  _faderTimer ;
                yield return null;
            }
        }
        
    }
}
