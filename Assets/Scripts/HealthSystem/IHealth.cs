using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IHealth : IDamageable
    {
        void Regen(int amount);
        void Kill();
    }
}
