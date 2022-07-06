using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScriptControl : MonoBehaviour
{
    public enum CameraFollowType { FreeRoam, Follow, Zoom, Shake, Circular, FrontTransition, FollowC, ZoomC, ShakeC, CircularC, FrontC , none }
    [SerializeField] Rigidbody Player;
    [SerializeField] Vector3 ExpectedFollowDistance;
    [SerializeField] Vector3 ZoomExpectedFollowDistance;
    [SerializeField] Vector3 FrontCamLocation;
    [SerializeField] float ShakeAmplitudes;
    [SerializeField] float ShakeFrequency;
    [SerializeField] float NormalMoveSpeed;
    [SerializeField] float RotateSpeed;
    [SerializeField] CinemachineVirtualCamera Vcam,ShakeCam,CircularCam;
    CinemachineBrain cbrain;
    bool LookAtPlayer;
    float MoveSpeed;
    Rigidbody rb;
    public CameraFollowType FollowType;
    bool Clock { get { return Mathf.FloorToInt(Time.fixedTime * ShakeFrequency) % 2 == 0; } }
    bool PastClock;
    Vector3 NoiseDelta = Vector3.zero;
    Vector3 pastMousePosition;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        cbrain = GetComponent<CinemachineBrain>();
        LookAtPlayer = true;
        MoveSpeed = NormalMoveSpeed;
        rb = GetComponent<Rigidbody>();
        FollowType = CameraFollowType.Follow;
        pastMousePosition = Input.mousePosition;
    }

    void FixedUpdate()
    {
        if ( Input.GetKeyDown(KeyCode.Escape) )
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        if ( FollowType == CameraFollowType.Shake)
        {
            if (Clock != PastClock)
            {
                NoiseDelta = new Vector3(1 - 2 * Random.value, 1 - 2 * Random.value, 1 - 2 * Random.value);
                PastClock = Clock;
            }
        }
        else
        {
            NoiseDelta = Vector3.zero;
        }

        if ( LookAtPlayer )
        {
            Vector3 del = (Player.position - rb.position + new Vector3(0,1.5f,0)) ;
            Vector3 delxz = Vector3.Scale(del, new Vector3(1, 0, 1));
            Vector3 perp = Vector3.Cross(Vector3.Cross(delxz, del) + NoiseDelta*ShakeAmplitudes, del);
            perp = perp * Mathf.Sign(perp.y);
            Quaternion q = Quaternion.LookRotation(del, perp);
            rb.rotation = Quaternion.Slerp(rb.rotation, q, Time.deltaTime );
        }

        switch( FollowType )
        {
            case CameraFollowType.FreeRoam:
                LookAtPlayer = false;
                MoveByInput();
                TurnByInput();
                cbrain.enabled = false;
                break;

            case CameraFollowType.Follow:
                MoveSpeed = NormalMoveSpeed;
                LookAtPlayer = true;
                rb.position = Vector3.Slerp(rb.position, Player.position + MoveByAxes(ExpectedFollowDistance)  , Time.deltaTime * MoveSpeed);
                cbrain.enabled = false;
                break;

            case CameraFollowType.Zoom:
                MoveSpeed = 4 * NormalMoveSpeed;
                LookAtPlayer = true;
                rb.position = Vector3.Slerp(rb.position, Player.position + MoveByAxes(ZoomExpectedFollowDistance), Time.deltaTime * MoveSpeed);
                cbrain.enabled = false;
                break;

            case CameraFollowType.Shake:
                MoveSpeed = NormalMoveSpeed;
                LookAtPlayer = true;
                rb.position = Vector3.Slerp(rb.position, Player.position + MoveByAxes(ExpectedFollowDistance) , Time.deltaTime * MoveSpeed);
                cbrain.enabled = false;
                break;

            case CameraFollowType.Circular:
                LookAtPlayer = true;
                rb.position = Vector3.Slerp(rb.position, Player.position + MoveByAxes(Vector3.Scale( new Vector3(ExpectedFollowDistance.z,ExpectedFollowDistance.y, ExpectedFollowDistance.z) , new Vector3(Mathf.Cos(0.2f*Time.time), 1, Mathf.Sin(0.2f*Time.time)) )), Time.deltaTime * MoveSpeed);
                cbrain.enabled = false;
                break;

            case CameraFollowType.FrontTransition:
                MoveSpeed = 4 * NormalMoveSpeed;
                LookAtPlayer = true;
                rb.position = Vector3.Slerp(rb.position, Player.position + MoveByAxes(FrontCamLocation), Time.deltaTime * MoveSpeed);
                cbrain.enabled = false;
                break;

            case CameraFollowType.FollowC:
                cbrain.enabled = true;
                SetVCS(true, false, false);
                Cinemachine3rdPersonFollow ct = Vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                ct.ShoulderOffset = ExpectedFollowDistance;
                CinemachineComposer ctc = Vcam.GetCinemachineComponent<CinemachineComposer>();
                Vcam.LookAt = Player.transform;
                Vcam.Follow = Player.transform;
                break;

            case CameraFollowType.ZoomC:
                SetVCS(true, false, false);
                cbrain.enabled = true;
                Cinemachine3rdPersonFollow ct1 = Vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                ct1.ShoulderOffset = ZoomExpectedFollowDistance;
                CinemachineComposer ctc1 = Vcam.GetCinemachineComponent<CinemachineComposer>();
                Vcam.LookAt = Player.transform;
                Vcam.Follow = Player.transform;
                break;
            case CameraFollowType.ShakeC:
                cbrain.enabled = true;
                SetVCS(false, false, true);
                break;
            case CameraFollowType.CircularC:
                cbrain.enabled = true;
                SetVCS(false, true, false);
                break;
            case CameraFollowType.FrontC:
                cbrain.enabled = true;
                SetVCS(true, false, false);
                Cinemachine3rdPersonFollow ct3 = Vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                ct3.ShoulderOffset = FrontCamLocation;
                CinemachineComposer ctc3 = Vcam.GetCinemachineComponent<CinemachineComposer>();
                Vcam.LookAt = Player.transform;
                Vcam.Follow = Player.transform;
                break;

        }



    }


    void MoveByInput ( )
    {
        rb.position += MoveSpeed * Time.deltaTime * (Input.GetKey(KeyCode.U) ? transform.forward : Vector3.zero);
        rb.position += MoveSpeed * Time.deltaTime * (Input.GetKey(KeyCode.J) ? -transform.forward : Vector3.zero);
        rb.position += MoveSpeed * Time.deltaTime * (Input.GetKey(KeyCode.K) ? transform.right : Vector3.zero);
        rb.position += MoveSpeed * Time.deltaTime * (Input.GetKey(KeyCode.H) ? -transform.right : Vector3.zero);
    }

    void TurnByInput()
    {
        Vector3 Delr = Input.mousePosition - pastMousePosition;
        Quaternion q = rb.rotation;
        q.eulerAngles = q.eulerAngles + RotateSpeed * new Vector3(-Delr.y, Delr.x, 0);
        rb.rotation = q;
        pastMousePosition = Input.mousePosition;
    }

    Vector3 MoveByAxes( Vector3 expectedDist )
    {
        return Player.transform.right * expectedDist.x + Player.transform.up * expectedDist.y + Player.transform.forward * expectedDist.z;
    }

    public void SetCameraType(int cft)
    {
        FollowType = (CameraFollowType)cft;
    }

    void SetVCS(bool vcam , bool ccam , bool scam )
    {
        Vcam.gameObject.SetActive(vcam);
        CircularCam.gameObject.SetActive(ccam);
        ShakeCam.gameObject.SetActive(scam);
    }


}




