using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _isPlayed;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_isPlayed)
            {
                _isPlayed = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
