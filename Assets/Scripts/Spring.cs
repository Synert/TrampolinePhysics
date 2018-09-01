using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float springMax = 5.0f;
    [SerializeField] private float force = 100.0f;
    private float neighborForce = 500.0f;
    private bool doOnce = false;
    private Rigidbody rb;
    private Vector3 origin;

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

            foreach(Collider spring in Physics.OverlapSphere(transform.position + new Vector3(0.0f, springMax), 1.0f))
            {
                if (spring.gameObject == GetComponentInChildren<Rigidbody>().gameObject) continue;
                SpringJoint newJoint = spring.gameObject.AddComponent<SpringJoint>();
                newJoint.anchor = Vector3.zero;
                newJoint.connectedAnchor = Vector3.zero;
                newJoint.connectedBody = GetComponentInChildren<Rigidbody>();
                newJoint.spring = neighborForce;
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
}
