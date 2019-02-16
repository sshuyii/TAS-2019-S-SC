using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class ThirdPersonCameraController : MonoBehaviour {

    #region Internal References
    private Transform _app;
    private Transform _view;
    private Transform _cameraBaseTransform;
    private Transform _cameraTransform;
    private Transform _cameraLookTarget;
    private Transform _avatarTransform;
    private Rigidbody _avatarRigidbody;
    #endregion

    #region Public Tuning Variables
    public Vector3 avatarObservationOffset_Base;
    public float followDistance_Base;
    public float verticalOffset_Base;
    public float pitchGreaterLimit;
    public float pitchLowerLimit;
    public float fovAtUp;
    public float fovAtDown;
    public float fov_Base;
    public Camera Camera;
    #endregion

    #region Persistent Outputs
    //Positions
    private Vector3 _camRelativePostion_Auto;

    //Directions
    private Vector3 _avatarLookForward;

    //Scalars
    private float _followDistance_Applied;
    private float _verticalOffset_Applied;
    private float _fieldOfView_Applied;

    
    //States
    private CameraStates _currentState;
    #endregion

    private void Start()
    {
        _currentState = CameraStates.Automatic;
        Camera.main.fieldOfView = fov_Base;
    }
    
    
    private void Awake()
    {
        _app = GameObject.Find("Application").transform;
        _view = _app.Find("View");
        _cameraBaseTransform = _view.Find("CameraBase");
        _cameraTransform = _cameraBaseTransform.Find("Camera");
        _cameraLookTarget = _cameraBaseTransform.Find("CameraLookTarget");

        _avatarTransform = _view.Find("AIThirdPersonController");
        _avatarRigidbody = _avatarTransform.GetComponent<Rigidbody>();
    }

    private float _idleTimer;  
   
    private void Update()
    {
        Camera.main.fieldOfView = fov_Base;
        
        //deciding when in which state
        if (Input.GetMouseButton(1))
        {
            _currentState = CameraStates.Manual;
        }
        else if (!Input.GetMouseButton(1) && _idleTimer > 1)
        {
            _currentState = CameraStates.Idle;
        }
        else
        {_currentState = CameraStates.Automatic;}

        //what should the camera do under each state
        if (_currentState == CameraStates.Automatic)
        {
            _AutoUpdate();//which contains moving the camera from walking to standing to walking
        }
        else if (_currentState == CameraStates.Manual)
        {
            _ManualUpdate();
        }
        else{_IdleUpdate();}
        print("idleTimer = " + _idleTimer);
        
//        if (Input.GetMouseButton(1))
//            _ManualUpdate();
//        else
//            _AutoUpdate();
    }

    #region States

    private int TimerLookAtOOI;
    private void _AutoUpdate()
    {
        _ComputeData();
        
        if (_Helper_IsThereOOI())
        {
            TimerLookAtOOI = 100;
        }

        if (TimerLookAtOOI > 50)
        {
            _LookAtObject(_Helper_WhatIsClosestOOI());

        }
        else if(TimerLookAtOOI > 0)
        {
            _LookAwayFromObject();
        }
        else
        {
            _FollowAvatar();
            _LookAtAvatar();
        }
        
        
        
    }
    private void _ManualUpdate()
    {
        _FollowAvatar();
        _ManualControl();
    }
    private void _IdleUpdate()
    {
        
    }
    #endregion

    #region Internal Logic

    float _standingToWalkingSlider = 0;
    float _OOISlider = 0;


    private void _ComputeData()
    {
        //where the avatar is facing
        _avatarLookForward = Vector3.Normalize(Vector3.Scale(_avatarTransform.forward, new Vector3(1, 0, 1)));

            if (_Helper_IsWalking())
            {
                //if the avatar is walking, the variable approaches to 1
                _standingToWalkingSlider = Mathf.MoveTowards(_standingToWalkingSlider, 1, Time.deltaTime);
            }
            else
            {
                //if the avatar is standing still, the variable approaches to 0
                _standingToWalkingSlider = Mathf.MoveTowards(_standingToWalkingSlider, 0, Time.deltaTime * 3);
            }

            float _followDistance_Walking = followDistance_Base;
            float _followDistance_Standing = followDistance_Base * 2;

            float _verticalOffset_Walking = verticalOffset_Base;
            float _verticalOffset_Standing = verticalOffset_Base * 3;

            _followDistance_Applied =
                Mathf.Lerp(_followDistance_Standing, _followDistance_Walking, _standingToWalkingSlider);
            _verticalOffset_Applied =
                Mathf.Lerp(_verticalOffset_Standing, _verticalOffset_Walking, _standingToWalkingSlider);

            _fieldOfView_Applied = Mathf.Lerp(fovAtDown, fovAtUp, _standingToWalkingSlider);
    }

    private void _FollowAvatar()
    {
        _camRelativePostion_Auto = _avatarTransform.position;
    
        _cameraLookTarget.position = _avatarTransform.position + avatarObservationOffset_Base;
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _avatarTransform.position - _avatarLookForward * _followDistance_Applied + Vector3.up * _verticalOffset_Applied, Time.deltaTime);
        
        //adjusting fov
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, _fieldOfView_Applied, Time.deltaTime);

    }

    private void _LookAtAvatar()
    {
        _cameraTransform.LookAt(_cameraLookTarget);
        
    }

    private Vector3 _cameraSupposedTransformPos;
    private float _followDistance_Applied_OOI;
    private float _verticalOffset_Applied_OOI;
    
    
    Vector3 TargetOfLAO;
    
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(TargetOfLAO, 1);
    }

    private float EaseInOutCubic(float t, float b, float c, float d)
    {
        t /= d/2;
        if (t < 1) return c / 2 * t * t * t + b;
        t -= 2;
        return c / 2 * (t * t * t + 2) + b;
    }

    private void _LookAtObject(Transform oOI)
    {
        //the look at target moves slowly to where the player is
        Vector3 Center = (oOI.position + _avatarTransform.position)/2;
                
        
        _cameraLookTarget.position = Vector3.Lerp(_cameraLookTarget.position, Center, Time.deltaTime);
        print("TargetOOI = " + TargetOfLAO);
        
        _cameraTransform.LookAt(_cameraLookTarget);        //还要有一个lerp给Lookat



        _OOISlider =  Mathf.Lerp(_OOISlider, 1, Time.deltaTime);

        print("OOISlider = " + _OOISlider);
        
//        _followDistance_Applied_OOI =
//            Mathf.Lerp((_cameraTransform.position - _avatarTransform.position).z, _cameraSupposedTransformPos.z, _OOISlider);
//        _verticalOffset_Applied_OOI =
//            Mathf.Lerp((_cameraTransform.position - _avatarTransform.position).y, _cameraSupposedTransformPos.y, _OOISlider);
        
        //where the camera is supposed to be
        
        //_cameraTransform.position = _avatarTransform.position - 3 * _avatarLookForward * _followDistance_Applied + 4 * Vector3.up * _verticalOffset_Applied;
        _cameraSupposedTransformPos = _avatarTransform.position - 1.2f * _avatarLookForward * _followDistance_Applied + 2f * Vector3.up * _verticalOffset_Applied;

        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _cameraSupposedTransformPos, Time.deltaTime);
        
        //Camera.main.fieldOfView = _fieldOfView_Applied;

        TimerLookAtOOI -= 1;
    }

    private void _LookAwayFromObject()
    {
        _cameraLookTarget.position = Vector3.Lerp(_cameraLookTarget.position, _avatarTransform.position, Time.deltaTime);
        _cameraTransform.LookAt(_cameraLookTarget);        //还要有一个lerp给Lookat

        _cameraSupposedTransformPos = _avatarTransform.position - _avatarLookForward * _followDistance_Applied + Vector3.up * _verticalOffset_Applied;

        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _cameraSupposedTransformPos, Time.deltaTime);
    }

    //控制相机旋转的角度在80度以内
    private void _ManualControl()
    {
        Vector3 _camEulerHold = _cameraTransform.localEulerAngles;

        if (Input.GetAxis("Mouse X") != 0)
            _camEulerHold.y += Input.GetAxis("Mouse X");

        if (Input.GetAxis("Mouse Y") != 0)
        {
            float temp = _camEulerHold.x - Input.GetAxis("Mouse Y");
            //保证角度大于零
            temp = (temp + 360) % 360;

            if (temp < 180)
                temp = Mathf.Clamp(temp, 0, 80);
            else
                temp = Mathf.Clamp(temp, 360 - 80, 360);

            _camEulerHold.x = temp;
        }

        Debug.Log("The V3 to be applied is " + _camEulerHold);
        _cameraTransform.localRotation = Quaternion.Euler(_camEulerHold);
    }
    #endregion

    #region Helper Functions

    private Vector3 _lastPos;
    private Vector3 _currentPos;
    private bool _Helper_IsWalking()
    {
        _lastPos = _currentPos;
        _currentPos = _avatarTransform.position;
        float velInst = Vector3.Distance(_lastPos, _currentPos) / Time.deltaTime;

        if (velInst > .15f)
            return true;
        else return false;
    }
    private bool _Helper_IsThereOOI()
    {
        //Returns an array with all colliders touching or inside the sphere.
        Collider[] stuffInSphere = Physics.OverlapSphere(_avatarTransform.position, 5f);
        bool _oOIPresent = false;

        for (int i = 0; i < stuffInSphere.Length; i++)
        {
            if (stuffInSphere[i].CompareTag("ObjectOfInterest"))
            {
                _oOIPresent = true;
            }
        }

        return _oOIPresent;
    }

    private Transform _Helper_WhatIsClosestOOI()
    {
        //Sphere Overlap
        //Sort for shortest
        //Return the shortest
        return transform;
    }
    
    #endregion
}

public enum CameraStates {Manual, Automatic, Idle}