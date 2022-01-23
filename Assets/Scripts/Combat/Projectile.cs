using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Resources;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private bool isHoming = true;
    [SerializeField] private GameObject _hitEffect = null;
    [SerializeField] private float _maxLifeTimer = 10f;
    private Health _target;
    private float _damage = 0;

    private GameObject _instigator = null;
    private void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    void Update()
    {
        if (_target != null)
        {
            if (isHoming && !_target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }
    }

    public void SetTarget(Health target, GameObject instigator, float damage)
    {
        _target = target;
        _damage = damage;
        _instigator = instigator;
        Destroy(gameObject, _maxLifeTimer);
    }
    
    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
            return _target.transform.position;
        }

        return _target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Health>() != _target) return;
        if(_target.IsDead()) return;
        _target.TakeDamage(_instigator,_damage);

        if(_hitEffect)
        {
            Instantiate(_hitEffect, GetAimLocation(), transform.rotation);
        }
        Destroy(gameObject);
    }
}
