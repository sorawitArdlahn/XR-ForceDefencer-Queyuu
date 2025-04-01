using System;
using System.Collections;
using Model;
using UnityEngine;
using Model.Stats;
using Audio;

namespace Controller.Stats
{
    [RequireComponent(typeof(RobotInGameStatsCtrl))]
    public class RobotInGameStatsCtrl : MonoBehaviour, IDamageable
    {
        public RobotInGameStats robotInGameStats;
        public GameObject deathEffect;

        public int CurrentHealth => robotInGameStats.currentHP;
        public int CurrentArmor => robotInGameStats.currentArmor;

        public int CurrentFuel => robotInGameStats.currentFuel;

        public event IDamageable.TakeDamageEvent OnTakeDamage;
        public event IDamageable.DeathEvent OnDeath;

        private bool isCanRefuel = true;
        private bool isRefueling = false; // Used to check if fuel is being refueled.

        private bool isCanUseFuel = true;
        private int activeFuelUsageCount = 0; // Variable counts the number of times UseFuel is called.
        
        public AudioSource audioSource;
        public AudioClip hitSound;

        private void Update()
        {
            if ((robotInGameStats.currentFuel < robotInGameStats.MaxFuel) && isCanRefuel && !isRefueling)
            {
                StartCoroutine(Refuel(1, 0.1f));
            }
        }

        public void TakeDamage(int damage)
        {
            if (CurrentHealth <= 0) {
                Debug.Log("Robot is already dead");
                return;
            }

            if (gameObject.CompareTag("Player"))
            {
                AudioManagerController.Instance.PlaySFX("PlayerDamage");
            }

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
                    OnDeath?.Invoke();

                    //death
                    var effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
                    if (!gameObject.CompareTag("Player"))
                    {
                        Destroy(effect, 1f);
                        Destroy(gameObject,5f);   
                        Debug.Log("Enemy Destroyed!");
                    }
                }
            }
            if (gameObject.CompareTag("Enemy")) audioSource.PlayOneShot(hitSound);

        }

        public IEnumerator UseFuel(int desiredFuel)
        {
            if (desiredFuel <= robotInGameStats.currentFuel && isCanUseFuel)
            {
                isCanUseFuel = false;
                isCanRefuel = false; // Prevent refueling
                activeFuelUsageCount++; // Increase usage

                robotInGameStats.setCurrentFuel(robotInGameStats.currentFuel - desiredFuel);
                yield return new WaitForSeconds(0.5f);
                isCanUseFuel = true;

                activeFuelUsageCount--; // Reduce the number of uses when finished
                if (activeFuelUsageCount == 0) // There is no UseFuel pending.
                {
                    StopCoroutine("Refuel");
                    isCanRefuel = true;
                }
                
            }
        }
        private IEnumerator Refuel(int fillRate, float fillSpeed)
        {
            if (!isCanRefuel || isRefueling) yield break; // If you are refueling, stop.
    
            isRefueling = true; // Refueling.
    
            while (isCanRefuel)
            {
                yield return new WaitForSeconds(fillSpeed);

                if (!isCanRefuel)
                {
                    isRefueling = false; // Finished refueling
                    yield break; 
                }

                int addedFuel = robotInGameStats.currentFuel + fillRate;
                if (addedFuel > robotInGameStats.MaxFuel)
                {
                    addedFuel = robotInGameStats.MaxFuel;
                }
                robotInGameStats.setCurrentFuel(addedFuel);
            }

            isRefueling = false; // Finished refueling
        }

        public void RegenerateArmor()
        {
            var newArmor = CurrentArmor + (robotInGameStats.maxArmor * 0.5);
            newArmor = Mathf.Clamp((float)newArmor, 0, robotInGameStats.maxArmor);
            robotInGameStats.setCurrentArmor((int)newArmor);
        }

        public void researchPointEarned()
        {
            robotInGameStats.data.currentResearchPoint += 75;
            robotInGameStats.data.accumulatedResearchPoint += 75;
        }
    }
}

