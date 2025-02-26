using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VRVignetteEffect : MonoBehaviour
{
    public Transform playerTransform; 
    public Volume volume; 

    private Vignette vignette;
    private float targetVignette = 0f;
    private Vector3 lastPosition; 

    void Start()
    {
        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true; 
            vignette.intensity.value = 0f; 
        }

        lastPosition = playerTransform.position;
    }

    void Update()
    {
        float speed = (playerTransform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = playerTransform.position;

        targetVignette = Mathf.Clamp(speed * 5, 0f, 1);
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetVignette, Time.deltaTime * 5f);
    }
}
