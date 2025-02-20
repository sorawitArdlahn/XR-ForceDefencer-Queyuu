using System;
using System.Collections;
using Model;
using UnityEngine;
using Model.Stats;

namespace Controller.Stats
{
    [RequireComponent(typeof(RobotInGameStatsCtrl))]
    public class RobotInGameStatsCtrl : MonoBehaviour, IDamageable
    {
        public RobotInGameStats robotInGameStats;
        
        public int CurrentHealth => robotInGameStats.currentHP;
        public int CurrentArmor => robotInGameStats.currentArmor;

        public event IDamageable.TakeDamageEvent OnTakeDamage;
        public event IDamageable.DeathEvent OnDeath;

        private void Update()
        {
            if (robotInGameStats.currentFuel < robotInGameStats.MaxFuel) //Fuel must always be replenished.
            {
                StartCoroutine(Refuel(0.5f, 1f));
            }
        }

        public void TakeDamage(int damage)
        {
            int damageTaken = damage;
            
            if (CurrentArmor > 0)
            {
                damageTaken = damage / 2; //Armor can absorb damage and reduce them to half.
            }
           
            int sign = (int)Mathf.Sign(CurrentArmor - damageTaken); //Check if Damage value over armor or not.
            
            if (sign >= 0) //if damage still less than armor. 
            {
                robotInGameStats.setCurrentArmor(CurrentArmor - damageTaken);
            }
            else
            {
                int damageRemainder =  Mathf.Abs(CurrentArmor - damageTaken); //Calculate the remaining damage after armor reduction.
                robotInGameStats.setCurrentArmor(0);
                
                robotInGameStats.SetCurrentHP(CurrentHealth - damageRemainder);
                if (CurrentHealth <= 0) // Death
                {
                    robotInGameStats.SetCurrentHP(0);
                }
            }

        }

        public void UseFuel(int desiredFuel)
        {
            if (desiredFuel >= robotInGameStats.currentFuel)
            {
                robotInGameStats.setCurrentFuel(robotInGameStats.currentFuel - desiredFuel);
            }
        }

        private IEnumerator Refuel(float fillRate, float fillSpeed)
        {
            int addedFuel = (int)Mathf.Lerp(robotInGameStats.currentFuel, robotInGameStats.MaxFuel, fillRate); //Replenish the fuel tank with the needed fuel, at a rate governed by the fill speed.
            robotInGameStats.setCurrentFuel(addedFuel);
            yield return new WaitForSeconds(fillSpeed);
        }
    }
}
