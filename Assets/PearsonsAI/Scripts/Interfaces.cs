using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{

        void TakeDamage(float damageDealt);
    
        void TakeDamageExplosion(float damageDealt, Vector3 center);
    
}
