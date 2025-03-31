using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller.Combat
{
    public class MissileLauncherController : MonoBehaviour
    {
        public List<Transform> firePoints;
        public GameObject missilePrefab;
        public float coolDownPeriodInSeconds;
        private GameObject target;

        private float timeStamp = 0f;
        
        
        public void LaunchMissile()
        {
            // foreach (var point in firePoints)
            // {
            //     var missile = Instantiate(missilePrefab, point.position, point.rotation);
            //     missile.GetComponent<HomingMissileController>().AssignTarget(target);
            // }

            StartCoroutine(DelaySpawnMissiles());

        }

        public void AssignTarget(GameObject target)
        {
            this.target = target;
        }

        IEnumerator DelaySpawnMissiles()
        {
            foreach (var point in firePoints)
            {
                var missile = Instantiate(missilePrefab, point.position, point.rotation);
                missile.GetComponent<HomingMissileController>().AssignTarget(target);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
