using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float springMax = 5.0f;
    [SerializeField] private float force = 100.0f;
    private List<Transform> neighbors;
    private float neighborForce = 500.0f;
    private bool doOnce = false;
    private Rigidbody rb;
    private Vector3 origin;
    private float dist = 1.0f;

	void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        GetComponentInChildren<SpringJoint>().connectedAnchor = new Vector3(0.0f, springMax) + transform.position;
        GetComponentInChildren<SpringJoint>().spring = force;
        rb.transform.position = new Vector3(0.0f, springMax) + transform.position;
	}
	
	void Update()
    {
		if(!doOnce)
        {
            doOnce = true;

            neighbors = new List<Transform>();

            foreach(Collider spring in Physics.OverlapSphere(rb.transform.position, dist))
            {
                if (spring.gameObject == GetComponentInChildren<Rigidbody>().gameObject) continue;
                neighbors.Add(spring.GetComponentInParent<Rigidbody>().transform);
                /*SpringJoint newJoint = spring.gameObject.AddComponent<SpringJoint>();
                newJoint.anchor = Vector3.zero;
                newJoint.connectedAnchor = Vector3.zero;
                newJoint.connectedBody = GetComponentInChildren<Rigidbody>();
                newJoint.spring = neighborForce;*/
            }
        }

        foreach(Transform neighbor in neighbors)
        {
            float desiredHeight = rb.transform.position.y;
            float yDist = desiredHeight - neighbor.position.y;
            if (yDist < 0.0f)
            {
                neighbor.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, yDist * neighborForce * 0.03f, 0.0f), ForceMode.VelocityChange);
                //neighbor.position += new Vector3(0.0f, yDist * 0.25f, 0.0f);
            }
            else
            {
                neighbor.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, yDist * neighborForce * 0.005f, 0.0f), ForceMode.VelocityChange);
                //neighbor.position += new Vector3(0.0f, yDist * 0.1f, 0.0f);
            }
        }
    }

    public void SetForce(float newForce, float newNeighbor)
    {
        force = newForce;
        GetComponentInChildren<SpringJoint>().spring = newForce;
        neighborForce = newNeighbor;
    }

    public void SetOrigin(Vector3 newOrigin)
    {
        origin = newOrigin;
    }

    public void SetDistance(float newDistance)
    {
        dist = newDistance;
    }
}
