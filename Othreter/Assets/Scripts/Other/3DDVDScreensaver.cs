using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsController : MonoBehaviour
{
    [SerializeField] private Vector3 bounds;
    [SerializeField] private bool showBoundriesInEditor = true;
    [SerializeField] private Vector3 center;
    [SerializeField] private float speed = 1.0f;

    private float posX;
    private float posY;
    private float posZ;

    private Vector3 minPos;
    private Vector3 middlePos;
    private Vector3 maxPos;

    private Vector3 targetLocation;

    void Start()
    {
        minPos = new Vector3(center.x - (bounds.x/2), center.y - (bounds.y / 2), center.z - (bounds.z / 2)); //min possible position
        middlePos = new Vector3(((bounds.x / 2) - (bounds.x / 2)) + center.x, ((bounds.y / 2) - (bounds.y / 2)) + center.y, ((bounds.z / 2) - (bounds.z / 2)) + center.z); //center of boundaries
        maxPos = new Vector3(center.x + (bounds.x / 2), center.y + (bounds.y / 2), center.z + (bounds.z / 2)); //max possible position

        transform.position = middlePos; //spawn at center

        //separate values for calculating separate transform position
        posX = transform.position.x;
        posY = transform.position.y;
        posZ = transform.position.z;

        targetLocation = maxPos; //pick starting target
    }

	void Update()
    {
        //X
        if (transform.position.x == targetLocation.x) //if at target location
        {
            if (transform.position.x > middlePos.x) //if over middle position
            {
                targetLocation.x = minPos.x;
            }
            else //if under middle position
            {
                targetLocation.x = maxPos.x;
            }
        }

        //Y
        else if (transform.position.y == targetLocation.y)
        {
            if (transform.position.y > middlePos.y)
			{
                targetLocation.y = minPos.y;
            }
            else
			{
                targetLocation.y = maxPos.y;
            }
        }

        //Z
        else if (transform.position.z == targetLocation.z)
        {
            if (transform.position.z > middlePos.z)
            {
                targetLocation.z = minPos.z;
            }
            else
            {
                targetLocation.z = maxPos.z;
            }
        }

        //Calculate separate position values
        posX = Mathf.MoveTowards(posX, targetLocation.x, speed * Time.deltaTime);
        posY = Mathf.MoveTowards(posY, targetLocation.y, speed * Time.deltaTime);
        posZ = Mathf.MoveTowards(posZ, targetLocation.z, speed * Time.deltaTime);

        //Change transform position to calculated position
        transform.position = new Vector3(posX, posY, posZ);
    }

    //Debug boundaries in editor
    private void OnDrawGizmos()
    {
        if(showBoundriesInEditor)
		{
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(center, bounds);
        }
    }
}
