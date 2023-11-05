using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This code calculates and moves targets for effector joints in a walking motion
//Code sourced from: https://www.youtube.com/watch?v=acMK93A-FSY

public class MoveTargets : MonoBehaviour
{
    //Initialize variables

    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform body = default;
    [SerializeField] MoveTargets otherFoot = default;
    [SerializeField] float speed = 1;
    [SerializeField] float stepDistance = 4;
    [SerializeField] float stepLength = 4;
    [SerializeField] float stepHeight = 1;
    [SerializeField] Vector3 footOffset = default;
    public float footSpacing;
    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;
    float lerp;

    private void Start()
    {
        //Get position of foot relative to parent body
        footSpacing = transform.localPosition.x;
        //Declare all positions
        currentPosition = newPosition = oldPosition = transform.position;
        //Declare all rotations
        currentNormal = newNormal = oldNormal = transform.up;
        //Lerp
        lerp = 1;
    }

    // Update is called once per frame

    void Update()
    {
        //Set positions to target current position
        transform.position = currentPosition;
        transform.up = currentNormal;

        //Draw ray from each leg
        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);

        //If hits ground
        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
        {
            //If other foot is not moving and not already lerping, and step distance is exceeded
            if (Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
            {
                //Set lerp value to 0
                lerp = 0;
                //Set direction depending on ray point; positive for forwards, and negative for backwards
                int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;

                //Calculate new forward foot target
                newPosition = info.point + (body.forward * stepLength * direction) + footOffset/*+new Vector3(10,0,0)*/;

                //set target angle adjacent to ground
                newNormal = info.normal;
            }
        }

        //If destination has not been reached
        if (lerp < 1)
        {
            //Lerp between old and new position
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);

            //Adjust foot y position based on sinusoidal arc
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            //Set current position to lerp
            currentPosition = tempPosition;

            //Lerp between old and new angle
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);

            //Iterate on lerp based on speed of movement
            lerp += Time.deltaTime * speed;
        }
        else
        {
            //Set position to static
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    //Draw target Gizmos
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.5f);
    }


    //Return true if lerp is less than 1
    public bool IsMoving()
    {
        return lerp < 1;
    }



}
