using Oculus.Interaction;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class GrabbHaptics : MonoBehaviour
{
    public float vibrationIntensity = 0.5f;   
    public float vibrationDuration = 0.075f;
    public AudioClip grabb, drop;
    private AudioSource audioSource;
    public enum Hand { Left, Right };
    public Hand hand;
    private OVRInput.Controller controllerInUse;

    private void Start()
    { 
        audioSource = GetComponent<AudioSource>();
    }
    public void StartHaptics()
    {
        DetectActiveController();
        audioSource.PlayOneShot(grabb);
    }

    public void StopHaptics()
    {
        OVRInput.SetControllerVibration(0, 0, controllerInUse);
        audioSource.PlayOneShot(drop);
    }

    private void TriggerHaptics()
    {
        if (controllerInUse != OVRInput.Controller.None)
        {
            OVRInput.SetControllerVibration(0.8f, vibrationIntensity, controllerInUse);  
            StartCoroutine(StopHapticsAfterDuration(vibrationDuration)); 
        }
    }

    private IEnumerator StopHapticsAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(0, 0, controllerInUse); 
    }
    
    public void DetectActiveController()
    {
        if (hand == Hand.Left)
            controllerInUse = OVRInput.Controller.LTouch;
        else if (hand == Hand.Right)
            controllerInUse = OVRInput.Controller.RTouch;

        if (controllerInUse != OVRInput.Controller.None)
            TriggerHaptics();
        
        else
        {
            controllerInUse = OVRInput.Controller.None;
            Debug.LogWarning("No GetHand script found.");
        }
    }
}