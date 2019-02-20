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
    private Transform _cameraPivotTransform;
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
        _cameraPivotTransform = _cameraBaseTransform.Find("CameraPivot");
        _cameraTransform = _cameraPivotTransform.Find("Camera");
        _cameraLookTarget = _cameraBaseTransform.Find("CameraLookTarget");

        _avatarTransform = _view.Find("AIThirdPersonController");
        _avatarRigidbody = _avatarTransform.GetComponent<Rigidbody>();
    }

    private float _idleTimer;
    private bool _MousePressed;
   
    private void Update()
    {
        Camera.main.fieldOfView = fov_Base;
        _cameraBaseTransform.position = _avatarTransform.position;
        _cameraPivotTransform.position = _avatarTransform.position + new Vector3(0, 0, 2);
        //_cameraPivotTransform.rotation = _cameraBaseTransform.rotation;
        
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
        {
            _currentState = CameraStates.Automatic;
        }
        
        
        //what should the camera do under each state
        if (_currentState == CameraStates.Automatic)
        {
            _AutoUpdate();//which contains moving the camera from walking to standing to walking
        }
        else if (_currentState == CameraStates.Manual)
        {
            _ManualUpdate();
        }
        else
        {
            _IdleUpdate();
        }
        print("idleTimer = " + _idleTimer);
        
    }

    #region States

    private int TimerLookAtOOI;
    private Transform WhatIsClosesetOOI;
    
    private void _AutoUpdate()
    {
        _ComputeData();
        print("_Helper_IsThereOOI() = " + _Helper_IsThereOOI());
        if (_Helper_IsThereOOI())
        {
            TimerLookAtOOI = 80;
            WhatIsClosesetOOI = _Helper_WhatIsClosestOOI();

           
        }

        if (TimerLookAtOOI > 40)
        {

            _LookAtObject(WhatIsClosesetOOI);

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
        //_FollowAvatar();
        //follow avatar changes the camera's position with a lerp, here it is better that the change has no delay
        //_cameraTransform.position = _avatarTransform.position - _avatarLookForward * _followDistance_Applied +
        //                            Vector3.up * _verticalOffset_Applied;
        _ManualControl();
        _LookAtAvatar();
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
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, _fieldOfView_Applied, Time.deltaTime * 1.5f);

    }

    private void _LookAtAvatar()
    {
        _cameraTransform.LookAt(_cameraLookTarget);
        
    }

    private Vector3 _cameraSupposedTransformPos;
    private float _followDistance_Applied_OOI;
    private float _verticalOffset_Applied_OOI;
     
    
    Vector3 TargetOfLAO;
   
    
   
    private void _LookAtObject(Transform oOI)
    {
        //the look at target moves slowly to where the player is
        Vector3 Center = (oOI.position + _avatarTransform.position)/2;
                
        
        _cameraLookTarget.position = Vector3.Lerp(_cameraLookTarget.position, Center, Time.deltaTime);
        //print("TargetOOI = " + TargetOfLAO);
        
        _cameraTransform.LookAt(_cameraLookTarget);


        _OOISlider =  Mathf.Lerp(_OOISlider, 1, Time.deltaTime);
        //print("OOISlider = " + _OOISlider);
        
      
        //_cameraTransform.position = _avatarTransform.position - 3 * _avatarLookForward * _followDistance_Applied + 4 * Vector3.up * _verticalOffset_Applied;
        _cameraSupposedTransformPos = _avatarTransform.position - 1.2f * _avatarLookForward * _followDistance_Applied + 3f * Vector3.up * _verticalOffset_Applied;

        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _cameraSupposedTransformPos, Time.deltaTime);
        
        //Camera.main.fieldOfView = _fieldOfView_Applied;

        TimerLookAtOOI -= 1;
        print("TimerLookAtOOI = " + TimerLookAtOOI);
    }

    private void _LookAwayFromObject()
    {
        _cameraLookTarget.position = Vector3.Lerp(_cameraLookTarget.position, _avatarTransform.position, Time.deltaTime);
        _cameraTransform.LookAt(_cameraLookTarget);        //还要有一个lerp给Lookat

        _cameraSupposedTransformPos = _avatarTransform.position - _avatarLookForward * _followDistance_Applied + Vector3.up * _verticalOffset_Applied;

        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _cameraSupposedTransformPos, Time.deltaTime);
    }

    private float rotateSpeed = 20;
    private float x_rot;
    private float y_rot;
    private float x_rot_total;

    public float X_sensitivity = 2f;//镜头跟随鼠标x向移动灵敏度  
    public float Y_sensitivity = 2f;//镜头跟随鼠标y向移动灵敏度  
    public float MinX_rotation = 5F;//镜头绕自身x轴旋转角度，抬头极限，z轴正方向是模型运动的前方  
    public float MaxX_rotation = 60F;//镜头绕自身x轴旋转角度，低头极限  
    private Vector3 originalPosition;
    private Quaternion originalRotation;


    private Vector3 _camPivotEulerHold;
    private Vector3 _camBaseEulerHold;
    private Vector3 _cameraSetPosition;

    //控制相机旋转的角度在80度以内
    private void _ManualControl()
    {
        _camPivotEulerHold = _cameraPivotTransform.localEulerAngles;
        _camBaseEulerHold = _cameraBaseTransform.localEulerAngles;
        _cameraSetPosition = _cameraTransform.position;

        print("_camPivotEulerHold = " + _camPivotEulerHold);


        if (Input.GetAxis("Mouse X") != 0)
        {
            _camBaseEulerHold.y += Input.GetAxis("Mouse X") * X_sensitivity;
            print("_camBaseEulerHold.y = " + _camBaseEulerHold.y);
        }


        print("_Helper_IsCameraIntersecting() = "+ _Helper_IsCameraIntersecting()); 
        
        if (Input.GetAxis("Mouse Y") != 0 && _Helper_IsCameraIntersecting() == false &&
            _cameraTransform.position.y > 0)
        {
            float temp = _camPivotEulerHold.x + Input.GetAxis("Mouse Y")* Y_sensitivity;

            
            
            print("temp = " + temp);

            //保证角度大于零
            temp = (temp + 360) % 360;

            if (temp < 180)
            {
                temp = Mathf.Clamp(temp, 30, 65);
            }
            else
            {
                temp = Mathf.Clamp(temp, 360 - 65, 360 - 30);
            }
            
            _cameraSetPosition.z = _cameraBaseTransform.position.z ;

            _camPivotEulerHold.x = temp;
            print("_camPivotEulerHold.x = " + _camPivotEulerHold.x);

        }
        
        //To decide if the camera is below or above pivot
        if (_cameraTransform.position.y < 2)
        {
            _cameraSetPosition.z = _cameraPivotTransform.position.z - 1.5f;
      
            if(_cameraLookTarget.transform.position.y < 2f)
            {
                _cameraLookTarget.transform.position += new Vector3(0, 0.03f, 0);
            }
            print("camera is below");
        }
        else
        {
            _cameraSetPosition.z = _cameraPivotTransform.position.z - 4f;
            print("camera is above");

        }


        //Debug.Log("The V3 to be applied is " + _camEulerHold);

        _cameraBaseTransform.localEulerAngles = _camBaseEulerHold;
        if(_Helper_IsCameraIntersecting() == false)
        {
            _cameraPivotTransform.localEulerAngles = _camPivotEulerHold;
        }        

        
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _cameraSetPosition, Time.deltaTime);

        //_cameraTransform.position = new Vector3(_cameraTransform.position.x,_cameraTransform.position.y, _cameraSetPosition.z);







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
   
    private bool _Helper_IsCameraIntersecting()
    {
        
        Collider[] stuffInSphere = Physics.OverlapSphere(_cameraTransform.position, 3.5f);
        
        for (int i = 0; i < stuffInSphere.Length; i++)
        {
            if (stuffInSphere[i].CompareTag("Player") || stuffInSphere[i].CompareTag("Ground"))
            {    
                print("raycasting");

                return true;
            }
        }

        return false;

//        RaycastHit hit;
//        Debug.DrawRay(_cameraTransform.position, _cameraTransform.TransformDirection(Vector3.forward) * 1.4f, Color.yellow);
//        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.TransformDirection(Vector3.forward), 1.4f))
//        {    
//                //_cameraTransform = _cameraCurrentTransform;
//                print("raycasting");
//            return true;
//        }
//
//        return false;
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
        Transform tMin = null;
        float minDist = Mathf.Infinity;


        List <Transform> OOIList = new List<Transform>();
        Collider[] stuffInSphere = Physics.OverlapSphere(_avatarTransform.position, 5f);
        for (int i = 0; i < stuffInSphere.Length; i++)
        {
            if (stuffInSphere[i].CompareTag("ObjectOfInterest"))
            {
                OOIList.Add(stuffInSphere[i].transform);
            }
        }

        for (int i = 0; i < OOIList.Count; i++)
        {
            float dist = Vector3.Distance(OOIList[i].position, _avatarTransform.position);
            if (dist < minDist)
            {
                tMin = OOIList[i];

                minDist = dist;
            }
        }
        
        return tMin;

  
    }
    
    #endregion
}

public enum CameraStates {Manual, Automatic, Idle}