using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIndentifier
        {
            A,B
        }

        [SerializeField] private DestinationIndentifier _destinationIndentifier;
        [SerializeField] private SceneAsset _scene;
        [SerializeField] private GameObject _spawnPoint;
        public GameObject SpawnPoint => _spawnPoint;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
         
            CanvasFader fader = FindObjectOfType<CanvasFader>();
            yield return fader.FadeOut();
            
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();
            
            yield return SceneManager.LoadSceneAsync(_scene.name);
            
            wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            wrapper.Save();
            
            yield return new WaitForSeconds(1f);
            
            yield return fader.FadeIn();
            
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.SpawnPoint.transform.position);
            player.transform.rotation = otherPortal.SpawnPoint.transform.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) continue;
                if(portal._destinationIndentifier != _destinationIndentifier) continue;
                return portal;
            }
            return null;
        }
    }

}