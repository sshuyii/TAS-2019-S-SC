using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ChunkExample : MonoBehaviour
{
    private MeshFilter _myMF;
    private MeshRenderer _myMR;

    private Mesh _myMesh;

    private Vector3[] _verts;
    private int[] _tris;
    private Vector2[] _uVs;
    private Vector3[] _normals;

    private int sizeSquare;
    private int _totalVertInd;
    private int _totalTrisInd;
    private GameObject _treadmill;
    private CubeTreadmill CubeTreadmill;
    private List<Vector3> _boarderVertsList = new List<Vector3> ();

    private void Awake()
    {
        _myMF = gameObject.AddComponent<MeshFilter>();
        _myMR = gameObject.AddComponent<MeshRenderer>();
        _treadmill = GameObject.Find("Treadmill");
        CubeTreadmill = _treadmill.GetComponent<CubeTreadmill>();
        sizeSquare = CubeTreadmill.sizeSquare;

        _myMesh = new Mesh();
    }

    private void Start()
    {
        _Init();
        _CalcMesh();
        _ApplyMesh();
    }

    private void _Init()
    {
        _totalVertInd = (sizeSquare + 1) * (sizeSquare + 1);
        _totalTrisInd = (sizeSquare) * (sizeSquare) * 2 * 3;//一共有这么多个三角形的顶点
        _verts = new Vector3[_totalVertInd];
        _tris = new int[_totalTrisInd];
        _uVs = new Vector2[_totalVertInd];
        _normals = new Vector3[_totalVertInd];
    }

 
    private void _CalcMesh()
    {
        //生成每个点的高度
        for (int z = 0; z <= sizeSquare; z++)
        {
            for (int x = 0; x <= sizeSquare; x++)
            {
                _verts[(z * (sizeSquare + 1)) + x] = 
                    new Vector3(x, 
                    Perlin.Noise(
                        10 * ((float)x + transform.position.x) / 8,// * 10得到一个起伏更明显的terrain 
                        10 * ((float)z + transform.position.z) / 8), 
                        z);
                
//                //找到所有边缘的点，放进一个list中
//                if (z == sizeSquare || x == sizeSquare || z == 0 || x == 0)
//                {
//                    _boarderVertsList.Add(_verts[(z * (sizeSquare + 1)) + x]);
//                }
            }

           
        }

        //每个面的朝向
        int _triInd = 0;
       

        for (int i = 0; i < sizeSquare; i++)
        {
            for (int j = 0; j < sizeSquare; j++)
            {
                int bottomLeft = j + (i * (sizeSquare + 1)); // true as long as j < sizesquare - 1
                int bottomRight = j + (i * (sizeSquare + 1)) + 1; // true as long as j < sizesquare -1
                int topLeft = j + ((i + 1) * (sizeSquare + 1));
                int topRight = j + ((i + 1) * (sizeSquare + 1)) + 1;

                _tris[_triInd] = bottomLeft;
                _triInd++;
                _tris[_triInd] = topLeft;
                _triInd++;
                _tris[_triInd] = bottomRight;
                _triInd++;
                _tris[_triInd] = topLeft;
                _triInd++;
                _tris[_triInd] = topRight;
                _triInd++;
                _tris[_triInd] = bottomRight;
                _triInd++;
                
            }
        }
        

        //CALCULATING NORMALS
        for (int i = 0; i < _totalTrisInd/3; i++)
        {
            //print("GeneratingMeshNormals Times = " + i);
            int normalTriangleIndex = i;
            int vertexIndexA = _tris[normalTriangleIndex];
            int vertexIndexB = _tris[normalTriangleIndex + 1];
            int vertexIndexC = _tris[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormal(vertexIndexA, vertexIndexB, vertexIndexC);
            _normals[vertexIndexA] += triangleNormal.normalized;
            _normals[vertexIndexB] += triangleNormal.normalized;
            _normals[vertexIndexC] += triangleNormal.normalized;
        }
    }

    private Vector3 SurfaceNormal(int indexA, int indexB, int indexC)
    {
        Vector3 pointA = _verts[indexA];
        Vector3 pointB = _verts[indexB];
        Vector3 pointC = _verts[indexC];

        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;
        
        return Vector3.Cross(sideAB, sideAC).normalized;
    }
    
    
    private void _ApplyMesh()
    {
        _myMesh.vertices = _verts;
        _myMesh.triangles = _tris;
        _myMesh.normals = _normals;
        _myMesh.RecalculateNormals();

        _myMF.mesh = _myMesh;

        _myMR.material = Resources.Load<Material>("MyMat");
    }

}