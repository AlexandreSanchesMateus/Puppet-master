using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    public void MovePlayerTo(Vector3 position, float speed, bool disableInputs = true);
    public void EnableInput(bool enable);
}
