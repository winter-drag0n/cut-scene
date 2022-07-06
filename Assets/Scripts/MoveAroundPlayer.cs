using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundPlayer : MonoBehaviour
{
    [SerializeField] Rigidbody player;
    [SerializeField] float Distance;
    [SerializeField] float Elevation;
    [SerializeField] GameObject VCamera1;
    [SerializeField] GameObject VCamera2;
    Rigidbody rb2;

    void Start()
    {
        rb2 = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {


    }




}
