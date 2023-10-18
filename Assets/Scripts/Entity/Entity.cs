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
        [SerializeField] protected knifeObject _knifeObject;
        [SerializeField] protected Transform _weaponHolder;

        protected GameObject _currentWeapon;

        private void Update()
        {
	        if (Input.GetKeyDown(KeyCode.A))
	        {
		        if (!_currentWeapon)
		        {
					_knifeObject.Take(_weaponHolder);

					_currentWeapon = _knifeObject.gameObject;
				}
		        else
		        {
			        _knifeObject.Release();

			        _currentWeapon = null;
				}
	        }
        }

		/* public GameObject Model => _model;
        [SerializeField] private GameObject _model;

        public Vector3 DefaultModelScale => _defaultModelScale;
        [SerializeField] private Vector3 _defaultModelScale;

		public Rigidbody Rigidbody => _rigidbody;
		[SerializeField] private Rigidbody _rigidbody;*/
    }
}
