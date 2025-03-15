using System.Collections.Generic;
using UnityEngine;

namespace Controller.Combat
{
    public class MissileLauncherController : MonoBehaviour
    {
        public List<Transform> firePoints;
        public GameObject missilePrefab;

        public void LaunchMissile()
        {
            foreach (var point in firePoints)
            {
                Instantiate(missilePrefab, point.position, point.rotation);
            }
        }
    }
}
