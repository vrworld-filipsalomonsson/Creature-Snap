using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VRVignetteEffect : MonoBehaviour
{
    public Transform playerTransform;
    public Volume volume;
    public SkinnedMeshRenderer skinnedMeshRenderer; // The mesh to change material
    public Material movingMaterial; // Material when moving
    public Material idleMaterial; // Material when idle

    private Vignette vignette;
    private float targetVignette = 0f;
    private Vector3 lastPosition;
    private bool isMoving = false;

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

        bool currentlyMoving = speed > 0.1f; 

        if (currentlyMoving != isMoving)
        {
            isMoving = currentlyMoving;
            if (skinnedMeshRenderer != null && movingMaterial != null && idleMaterial != null)
            {
                skinnedMeshRenderer.material = isMoving ? movingMaterial : idleMaterial;
            }
        }
    }
}
