using UnityEngine;

namespace Controller
{
    public class CameraHolderCtrl : MonoBehaviour
    {
        [SerializeField] private Transform cameraPosition;
        
        void Update()
        {
            transform.position = cameraPosition.position;
        }
    }
}
