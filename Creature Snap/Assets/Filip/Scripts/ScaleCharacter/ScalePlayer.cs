using UnityEngine;

public class ScalePlayer : MonoBehaviour
{
    public float playerHeight;
    public float baseHeight = 1.84f;
    public float playerScale;
    public Transform playerModel;
    public void SetScaleFromHeight()
    {
        float scaleFactor = playerHeight / baseHeight;
        playerModel.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

}
