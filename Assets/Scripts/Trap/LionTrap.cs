using Cinemachine;
using DG.Tweening;
using Game;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LionTrap : Trap
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    public UnityEvent OnActivate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Activate();
        
    }
    [Button]
    private void Activate()
    {
        _camera.gameObject.SetActive(true);
        OnActivate.Invoke();
        Debug.Log("activate");
    }
}
