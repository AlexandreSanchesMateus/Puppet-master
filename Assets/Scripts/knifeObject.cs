using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class knifeObject : MonoBehaviour,IPickable
{
    public UnityEvent OnCut;
    public void Take(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void Release()
    {
        transform.SetParent(null);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<ICutable>(out ICutable toCut))
        {
            toCut.Cut();
            
        }
        OnCut?.Invoke();
    }
    [Button]
    public void DebugCut()
    {
        OnCut?.Invoke();
    }    
}
