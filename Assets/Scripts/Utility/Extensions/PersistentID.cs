// PersistentId.cs
using UnityEngine;

public class PersistentId : MonoBehaviour
{
    [SerializeField] private string _persistentId;
    
    public string Id {
        get {
            // Generate only once per object lifetime
            if(string.IsNullOrEmpty(_persistentId))
                _persistentId = GeneratePersistentId();
            return _persistentId;
        }
    }

    private string GeneratePersistentId()
    {
        // Uses scene path + position to create unique stable ID
        string scenePath = gameObject.scene.path;
        Vector3 pos = transform.position;
        return $"{scenePath}|{pos.x:F2},{pos.y:F2},{pos.z:F2}";
    }

    // Reset ID (call manually if needed)
    public void RegenerateId()
    {
        _persistentId = GeneratePersistentId();
    }
}