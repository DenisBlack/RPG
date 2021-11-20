using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Health _health;
        void Start()
        {
            _health = GetComponent<Health>();
        }

        void Update()
        {
            if(_health.IsDead())
                return;
            
            if (InteractWithCombat())
            {
                return;
            }
            if(InteractWithMovement());
            {
                return;
            }
            print("Do nothink");
            
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if(target == null) 
                    continue;

                GameObject targetGO = target.gameObject;
                
                if(!GetComponent<Fighter>().CanAttack(targetGO))
                    continue;

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);   
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        { 
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }
        
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
