using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class AutoAgentBehavior : MonoBehaviour
{

    public Vector3 moveDirection;

    public float moveVelocityMagnitude;
    public Transform myModelTransform;
    public float MagnitudeModifier;
    private float OriMagnitude;

    private Collider[] collAll;

        
    [Range(0.0f, 1.0f)] public float clumpVar;
    [Range(0.0f, 1.0f)] public float alignVar;
    [Range(0.0f, 1.0f)] public float sepVar;
    [Range(0.0f, 1.0f)] public float oriVar;
    
    void Start()
    {
        //moveDirection = Vector3.Normalize(Random.insideUnitSphere);
        myModelTransform = transform.GetChild(0);
        OriMagnitude = moveVelocityMagnitude;

    }
    
    void Update()
    {
//        Vector3 toOther = FlockOrigin(collAll) - transform.position;
//        
//        if (Vector3.Dot(transform.forward, toOther) > 0f)
//        {
//            MagnitudeModifier = Vector3.Magnitude(toOther)/10;
//        }
//        else
//        {
//            MagnitudeModifier = 1;
//        }
        //moveVelocityMagnitude =  OriMagnitude * MagnitudeModifier;

        //print("Modifier = " + MagnitudeModifier);
    }


    //to get the array of every fish's collider
    public void PassArrayOfAllAgents(Collider[] contextAll)
    {
        collAll = contextAll;
    }
    
    //to get all fishes moving in the direction and with the speed assigned
    public void PassArrayOfContext(Collider[] context)
    {
        //use the context, which is an array of the fishes created from the FlockManager
        //called from the FlockManager
        List<Collider> contextWithoutMe = new List<Collider>();

        foreach (Collider c in context)
        {
            if (c.gameObject != gameObject)
                contextWithoutMe.Add(c);
        }

        CalcMyDir(contextWithoutMe.ToArray());

        MoveInMyAssignedDirection(moveDirection, moveVelocityMagnitude);
    }
    
    
    void MoveInMyAssignedDirection(Vector3 direction, float magnitude)
    {
        transform.position += direction * magnitude * Time.deltaTime;
        myModelTransform.rotation = Quaternion.LookRotation(direction);
        
    }

    
    public void CalcMyDir(Collider[] context)
    {
                
        moveDirection = Vector3.Lerp(moveDirection, 
            Vector3.Normalize(ClumpDir(context) * clumpVar 
                              + Align(context) * alignVar 
                              + Separation(context) * sepVar * context.Length/5
                              + AttractedToOrigin() * oriVar * Vector3.Magnitude(transform.position)/50), 
                            .03f);
    }

    float desiredseparation = 1;

    
    //separation
    Vector3 Separation(Collider[] context)
    {
        Vector3 distance = Vector3.zero;
        ;
        foreach (Collider c in context)
        { 
            //distance from another fish
            distance += transform.position - c.transform.position;
            
        }
       
        //distance /= context.Length;   

        Vector3 normalizedAvoidance = Vector3.Normalize(distance);

        return normalizedAvoidance;
    }

    //the flock direction that one fish wants to go to?
    //aka cohesion
    Vector3 ClumpDir(Collider[] context)
    {
        
        //get the center point of the flock
        Vector3 midpoint = Vector3.zero;
        
        foreach (Collider c in context)
        {
            midpoint += c.transform.position;
        }

        midpoint /= context.Length;
        
        //get the direction that one fish should go to
        Vector3 dirIWantToGo = midpoint - transform.position;
        //normalize it
        Vector3 normalizedDirIWantToGo = Vector3.Normalize(dirIWantToGo);

        
        return normalizedDirIWantToGo;
    }

    //get the average direction that each fish in the school is moving toward
    //aka alignment
    Vector3 Align(Collider[] context)
    {
        Vector3 headings = Vector3.zero;

        foreach (Collider c in context)
        {
            headings += c.transform.GetChild(0).forward;
        }

        headings /= context.Length;
        return Vector3.Normalize(headings);
    }

    //in case some fish get too far away and will never be back again
    Vector3 AttractedToOrigin()
    {
        Vector3 _origin = Vector3.zero;
     
        _origin -= transform.position;

        return _origin;
    }

    Vector3 FlockOrigin(Collider[] contextAll)
    {
        Vector3 _originAll = Vector3.zero;
        
        foreach (Collider c in contextAll)
        {
            _originAll += c.transform.position;
        }

        _originAll /= contextAll.Length;

        return _originAll;
    }
   
}
