﻿using UnityEngine;

namespace Model
{
    public interface IDamageable
    {
        public int CurrentHealth { get;}
        public int CurrentArmor { get;}

        public delegate void TakeDamageEvent(int Damage);
        public event TakeDamageEvent OnTakeDamage;
        
        public delegate void DeathEvent(Vector3 Position);
        public event DeathEvent OnDeath;
        
        public void TakeDamage(int Damage);

    }
}