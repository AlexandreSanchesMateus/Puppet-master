using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Entity : MonoBehaviour
    {
        public Health Health => _health;
        [SerializeField, Required()] protected Health _health;

        //[SerializeField] protected IPickable _knifeObject;
        [SerializeField] protected Transform _weaponHolder;
	    public Transform WeaponHolder => _weaponHolder;

        public IPickable currentWeapon;

        public bool HasWeapon()
        {
	        return currentWeapon != null;
        }

        private void Update()
        {
	        if (Input.GetKeyDown(KeyCode.A))
	        {
		        if (currentWeapon == null)
		        {
			        currentWeapon.Take(_weaponHolder);

					currentWeapon = currentWeapon;
				}
		        else
		        {
			        currentWeapon.Release();

			        currentWeapon = null;
				}
	        }
        }
/*
        public GameObject Model => _model;
        [SerializeField] private GameObject _model;

        public Vector3 DefaultModelScale => _defaultModelScale;
        [SerializeField] private Vector3 _defaultModelScale;

		public Rigidbody Rigidbody => _rigidbody;
		[SerializeField] private Rigidbody _rigidbody;*/
    }
}
