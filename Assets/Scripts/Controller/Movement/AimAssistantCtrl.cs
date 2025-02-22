using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

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
    
    
        public float setDistance; // for debuging
    
        private bool isAimActive = false;
        private void Awake()
        {
            allEnemies = new List<GameObject>();
            foreach (var enemyGameObject in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                allEnemies.Add(enemyGameObject);
            }
            aimImage.gameObject.SetActive(false);
            aimImage.transform.SetParent(worldSpaceCanvas.transform);
            enabled = false;
        }

        private void OnEnable()
        {
            isAimActive = true;
            LockOnText.text =  "ON" ;
            LockOnText.color = Color.green;
        }

        private void OnDisable()
        {
            if (aimImage != null)
            {
                aimImage.gameObject.SetActive(false);
            }
            LockOnText.text =  "OFF" ;
            LockOnText.color = Color.red;
            isAimActive = false;
        }

        private void Update()
        {
            if (isAimActive)
            {
                MoveToClosetEnemy();
            }
        }

        private GameObject FindClosestEnemy()
        {
            float closestDistance = 999;
            GameObject closestEnemy = null;
            foreach (GameObject enemy in allEnemies)
            {
                float distanceBetweenPlayerAndEnemy = Vector3.Distance(playerTransform.position, enemy.transform.position);
                if (distanceBetweenPlayerAndEnemy < closestDistance)
                {
                    closestDistance = distanceBetweenPlayerAndEnemy;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }

        private void MoveToClosetEnemy()
        {
            target = FindClosestEnemy();
            Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y + targetHight, target.transform.position.z);
            transform.position = targetPosition;
            transform.LookAt(cockpitTransform.position);
        
            aimImage.transform.position = pointer.position;
            aimImage.transform.LookAt(cockpitTransform.position);
            aimImage.gameObject.SetActive(true);

            if (Vector3.Distance(playerTransform.position, pointer.position) >= setDistance)
            {
                var rotation = Quaternion.LookRotation(pointer.position - playerTransform.position);
            
                rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            
                playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, rotation, Time.deltaTime); 
            
                cockpitTransform.LookAt(pointer.position);
            
                newAngle = cockpitTransform.rotation.eulerAngles;
            }
        
        }

        public Vector3 GetNewAngles()
        {
            return newAngle;
        }
    }
}
