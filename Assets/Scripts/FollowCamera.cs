using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class FollowCamera : MonoBehaviour
{
    [SerializeField] CinemachineBrain CBrain;


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = CBrain.transform.rotation;
    }
}
