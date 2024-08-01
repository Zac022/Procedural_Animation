using System.Collections.Generic;
using UnityEngine;

public class ProceduralWalker : MonoBehaviour
{
    public Transform leftHip, leftKnee, leftAnkle, rightHip, rightKnee, rightAnkle, leftShoulder, rightShoulder, head, spine;
    public float walkSpeed = 2f;
    public float stepCycle = 0.5f;
    public float stepHeight = 0.1f;

    public float armSwingAmplitude = 0.2f;
    public float headBobAmplitude = 0.05f;
    public float spineAngle = 10f;
    public float KneeAngle = 25f;
    public float MovementSpeed = 3.0f;
    public float waitTimeTurn = 0.2f;
    public float walkStepCycle = 0.1f;

    private float timeSinceStep = 0f;
    private float rotationAngle = 90f;
    private List<float> rotateAngle = new List<float>() { 90f, -90f };

    private float walkCycle = 0f;
    private float turnCycle = 0f;
    private bool canTurn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("box"))
        {
            canTurn = true;
            Debug.Log("Trigger Entered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("box"))
        {
            canTurn = false;
        }
    }


    void Update()
    {
        PreceduralMovement();
        PlayerMovement();

        if (canTurn)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(transform.localRotation.x, rotateAngle[Random.Range(0,2)], transform.localRotation.z), Time.deltaTime);

        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(transform.localRotation.x, -90f, transform.localRotation.z), Time.deltaTime);
        }

        
    }

    private void PlayerMovement()
    {
        transform.position += -transform.right * (MovementSpeed) * Time.deltaTime;
    }

    private void PreceduralMovement()
    {
        timeSinceStep += Time.deltaTime;

        walkCycle = Mathf.Repeat(timeSinceStep / stepCycle, 1f);
      //  turnCycle = Mathf.Repeat(timeSinceStep, 10f);

        float leftLegHeight = Mathf.Sin(walkCycle * Mathf.PI * 2) * stepHeight;
        float rightLegHeight = Mathf.Sin((walkCycle + 0.5f) * Mathf.PI * 2) * stepHeight;

        leftAnkle.localPosition = new Vector3(leftAnkle.localPosition.x, leftLegHeight, 0);
        rightAnkle.localPosition = new Vector3(rightAnkle.localPosition.x, rightLegHeight, 0);

        float leftLegAngle = Mathf.Sin(walkCycle * Mathf.PI * 2) * KneeAngle;
        float rightLegAngle = Mathf.Sin((walkCycle + 0.5f) * Mathf.PI * 2) * KneeAngle;

        //leftHip.localRotation = Quaternion.Euler(leftLegAngle * 0.5f, 0, 0);
        //rightHip.localRotation = Quaternion.Euler(rightLegAngle * 0.5f, 0, 0);

        leftKnee.localRotation = Quaternion.Euler(-leftLegAngle * 1.5f, 0, 0);
        rightKnee.localRotation = Quaternion.Euler(-rightLegAngle * 1.5f, 0, 0);

        float leftArmAngle = Mathf.Sin(walkCycle * Mathf.PI * 2) * armSwingAmplitude;
        float rightArmAngle = Mathf.Sin((walkCycle + 0.5f) * Mathf.PI * 2) * armSwingAmplitude;
        leftShoulder.localRotation = Quaternion.Euler(leftShoulder.localRotation.x, leftShoulder.localRotation.y, leftArmAngle);
        rightShoulder.localRotation = Quaternion.Euler(0, 0, -rightArmAngle);

        float headBob = Mathf.Sin(walkCycle * Mathf.PI * 2) * headBobAmplitude;
        head.localPosition = new Vector3(0, headBob, 0);

        float spineAngleOffset = Mathf.Sin(walkCycle * Mathf.PI * 2) * spineAngle;
        spine.localRotation = Quaternion.Euler(spineAngleOffset, 0, 0);
    }
}
