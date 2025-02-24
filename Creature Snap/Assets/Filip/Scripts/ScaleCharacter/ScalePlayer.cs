using UnityEngine;

public class ScalePlayer : MonoBehaviour
{
    public Transform playerModel; // Assign your player model here
    public float baseHeight = 1.75f; // Reference height in meters

    public void SetPlayerHeight(float userHeight)
    {
        float scale = userHeight / baseHeight;
        playerModel.localScale = new Vector3(scale, scale, scale);
    }
}
