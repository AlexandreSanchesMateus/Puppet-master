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
/*
        public GameObject Model => _model;
        [SerializeField] private GameObject _model;

        public Vector3 DefaultModelScale => _defaultModelScale;
        [SerializeField] private Vector3 _defaultModelScale;

		public Rigidbody Rigidbody => _rigidbody;
		[SerializeField] private Rigidbody _rigidbody;*/
    }
}
