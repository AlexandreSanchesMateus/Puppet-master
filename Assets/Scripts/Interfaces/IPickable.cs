using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IPickable
    {
        public void PickUp(Transform parent);
        public void Drop();
        public void Throw();
    }
}
