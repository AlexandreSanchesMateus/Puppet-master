using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Physics2DInteraction : MonoBehaviour
{
    [SerializeField] UnityEvent<Collider2D> _onTriggerEnter;
    [SerializeField] UnityEvent<Collider2D> _onTriggerExit;
    [SerializeField] UnityEvent<Collider2D> _onTriggerStay;

    [SerializeField] UnityEvent<Collision2D> _onCollisionEnter;

    public event UnityAction<Collider2D> TriggerEnter2D { add => _onTriggerEnter.AddListener(value); remove => _onTriggerEnter.RemoveListener(value); }
    public event UnityAction<Collider2D> TriggerExit2D { add => _onTriggerExit.AddListener(value); remove => _onTriggerExit.RemoveListener(value); }
    public event UnityAction<Collider2D> TriggerStay2D { add => _onTriggerStay.AddListener(value); remove => _onTriggerStay.RemoveListener(value); }
    
    
    public event UnityAction<Collision2D> CollisionEnter { 
        add => _onCollisionEnter.AddListener(value); 
        remove => _onCollisionEnter.RemoveListener(value); }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _onCollisionEnter.Invoke(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
        => _onTriggerEnter.Invoke(collision);

    private void OnTriggerExit2D(Collider2D collision)
        => _onTriggerExit.Invoke(collision);

    private void OnTriggerStay2D(Collider2D collision)
        => _onTriggerStay.Invoke(collision);
}
