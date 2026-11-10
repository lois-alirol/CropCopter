using UnityEngine;

public class FollowDrone : MonoBehaviour
{
    public GameObject drone;
    public Vector3 offset;
    
    void Update() 
    {
        if (drone)
        {
            transform.position = drone.transform.position + offset;
        }
    }
}