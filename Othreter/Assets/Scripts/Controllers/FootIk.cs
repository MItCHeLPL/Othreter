using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIk : MonoBehaviour
{
	private Animator anim;

	//Foot IK
	private Vector3 rightFootPosition, leftFootPosition, leftFootIkPosition, rightFootIkPosition;
	private Quaternion leftFootIkRotation, rightFootIkRotation;
	private float lastPelvisPositionY, lastRightFootPositionY, lastLeftFootPositionY;

	[Header("Feet Grounder")]
	public bool enableFeetIk = true;
	[Range(0, 2)] [SerializeField] private float heightFromGroundRaycast = 1.14f;
	[Range(0, 2)] [SerializeField] private float raycastDownDistance = 0.875f;
	[SerializeField] private LayerMask environmentLayer;
	[SerializeField] private float pelvisOffset = 0f;
	[Range(0, 1)] [SerializeField] private float pelvisUpAndDownSpeed = 0.28f;
	[Range(0, 1)] [SerializeField] private float feetToIkPositionSpeed = 0.5f;

	public string leftFootAnimVariableName = "LeftFootCurve";
	public string rightFootAnimVariableName = "RightFootCurve";

	public bool useProIkFeature = false;
	public bool showSolverDebug = true;

	private float rDistance = 0.0f;
	private float lDistance = 0.0f;

	[HideInInspector]
	public float distance = 0.0f;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	//Foot IK
	#region FeetGrounding

	/// <summary>
	/// We are updating the AdjustFeetTarget method and also find the position of each foot inside our Solver Position.
	/// </summary>
	private void FixedUpdate()
	{
		if (enableFeetIk == false) { return; }
		if (anim == null) { return; }

		AdjustFeetTarget(ref rightFootPosition, HumanBodyBones.RightFoot);
		AdjustFeetTarget(ref leftFootPosition, HumanBodyBones.LeftFoot);

		//find and raycast to the ground to find positions
		rDistance = FeetPositionSolver(rightFootPosition, ref rightFootIkPosition, ref rightFootIkRotation); // handle the solver for right foot
		lDistance = FeetPositionSolver(leftFootPosition, ref leftFootIkPosition, ref leftFootIkRotation); //handle the solver for the left foot

		if(rDistance > lDistance)
		{
			distance = rDistance;
		}
		else if(lDistance > rDistance)
		{
			distance = lDistance;
		}
		else
		{
			distance =  rDistance;
		}

		distance -= heightFromGroundRaycast;

		if(Mathf.Abs(distance) > raycastDownDistance)
		{
			distance = 0;
		}
	}

	private void OnAnimatorIK(int layerIndex)
	{
		if (enableFeetIk == false) { return; }
		if (anim == null) { return; }

		MovePelvisHeight();

		//right foot ik position and rotation -- utilise the pro features in here
		anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

		if (useProIkFeature)
		{
			anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat(rightFootAnimVariableName));
		}

		MoveFeetToIkPoint(AvatarIKGoal.RightFoot, rightFootIkPosition, rightFootIkRotation, ref lastRightFootPositionY);

		//left foot ik position and rotation -- utilise the pro features in here
		anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

		if (useProIkFeature)
		{
			anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat(leftFootAnimVariableName));
		}

		MoveFeetToIkPoint(AvatarIKGoal.LeftFoot, leftFootIkPosition, leftFootIkRotation, ref lastLeftFootPositionY);
	}

	#endregion



	#region FeetGroundingMethods

	/// <summary>
	/// Moves the feet to ik point.
	/// </summary>
	/// <param name="foot">Foot.</param>
	/// <param name="positionIkHolder">Position ik holder.</param>
	/// <param name="rotationIkHolder">Rotation ik holder.</param>
	/// <param name="lastFootPositionY">Last foot position y.</param>
	void MoveFeetToIkPoint(AvatarIKGoal foot, Vector3 positionIkHolder, Quaternion rotationIkHolder, ref float lastFootPositionY)
	{
		Vector3 targetIkPosition = anim.GetIKPosition(foot);

		if (positionIkHolder != Vector3.zero)
		{
			targetIkPosition = transform.InverseTransformPoint(targetIkPosition);
			positionIkHolder = transform.InverseTransformPoint(positionIkHolder);

			float yVariable = Mathf.Lerp(lastFootPositionY, positionIkHolder.y, feetToIkPositionSpeed);
			targetIkPosition.y += yVariable;

			lastFootPositionY = yVariable;

			targetIkPosition = transform.TransformPoint(targetIkPosition);

			anim.SetIKRotation(foot, rotationIkHolder);
		}

		anim.SetIKPosition(foot, targetIkPosition);
	}
	/// <summary>
	/// Moves the height of the pelvis.
	/// </summary>
	private void MovePelvisHeight()
	{

		if (rightFootIkPosition == Vector3.zero || leftFootIkPosition == Vector3.zero || lastPelvisPositionY == 0)
		{
			lastPelvisPositionY = anim.bodyPosition.y;
			return;
		}

		float lOffsetPosition = leftFootIkPosition.y - transform.position.y;
		float rOffsetPosition = rightFootIkPosition.y - transform.position.y;

		float totalOffset = (lOffsetPosition < rOffsetPosition) ? lOffsetPosition : rOffsetPosition;

		Vector3 newPelvisPosition = anim.bodyPosition + Vector3.up * totalOffset;

		newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

		anim.bodyPosition = newPelvisPosition;

		lastPelvisPositionY = anim.bodyPosition.y;

	}

	/// <summary>
	/// We are locating the Feet position via a Raycast and then Solving
	/// </summary>
	/// <param name="fromSkyPosition">From sky position.</param>
	/// <param name="feetIkPositions">Feet ik positions.</param>
	/// <param name="feetIkRotations">Feet ik rotations.</param>
	private float FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIkPositions, ref Quaternion feetIkRotations)
	{
		//raycast handling section 
		RaycastHit feetOutHit;

		if (showSolverDebug)
			Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.yellow);

		if (Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit, raycastDownDistance + heightFromGroundRaycast, environmentLayer))
		{
			//finding our feet ik positions from the sky position
			feetIkPositions = fromSkyPosition;
			feetIkPositions.y = feetOutHit.point.y + pelvisOffset;
			feetIkRotations = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;

			return feetOutHit.distance;
		}

		feetIkPositions = Vector3.zero; //it didn't work :(
		return 0;

	}
	/// <summary>
	/// Adjusts the feet target.
	/// </summary>
	/// <param name="feetPositions">Feet positions.</param>
	/// <param name="foot">Foot.</param>
	private void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot)
	{
		feetPositions = anim.GetBoneTransform(foot).position;
		feetPositions.y = transform.position.y + heightFromGroundRaycast;

	}

	#endregion
}
