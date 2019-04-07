using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject myAutoAgentPrefab;
    [Range(1, 500)] public int numberOfSpawns;

    List<GameObject> _allMyAgents = new List<GameObject>();
    List<Collider> _allMyColliders = new List<Collider>();


    private Collider[] collAll;
    
    // Start is called before the first frame update
    void Start()
    {
        //to create fishes
        float rCubed = 3 * numberOfSpawns / (4 * Mathf.PI * .01f);//.01 per unit volume
        float r = Mathf.Pow(rCubed, .3333333f);
        
        for (int i = 0; i < numberOfSpawns; i++)
        {
            _allMyAgents.Add(Instantiate(myAutoAgentPrefab, Random.insideUnitSphere * r, Quaternion.identity, transform));
        }
        
        collAll = new Collider[numberOfSpawns];

    }

    
    //下面这行代码好像有问题，只会记录离他最近的一个
    //Collider[] collInRad = new Collider[1];
    
    // Update is called once per frame
    void Update()
    {
        
      
        
        //get colliders near one fish
        foreach (GameObject g in _allMyAgents)
        {
            
            AutoAgentBehavior a = g.GetComponent<AutoAgentBehavior>();
            _allMyColliders.Add(g.GetComponent<Collider>());


            //Physics.OverlapSphereNonAlloc(g.transform.position, 5, collInRad);
            Collider[] collInRad = Physics.OverlapSphere(g.transform.position, 3);
            
            
            //pass the colliders near a fish into script AutoAgentBehavior
            a.PassArrayOfContext(collInRad);
            a.PassArrayOfAllAgents(collAll);

        }
        
        for (int i = 0; i < numberOfSpawns; i++)
        {
            collAll[i] = _allMyColliders[i];
        }
        
        
    }
}
