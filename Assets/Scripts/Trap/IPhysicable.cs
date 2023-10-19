using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhysicable
{
    public void EnablePhysique(bool enable);
    public void SetPhysicState(PuppetPhysic.EPuppetPhysic newState);
}
