using UnityEngine;

public class RigToPlayer : MonoBehaviour
{
    private Transform player;
    void Start()
    {
        player = FindFirstObjectByType<OVRManager>().transform;   
    }
    void Update()
    {
        transform.position = player.position;
    }
}
