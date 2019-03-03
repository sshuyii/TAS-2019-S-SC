using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class MeshWriter : MonoBehaviour
{
    
    #region Public References

    public Vector3 TopPoint;
    public Vector3 MidPoint1;
    public Vector3 MidPoint2;
    public Vector3 MidPoint3;
    public Vector3 MidPoint4;
    public Vector3 MidPoint5;

    #endregion
    
    #region Internal References
    private MeshRenderer _myMR;
    private MeshFilter _myMF;
    #endregion

    #region Data
    private Mesh _myMesh;
    public Vector3[] _verts;
    private int[] _tris;
    private Vector3[] _normals;
    private Vector2[] _uVs;
    #endregion



    void Awake()
    {
        _myMR = GetComponent<MeshRenderer>();
        _myMF = GetComponent<MeshFilter>();
    }

    private void Start()
    {
        _ComputeMesh();
        _ApplyMesh();
    }

    private void _ComputeMesh()
    {
        _myMesh = new Mesh();
            
        //0,0,0  1,0,0  0,1,0
        _verts = new Vector3[18];
        _verts[0] = TopPoint;
        _verts[1] = MidPoint1;
        _verts[2] = MidPoint2;
        
        _verts[3] = TopPoint;
        _verts[4] = MidPoint2;
        _verts[5] = MidPoint3;
        
        _verts[6] = TopPoint;
        _verts[7] = MidPoint3;
        _verts[8] = MidPoint4;
        
        _verts[9] = TopPoint;
        _verts[10] = MidPoint4;
        _verts[11] = MidPoint1;
        
        _verts[12] = MidPoint1;
        _verts[13] = MidPoint3;
        _verts[14] = MidPoint4;

        _verts[15] = MidPoint1;
        _verts[16] = MidPoint2;
        _verts[17] = MidPoint3;

        
        _tris = new int[18];
        _tris[0] = 0;
        _tris[1] = 1;
        _tris[2] = 2;
        
        _tris[3] = 3;
        _tris[4] = 4;
        _tris[5] = 5;
        
        _tris[6] = 6;
        _tris[7] = 7;
        _tris[8] = 8;
        
        _tris[9] = 9;
        _tris[10] = 10;
        _tris[11] = 11;
        
        _tris[12] = 12;
        _tris[13] = 13;
        _tris[14] = 14;
       
        _tris[15] = 15;
        _tris[16] = 16;
        _tris[17] = 17;

       

        _normals = new Vector3[18];
        _normals[0] = -Vector3.Cross(TopPoint - MidPoint1, TopPoint - MidPoint2);
        _normals[1] = -Vector3.Cross(TopPoint - MidPoint1, TopPoint - MidPoint2);
        _normals[2] = -Vector3.Cross(TopPoint - MidPoint1, TopPoint - MidPoint2);
        
        _normals[3] = Vector3.Cross(TopPoint - MidPoint2, TopPoint - MidPoint3);
        _normals[4] = Vector3.Cross(TopPoint - MidPoint2, TopPoint - MidPoint3);
        _normals[5] = Vector3.Cross(TopPoint - MidPoint2, TopPoint - MidPoint3);
        
        _normals[6] = Vector3.Cross(TopPoint - MidPoint3, TopPoint - MidPoint4);
        _normals[7] = Vector3.Cross(TopPoint - MidPoint3, TopPoint - MidPoint4);
        _normals[8] = Vector3.Cross(TopPoint - MidPoint3, TopPoint - MidPoint4);
       
        _normals[9] = Vector3.Cross(TopPoint - MidPoint1, TopPoint - MidPoint4);
        _normals[10] = Vector3.Cross(TopPoint - MidPoint1, TopPoint - MidPoint4);
        _normals[11] = Vector3.Cross(TopPoint - MidPoint1, TopPoint - MidPoint4);
        
        _normals[12] = Vector3.Cross(MidPoint1 - MidPoint2, MidPoint3 - MidPoint2);
        _normals[13] = Vector3.Cross(MidPoint1 - MidPoint2, MidPoint3 - MidPoint2);
        _normals[14] = Vector3.Cross(MidPoint1 - MidPoint2, MidPoint3 - MidPoint2);
        
        _normals[15] = Vector3.Cross(MidPoint1 - MidPoint2, MidPoint3 - MidPoint2);
        _normals[16] = Vector3.Cross(MidPoint1 - MidPoint2, MidPoint3 - MidPoint2);
        _normals[17] = Vector3.Cross(MidPoint1 - MidPoint2, MidPoint3 - MidPoint2);
        



        _uVs = new Vector2[18];
        _uVs[0] = new Vector2(0, 0);
        _uVs[1] = new Vector2(1, 0);
        _uVs[2] = new Vector2(0, 1);
        _uVs[3] = new Vector2(0, 1);
        _uVs[4] = new Vector2(1, 1);
        _uVs[5] = new Vector2(1, 0);
        _uVs[6] = new Vector2(1, 0);
        _uVs[7] = new Vector2(1, 0);
        _uVs[8] = new Vector2(1, 0);
        _uVs[9] = new Vector2(1, 0);
        _uVs[10] = new Vector2(1, 0);
        _uVs[11] = new Vector2(1, 0);
        _uVs[12] = new Vector2(1, 0);
        _uVs[13] = new Vector2(1, 0);
        _uVs[14] = new Vector2(1, 0);
        _uVs[15] = new Vector2(1, 0);
        _uVs[16] = new Vector2(1, 0);
        _uVs[17] = new Vector2(1, 0);

        
        

    }

    private void _RandomUVs()
    {
        for (int i = 0; i < _normals.Length; i++)
        {
            _normals[i] = Random.insideUnitSphere;
        }
    }

    private void _ApplyMesh()
    {
        _myMesh.vertices = _verts;
        _myMesh.triangles = _tris;
        _myMesh.normals = _normals;
        _myMesh.uv = _uVs;

        _myMF.mesh = _myMesh;
    }

//    private void Update()
//    {
//        _verts[0] = Vector3.zero;
//        _verts[1] = Vector3.right * (Mathf.Sin(Time.time) + 3);
//        _verts[2] = Vector3.up * (Mathf.Cos(Time.time) + 3);
//
//        //_RandomUVs();
//
//        _ApplyMesh();
//    }
}
