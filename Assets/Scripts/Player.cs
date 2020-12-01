using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Effects;

public class Player : MonoBehaviour
{
    [Header("General")]
    [Tooltip("In ms^-1")] [SerializeField] float speed = 4f; // serialize the xSpeed in meter per second
    [Tooltip("In m")] [SerializeField] float xRange = 4f;
    [Tooltip("In m")] [SerializeField] float yRange = 2f;
    [SerializeField] GameObject[] guns;

    [Header("Screen control based")]
    [SerializeField] float positionPitchFactor = 5f;
    [SerializeField] float controlPitchFactor = -30f;

    [Header("Control throw based")]
    [SerializeField] float controlYawFactor = 5f;
    [SerializeField] float controlRollFactor = -20f;

    float xThrow, yThrow;
    bool isControlEnable = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isControlEnable)
        {
            movement();
            rotation();
            fire();
        }
        
    }

    void movement()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        float xOffset = xThrow * speed * Time.deltaTime;
        float yOffset = yThrow * speed * Time.deltaTime;

        float rawXPosition = transform.localPosition.x + xOffset;
        float rawYPosition = transform.localPosition.y + yOffset;

        float xPosition = Mathf.Clamp(rawXPosition, -xRange, xRange);
        float yPosition = Mathf.Clamp(rawYPosition, -yRange, yRange);

        transform.localPosition = new Vector3(xPosition, yPosition, transform.localPosition.z);
    }

    void rotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yaw = transform.localPosition.x * controlYawFactor;
        float roll = xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch,yaw, roll);
    }

    void OnPlayerDeath()
    {
        isControlEnable = false;
    }

    private void fire()
    {
        if (CrossPlatformInputManager.GetButton("fire"))
        {
            ActivateGunSignal(true);
        }
        else
        {
            ActivateGunSignal(false);
        }
    }

    private void ActivateGunSignal(bool fire)
    {
        foreach(GameObject gun in guns)
        {
            ParticleSystem.EmissionModule emmisionModule = gun.GetComponent<ParticleSystem>().emission;
            emmisionModule.enabled = fire;
        }
    }
}
