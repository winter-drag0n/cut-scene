using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class CharControl : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float Acceleration;
    [SerializeField] float MaxSpeed;
    [SerializeField] float TurnRate;
    [SerializeField] Renderer Mrenderer;
    [SerializeField] Collider Hand;


    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }


    void Movement()
    {
        Profiler.BeginSample("CharControlFixedUpdate");

        if (Input.GetKey(KeyCode.UpArrow) && Vector2.Dot(rb.velocity, transform.forward) < MaxSpeed)
        {
            rb.AddForce(transform.forward * Acceleration);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Vector2.Dot(rb.velocity, -transform.forward) < MaxSpeed)
        {
            rb.AddForce(-transform.forward * Acceleration);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.angularVelocity = new Vector3(0, TurnRate, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.angularVelocity = new Vector3(0, -TurnRate, 0);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }

        anim.SetBool("Pick", Input.GetKey(KeyCode.D));
        anim.SetBool("Waving", Input.GetKey(KeyCode.A));
            

        float speed = Dotxz(rb.velocity, transform.forward);
        anim.SetFloat("Speed", speed);

        Profiler.EndSample();
    }


    float Dotxz( Vector3 a , Vector3 b )
    {
        return a.x * b.x + b.z * a.z;
    }



    void KillerCode()
    {
        for ( int i = 0; i < 200; i ++ )
        {
            Debug.Log(Mrenderer.material.GetTexture("_BaseMap"));

        }
    }

    public void StartPick()
    {
        Hand.enabled = true;
    }

    public void StopPick()
    {
        Hand.enabled = false;
    }



}
