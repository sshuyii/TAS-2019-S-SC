using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class FishMovement : MonoBehaviour
{
    private float velMag;
    //private GameObject fish;
    public AutoAgentBehavior AutoAgentBehavior;
    private MeshRenderer _myMR;
    private float OriFrequency;
    

    private void Start()
    {
        //fish = GameObject.Find("Fish");

        //_myMR = fish.GetComponent<MeshRenderer>();
        _myMR = GetComponent<MeshRenderer>();

        velMag = UnityEngine.Random.Range(-5.0f, 5.0f);

        OriFrequency = _myMR.material.GetFloat("_Frequency");
    }

    void Update()
    {
        
        _myMR.material.SetFloat("_TimeOffset", velMag);
        //print("TimeOffset = " + _myMR.material.GetFloat("_TimeOffset") + "velMag =" + velMag);
        //_myMR.material.SetFloat("_Frequency", AutoAgentBehavior.MagnitudeModifier * OriFrequency);
    }
}
