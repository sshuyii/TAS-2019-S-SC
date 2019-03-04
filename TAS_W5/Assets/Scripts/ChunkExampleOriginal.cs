using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ChunkExampleOriginal : MonoBehaviour
{
    private MeshFilter _myMF;
    private MeshRenderer _myMR;

    private Mesh _myMesh;

    private Vector3[] _verts;
    private int[] _tris;
    private Vector2[] _uVs;
    private Vector3[] _normals;
    
    private Vector3[] _vertsApplied;
    private int[] _trisApplied;
    private Vector2[] _uVsApplied;
    private Vector3[] _normalsApplied;

    private int sizeSquare;
    private int _totalVertInd;
    private int _totalTrisInd;
    private int _totalVertIndApplied;
    private int _totalTrisIndApplied;
    
    private CubeTreadmill CubeTreadmill;
    private List<Vector3> _boarderVertsList = new List<Vector3> ();

    

    private void Awake()
    {
        _myMF = gameObject.AddComponent<MeshFilter>();
        _myMR = gameObject.AddComponent<MeshRenderer>();
        CubeTreadmill = GetComponent<CubeTreadmill>();
        sizeSquare = CubeTreadmill.sizeSquare + 2;

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
        
        _totalVertIndApplied = (sizeSquare) * (sizeSquare);
        _totalTrisIndApplied = (sizeSquare - 1) * (sizeSquare - 1) * 2 * 3;//一共有这么多个三角形的顶点
        
        _verts = new Vector3[_totalVertInd];
        _vertsApplied = new Vector3[_totalVertIndApplied];
        
        _tris = new int[_totalTrisInd];
        _trisApplied = new int[_totalTrisIndApplied];
        
        _uVs = new Vector2[_totalVertInd];

        _normals = new Vector3[_totalVertInd];
        _normalsApplied = new Vector3[_totalVertIndApplied];

        
    }


    private void _CalcMesh()
    {
        //所有点放在坐标系上
        int[,] vertexIndiceMap = new int[sizeSquare + 1, sizeSquare + 1];
      
        
        int meshVertexIndex = 1;
        int boarderVertexIndex = -1;
        int Times = 0;
        //生成每个点的高度
        for (int z = 0; z <= sizeSquare; z++)
        {
            for (int x = 0; x <= sizeSquare; x++)
            {
                _verts[(z * (sizeSquare + 1)) + x] =
                    new Vector3(x,
                        Perlin.Noise(
                            50 * ((float) x + transform.position.x) / 8, // * 10得到一个起伏更明显的terrain 
                            50 * ((float) z + transform.position.z) / 8),
                        z);

                //找到所有边缘的点，放进一个list中
                if (z == sizeSquare|| x == sizeSquare|| z == 0 || x == 0)
                {
                    vertexIndiceMap[x, z] = boarderVertexIndex;
                    boarderVertexIndex--;
                }
                else
                {
                    vertexIndiceMap[x, z] = meshVertexIndex;
                    meshVertexIndex++;
                }
                
                //找到需要生成的中心的点   
                if (vertexIndiceMap[x, z] > 0)
                {
//                    print("x = " + x + " z = " + z);
//                    print("(z - 1) * (sizeSquare - 1) + x - 1 = " + ((z - 1) * (sizeSquare - 1) + x - 1));
//                    print("(z * (sizeSquare + 1)) + x = " + ((z * (sizeSquare + 1)) + x));

                    _vertsApplied[(z - 1) * (sizeSquare - 1) + x - 1] = _verts[(z * (sizeSquare + 1)) + x];

                }
            }
        }

       

        //每个面的朝向
        int _triInd = 0;
        int _triIndApplied = 0;

        for (int i = 0; i < sizeSquare - 1; i++)
        {
            for (int j = 0; j < sizeSquare - 1; j++)
            {
                //找到需要生成的中心的tri
                if (vertexIndiceMap[j, i] > 0)
                {
                    //print("j = " + j + " i = " + i);

                    int a = j - 1;
                    int b = i - 1;
                    int bottomLeft = a + (b * (sizeSquare - 1)); // true as long as j < sizesquare - 1
                    int bottomRight = a + (b * (sizeSquare - 1)) + 1; // true as long as j < sizesquare -1
                    int topLeft = a + ((b + 1) * (sizeSquare - 1));
                    int topRight = a + ((b + 1) * (sizeSquare - 1)) + 1;

                    _trisApplied[_triIndApplied] = bottomLeft;
                    //print("bottomLeft = " + bottomLeft);
                    _triIndApplied++;
                    _trisApplied[_triIndApplied] = topLeft;
                    _triIndApplied++;
                    _trisApplied[_triIndApplied] = bottomRight;
                    _triIndApplied++;
                    _trisApplied[_triIndApplied] = topLeft;
                    _triIndApplied++;
                    _trisApplied[_triIndApplied] = topRight;
                    _triIndApplied++;
                    _trisApplied[_triIndApplied] = bottomRight;
                    _triIndApplied++;

                }
            }

        }
        
        //计算一边正常情况下的各个tri的顶点的坐标
        for (int i = 0; i < sizeSquare; i++)
        {
            for (int j = 0; j < sizeSquare; j++)
            {
                int bottomLeftW = j + (i * (sizeSquare + 1)); // true as long as j < sizesquare - 1
                int bottomRightW = j + (i * (sizeSquare + 1)) + 1; // true as long as j < sizesquare -1
                int topLeftW = j + ((i + 1) * (sizeSquare + 1));
                int topRightW = j + ((i + 1) * (sizeSquare + 1)) + 1;

                _tris[_triInd] = bottomLeftW;
                _triInd++;
                _tris[_triInd] = topLeftW;
                _triInd++;
                _tris[_triInd] = bottomRightW;
                _triInd++;
                _tris[_triInd] = topLeftW;
                _triInd++;
                _tris[_triInd] = topRightW;
                _triInd++;
                _tris[_triInd] = bottomRightW;
                _triInd++;
            }

        }
        



        for (int i = 0; i < _totalTrisInd / 3; i++)//有多少个三角形loop多少次
        {
            //normal每个点只有一个，但这里先求出一共多少三角形的顶点，每个点加上所有面的法向量，最后求平均
            int normalTriangleIndex = i;//
            int vertexIndexA = _tris[normalTriangleIndex * 3];
            int vertexIndexB = _tris[normalTriangleIndex * 3 + 1];
            int vertexIndexC = _tris[normalTriangleIndex * 3 + 2];
  
            

            //为了取加权平均数
            Vector3 triangleNormal1 = SurfaceNormal(vertexIndexA, vertexIndexB, vertexIndexC);
            _normals[vertexIndexA] += triangleNormal1;
            _normals[vertexIndexB] += triangleNormal1;
            _normals[vertexIndexC] += triangleNormal1;
           

        }

        for (int i = 0; i < _totalVertInd; i++)
        {
            _normals[i] = _normals[i].normalized;

        }
      
        //找到中间那些点的normals
        for (int i = 0; i <= sizeSquare; i++)
        {
            for (int j = 0; j <= sizeSquare; j++)
            {
                //找到需要生成的中心的点
                if (vertexIndiceMap[j, i] > 0)
                {
//                    print("j = " + j + " i = " + i);
//                    print("(z - 1) * (sizeSquare - 1) + x - 1 = " + ((i - 1) * (sizeSquare - 1) + j - 1));
//                    print("(z * (sizeSquare + 1)) + x = " + ((i * (sizeSquare + 1)) + j));

                    _normalsApplied[(i - 1) * (sizeSquare - 1) + j - 1] = _normals[(i * (sizeSquare + 1)) + j];
                }
            }
        }
        
        for (int i = 0; i < _totalVertInd; i++)
        {
            print("_normal000 = " + _normals[i]);
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