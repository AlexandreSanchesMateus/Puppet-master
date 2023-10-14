using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BearTrap : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private Transform _firstPart, _secondPart;
    private Vector3 _baseRotFirstPart, _baseRotSecondPart;
    public UnityEvent OnClose;
    // Start is called before the first frame update
    void Start()
    {
        
        BaseSate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BaseSate()
    {
        _camera.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        Activate();
    }
    [Button]
    private void Activate()
    {
        _camera.gameObject.SetActive(true);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(_firstPart.DOLocalRotate(new Vector3(0, 0, 90),0.20f,RotateMode.Fast));
        mySequence.Join(_secondPart.DOLocalRotate(new Vector3(0, -180, 90), 0.20f, RotateMode.Fast).OnComplete(()=> OnClose.Invoke()));
    }
}
