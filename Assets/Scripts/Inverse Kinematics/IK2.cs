using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//Code sourced from https://www.youtube.com/watch?v=qqOAzn05fvk&t=266s
//Commented by qiuand

//This code contains the inverse kinematics algorithm to calculate joint positions relative to an effector, target, and pole

public class IK2 : MonoBehaviour
{
    //Initialize parameters
    public int chainLength = 2;
    public Transform target;
    public Transform pole;
    public int iterations = 10;
    public float delta = 0.001f;
    [Range(0, 1)]
    public float snapBackStrength = 1f;

    //Initialize variables for joint positions and total length
    protected float[] bonesLength;
    protected float completeLength;
    protected Transform[] bones;
    protected Vector3[] positions;

    //Initialize variables to store joint rotations
    protected Vector3[] startDirectionSucc;
    protected Quaternion[] startRotationBone;
    protected Quaternion startRotationTarget;
    protected Quaternion startRotationRoot;
    
    private void Awake()
    {
        //Initialize on awake
        Init();
    }

    //Declare variables on initialize
    private void Init()
    {
        //Set length of joint arrays
        bones = new Transform[chainLength + 1];
        positions = new Vector3[chainLength + 1];
        bonesLength = new float[chainLength];
        startDirectionSucc = new Vector3[chainLength + 1];
        startRotationBone = new Quaternion[chainLength + 1];

        //Make target if target is not set
        if (target == null)
        {
            target = new GameObject(gameObject.name + " Target").transform;
            target.position = transform.position;
        }
        //Set initial rotation of target object
        startRotationTarget = target.rotation;
       
        //Default total length
        completeLength = 0;

        //Set length of segments between bones and record starting rotations
        var current = transform;
        for(var i=bones.Length-1; i>=0; i--)
        {
            bones[i] = current;
            startRotationBone[i] = current.rotation;
            //If current bone is effector
            if (i == bones.Length - 1)
            {
                //Record direction to target
                startDirectionSucc[i] = target.position - current.position; 
            }
            else
            {
                //Else record direction + add length to total length
                startDirectionSucc[i] = bones[i + 1].position - current.position;
                bonesLength[i] = (bones[i + 1].position - current.position).magnitude;
                completeLength += bonesLength[i];
            }
            //Set current to parrent of current
            current = current.parent;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }
/*    private void LateUpdate()
    {
        //Resolve inverse kinematics
        ResolveIK();
        print(1);
    }*/
    //Calculate bone position and rotation
    private void ResolveIK()
    {
        //Break if target doesn't exist
        if (target == null)
        {
            return;
        }
        //Recalculate position/rotation/total length if chain length has changed
        if (bonesLength.Length != chainLength)
        {
            Init();
        }

        //Set position array to bone positions
        for(int i=0; i<bones.Length; i++)
        {
            positions[i] = bones[i].position;
        }
        //Calculate angle difference in newly rotated target
        var rootRot = (bones[0].parent!=null) ? bones[0].parent.rotation : Quaternion.identity;
        var rootRotDiff = rootRot * Quaternion.Inverse(startRotationRoot);

        //If target is too far, set all bones on a direction vector pointing towards it
        if ((target.position - bones[0].position).sqrMagnitude >= completeLength * completeLength)
        {
            var direction = (target.position - positions[0]).normalized;
            for (int i = 1; i < positions.Length; i++)
            {
                positions[i] = positions[i - 1] + direction * bonesLength[i - 1];
            }
        }
        //Else, calculate position/rotation
        else
        {
            //For each iteration
            for (int iteration = 0; iteration < iterations; iteration++)
            {
                //For every bone excluding root
                for(int i=positions.Length-1; i>0; i--)
                {
                    //If bone is effector, set transform to target
                    if (i == positions.Length - 1)
                    {
                        positions[i] = target.position;
                    }
                    //Else, backwards step: set bone at normal length behind child bone
                    else
                    {
                        positions[i] = positions[i + 1] + (positions[i] - positions[i + 1]).normalized * bonesLength[i];
                    }
                }
                //Forwards step: starting at root's child and moving towards effector, set each bone at a standard length and direction 
                for(int i=1; i<positions.Length; i++)
                {
                    positions[i] = positions[i - 1] + (positions[i] - positions[i - 1]).normalized * bonesLength[i - 1];
                }
                //If position to target is close enough, break
                if ((positions[positions.Length - 1] - target.position).sqrMagnitude < delta * delta)
                {
                    break;
                }
            }
        }
        //If pole exists
        if (pole != null)
        {
            //For each bone
            for (int i = 1; i < positions.Length - 1; i++)
            {
                //Find plane perpendicular to previous and next bones, that passes through previous bone
                var plane = new Plane(positions[i + 1] - positions[i - 1], positions[i - 1]);
                //Find closest points to pole and bone on the plane
                var projectedPole = plane.ClosestPointOnPlane(pole.position);
                var projectedBone = plane.ClosestPointOnPlane(positions[i]);
                //Find angle difference between projected pole annd bone relative to plane
                var angle = Vector3.SignedAngle(projectedBone - positions[i - 1], projectedPole - positions[i - 1], plane.normal);
                //Rotate bone by calculated angle using plane as an axis
                positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (positions[i] - positions[i - 1]) + positions[i - 1];
            }
        }

        //For each bone
        for(int i=0; i<positions.Length; i++)
        {
            //If effector, set rotation to target
            if (i == positions.Length - 1)
            {
                bones[i].rotation = target.rotation * Quaternion.Inverse(startRotationTarget) * startRotationBone[i];
            }
            //If joint, set rotation to face next bone
            else
            {
                bones[i].rotation = Quaternion.FromToRotation(startDirectionSucc[i], positions[i + 1] - positions[i]) * startRotationBone[i];
            }
            //Set bone positions
            bones[i].position = positions[i];
        }
/*        for (int i=0; i<positions.Length; i++)
        {
            bones[i].position = positions[i];
        }*/
    }
    // Update is called once per frame
    void Update()
    {
/*        print(1);*/
    }
    //Draw Gizmos function
    private void OnDrawGizmos()
    {
        var current = this.transform;
        for (int i = 0; i < chainLength && current != null && current.parent != null; i++)
        {
            var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }
    }
}
