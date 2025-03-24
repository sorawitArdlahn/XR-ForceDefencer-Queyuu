using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Model.Stats;
using System.Collections;
using Controller.Combat;

namespace Controller.Movement
{
    public class AimAssistantCtrl : MonoBehaviour
    {
        private List<GameObject> allEnemies = new List<GameObject>();
        private bool isTurnOn = false;
        public Canvas worldSpaceCanvas;
        public Image aimImage;
        public TextMeshProUGUI LockOnText;
        public Transform playerTransform;
        public Transform cockpitTransform;
        public Transform pointer;
        public GameObject target;
        public float targetHight;

        private Vector3 newAngle;

        public float targetDistance;
        public float setDistance; // for debuging

        private bool isEnemyAvailable = true;
    
        private bool isAimActive = false;
        
        public MissileLauncherController missileLauncherController;
        private void Awake()
        {
            allEnemies = new List<GameObject>();
            
            aimImage.gameObject.SetActive(false);
            aimImage.transform.SetParent(worldSpaceCanvas.transform);
            enabled = false;
        }

        private void OnEnable()
        {
            LockOnText.gameObject.SetActive(true);
            isAimActive = true;
            LockOnText.text =  "-LOCKED-" ;
            // LockOnText.color = Color.green;

            // if (!isEnemyAvailable)
            // {
            //     LockOnText.text =  "No Enemy Available";
            //     LockOnText.fontSize = 9;
            //     LockOnText.color = Color.red;
            //     isAimActive = false;
            //     target = null;
            //     allEnemies.Clear();
            // }
        }

        private void OnDisable()
        {
            if (aimImage != null)
            {
                aimImage.gameObject.SetActive(false);
            }
            LockOnText.gameObject.SetActive(false);
            // LockOnText.color = Color.red;
            isAimActive = false;
            target = null;
            aimImage.transform.position = cockpitTransform.position;
        }

        private void Update()
        {
            ClearDeathEnemy();
            if (isAimActive)
            {
                FindEnemy();
                if (!isEnemyAvailable)
                {
                    LockOnText.text =  "-ENEMY-NOT-FOUND-";
                    LockOnText.fontSize = 9;
                    // LockOnText.color = Color.red;
                    isAimActive = false;
                    target = null;
                    allEnemies.Clear();
                }
                
                MoveToClosetEnemy();
            }
        
            if (target && target.GetComponent<RobotInGameStats>().currentHP <= 0)
            {
                allEnemies.Remove(target);
                target = null;
            }

            
        }

        private void FindEnemy()
        {
            foreach (var enemyGameObject in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (enemyGameObject.GetComponent<RobotInGameStats>().currentHP <= 0) continue;
                if (allEnemies.Contains(enemyGameObject)) continue;
                allEnemies.Add(enemyGameObject);
            }
            
            if (allEnemies.Count == 0)
            {
                isEnemyAvailable = false;
            }else
            {
                isEnemyAvailable = true;
            }
        }

        private void ClearDeathEnemy()
        {
            if (allEnemies.Count == 0) return;
            foreach (var enemy in allEnemies)
            {
                if (!enemy)
                {
                    allEnemies.Remove(enemy);
                }
            }
        }

        private GameObject FindClosestEnemy()
        {
            newAngle = Vector3.zero;
            float closestDistance = 999;
            GameObject closestEnemy = null;
            float distanceBetweenPlayerAndEnemy;
            foreach (GameObject enemy in allEnemies)
            {
                try
                {
                    distanceBetweenPlayerAndEnemy = Vector3.Distance(playerTransform.position, enemy.transform.position);
                }
                catch
                { 
                    continue;
                }

                if (distanceBetweenPlayerAndEnemy < closestDistance)
                {
                    closestDistance = distanceBetweenPlayerAndEnemy;
                    closestEnemy = enemy;
                }
            }

            try
            {
                targetDistance = Vector3.Distance(playerTransform.position, closestEnemy.transform.position);
            }
            catch
            {
                missileLauncherController.AssignTarget(null);
                return null;
            }
            
            missileLauncherController.AssignTarget(closestEnemy);
            return closestEnemy;
            
        }

        private void MoveToClosetEnemy()
        {
            newAngle = Vector3.zero;
            target = FindClosestEnemy();
            if (target)
            {
                Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y + targetHight, target.transform.position.z);
                transform.position = targetPosition;
                transform.LookAt(cockpitTransform.position);

                aimImage.transform.position =  Vector3.Slerp(aimImage.transform.position, pointer.position, Time.deltaTime * 1.5f); ;
                aimImage.transform.LookAt(cockpitTransform.position);
                aimImage.gameObject.SetActive(true);

                if (Vector3.Distance(playerTransform.position, aimImage.transform.position) >= setDistance)
                {
                    var rotation = Quaternion.LookRotation(aimImage.transform.position - playerTransform.position);
                
                    rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
                
                    playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, rotation, Time.deltaTime); 
                
                    cockpitTransform.LookAt(aimImage.transform.position);
                
                    newAngle = cockpitTransform.rotation.eulerAngles;
                }
            }

            else{
                isEnemyAvailable = false;
                //ClearDeathEnemy();
            }

        }

        public Vector3 GetNewAngles(float cockpitRotation)
        {
            if(newAngle == Vector3.zero)
            {
                return new Vector3(cockpitRotation, cockpitRotation, cockpitRotation);
            }

            return newAngle;
        }

        public bool IsEnemyAvailable()
        {
            FindEnemy();
            return isEnemyAvailable;
        }

        public GameObject GetCurrentTarget()
        {
            return target;
        }
    }
}
