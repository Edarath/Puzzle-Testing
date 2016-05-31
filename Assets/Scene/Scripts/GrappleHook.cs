using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class GrappleHook : MonoBehaviour {
    Rigidbody rb;
    public float speed;
    public float reelSpeed;
    bool inAir;
    bool anchored;
    bool shot;
    bool reload;
    private HingeJoint contactHinge;
    public GameObject player;
    public Rigidbody playerRB;
    private FirstPersonController controller;

    void OnCollisionEnter(Collision col)
    {
        if (shot == false)
        {
            shot = true;
            rb.velocity = Vector3.zero;
            contactHinge = gameObject.AddComponent<HingeJoint>();
            contactHinge.connectedBody = col.rigidbody;
            anchored = true;
            inAir = false;
            playerRB.isKinematic = false;
            playerRB.useGravity = false;
        } 
    }
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        inAir = false;
        shot = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && inAir == false)
        {
            inAir = true;
            rb.isKinematic = false;
            rb.AddForce(transform.forward * speed);
            transform.parent = null;
        }
        if (anchored == true)
        {
            controller = FindObjectOfType<FirstPersonController>();
            controller.m_GravityMultiplier = (0);
            controller.m_StickToGroundForce = (0);
            //playerRB.AddForce(transform.position * reelSpeed);

            float step = reelSpeed * Time.deltaTime;
            player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, step);
        }
        if (Input.GetMouseButtonDown(1))
        {
            anchored = false;
            inAir = false;
            rb.isKinematic = true;
            reload = true;
            shot = false;
            transform.SetParent(GameObject.Find("grapple_gun").GetComponent<Transform>());
            transform.position = GameObject.Find("grapple_reload").GetComponent<Transform>().position;
            transform.rotation = GameObject.Find("grapple_reload").GetComponent<Transform>().rotation;
            Destroy(contactHinge);
            //controller.m_GravityMultiplier = (2);
            //controller.m_StickToGroundForce = (10);
        }
    }
}
