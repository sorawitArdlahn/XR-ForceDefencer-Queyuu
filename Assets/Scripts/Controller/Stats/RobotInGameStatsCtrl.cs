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
        
        [SerializeField] private bool _isCanRefuel = true;

        private void Update()
        {

            if (robotInGameStats.currentFuel < robotInGameStats.MaxFuel && _isCanRefuel)
            {
                StartCoroutine(Refuel(1, 1f));
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                TakeDamage(10);
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                UseFuel(50);
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
            if (desiredFuel <= robotInGameStats.currentFuel)
            {
                _isCanRefuel = false;
                robotInGameStats.setCurrentFuel(robotInGameStats.currentFuel - desiredFuel);
                _isCanRefuel = true;
            }
        }

        private IEnumerator Refuel(int fillRate, float fillSpeed)
        {
            Debug.Log("IEnumerator Refuel wait 2 sec");
            yield return new WaitForSeconds(2);
            int addedFuel = robotInGameStats.currentFuel;
            addedFuel += fillRate;
            if (addedFuel > robotInGameStats.MaxFuel)
            {
                addedFuel = robotInGameStats.MaxFuel;
            }
            Debug.Log("wait 0.1 sec to add fuel");
            yield return new WaitForSeconds(.01f);
            robotInGameStats.setCurrentFuel(addedFuel);
            
        }
        
        
    }
}
