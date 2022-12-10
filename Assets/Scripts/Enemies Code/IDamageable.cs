using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int CurrentHealth { get; set; }
    void TakeDamage(int damageAmount, Vector2 point = default);
}
