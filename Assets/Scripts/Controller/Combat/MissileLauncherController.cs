using System.Collections.Generic;
using UnityEngine;

namespace Controller.Combat
{
    public class MissileLauncherController : MonoBehaviour
    {
        public List<Transform> firePoints;
        public GameObject missilePrefab;
        private GameObject target;

        public void LaunchMissile()
        {
            foreach (var point in firePoints)
            {
                var missile = Instantiate(missilePrefab, point.position, point.rotation);
                missile.GetComponent<HomingMissileController>().AssignTarget(target);
            }
        }

        public void AssignTarget(GameObject target)
        {
            this.target = target;
        }
    }
}
