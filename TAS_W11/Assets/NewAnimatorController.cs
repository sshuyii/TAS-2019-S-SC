using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAnimatorController : MonoBehaviour
{
    
    private float walkRun_TreeVal_X;
    private float walkRun_TreeVal_Y;

  
    private Animator _myAnimator;
    
    private float time;
    private float idleTime;
    
    [Header("Tuning Values")]
    [Range(0.001f, 10.0f)] public float walkCycleTime;
    [Range(0.00f, 1.00f)] public float walkRunMagnitude;

    [Range(0.00f, 1.00f)] public float walkRunBlendTotal;

    
    // Start is called before the first frame update
    void Start()
    {
        _myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        walkCycleTime = 1 - (.2f * walkRunBlendTotal);
        walkRunMagnitude = .25f + (.75f * walkRunBlendTotal);

        time += (Mathf.PI * 2 * Time.deltaTime) / walkCycleTime;

        walkRun_TreeVal_X = Mathf.Cos(time) * walkRunMagnitude;
        walkRun_TreeVal_Y = Mathf.Sin(time) * walkRunMagnitude;

        _myAnimator.SetFloat("WalkRun_TreeVal_X", walkRun_TreeVal_X);
        _myAnimator.SetFloat("WalkRun_TreeVal_Y", walkRun_TreeVal_Y);
    }
    
}
