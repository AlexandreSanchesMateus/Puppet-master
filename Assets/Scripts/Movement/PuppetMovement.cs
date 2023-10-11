using NaughtyAttributes;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuppetMovement : MonoBehaviour
{
    [SerializeField, BoxGroup("Dependencies")] private PuppetPhysic _puppetPhysic;
    [SerializeField, BoxGroup("Dependencies")] private BalanceForce _puppetBalance;

    [SerializeField, BoxGroup("Settings")] private float _movementSpeed;
    [SerializeField, BoxGroup("Settings")] private float _turnSpeed;

    public void SetRightLegValue(Vector2 value)
    {

    }

    public void SetLeftLegValue(Vector2 value)
    {

    }
}
