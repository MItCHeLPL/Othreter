using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsController : MonoBehaviour
{
    [SerializeField] private Vector3 bounds;
    [SerializeField] private Vector3 center;
    [SerializeField] private float speed = 1.0f;

    private float posX;
    private float posY;
    private float posZ;

    private Vector3 targetLocation;
    
    private Vector3 minPos;
    private Vector3 middlePos;
    private Vector3 maxPos;

    private Vector3 offset;

    void Start()
    {
        minPos = new Vector3(center.x - (bounds.x/2), center.y - (bounds.y / 2), center.z - (bounds.z / 2)); //min possible position
        middlePos = new Vector3(((bounds.x / 2) - (bounds.x / 2)) + center.x, ((bounds.y / 2) - (bounds.y / 2)) + center.y, ((bounds.z / 2) - (bounds.z / 2)) + center.z); //center of boundaries
        maxPos = new Vector3(center.x + (bounds.x / 2), center.y + (bounds.y / 2), center.z + (bounds.z / 2)); //max possible position

        PickNewOffset();

        transform.position = middlePos; //spawn at center

        //separate values for calculating separate transform position
        posX = transform.position.x;
        posY = transform.position.y;
        posZ = transform.position.z; 

        targetLocation = new Vector3(middlePos.x + offset.x, middlePos.y + offset.y, middlePos.z + offset.z); //first target position with offset

        //targetLocation = new Vector3((bounds.x / 2) + center.x, (bounds.y / 2) + center.y, (bounds.z / 2) + center.z); 

        //temp
        Debug.Log(minPos);
        Debug.Log(middlePos);
        Debug.Log(maxPos);
        Debug.Log(offset);
        Debug.Log(targetLocation);
    }

	private void OnDrawGizmos() //temp
	{
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, bounds);
    }

	void Update()
    {
        //Calculate separate position values
        posX = Mathf.MoveTowards(posX, targetLocation.x, speed * Time.deltaTime);
        posY = Mathf.MoveTowards(posY, targetLocation.y, speed * Time.deltaTime);
        posZ = Mathf.MoveTowards(posZ, targetLocation.z, speed * Time.deltaTime);

        //X
        if (transform.position.x == targetLocation.x) //if at target location
        {
            //PickNewOffset();

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
            //PickNewOffset();

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
            //PickNewOffset();

            if (transform.position.z > middlePos.z)
            {
                targetLocation.z = minPos.z;
            }
            else
            {
                targetLocation.z = maxPos.z;
            }
        }

        //Change transform position to calculated position
        transform.position = new Vector3(posX, posY, posZ); //works
    }

    private void PickNewOffset()
	{
        float x = Random.Range(minPos.x, maxPos.x);
        float y = Random.Range(minPos.y, maxPos.y);
        float z = Random.Range(minPos.z, maxPos.z);

        offset = new Vector3(x, y, z); //offset for target position
    }
}
