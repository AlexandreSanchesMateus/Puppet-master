using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class StringManager : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    private void Update()
    {
        if (_lineRenderer == null) return;

        _lineRenderer.useWorldSpace = true;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, new Vector3(transform.position.x,Vector3.up.y * 100, transform.position.z));
    }
}
