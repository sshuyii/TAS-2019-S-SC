using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeTreadmill : MonoBehaviour
{
    public GameObject cube;
    
    public GameObject target;
    public GameObject ChunkExample;
    public int sizeSquare;//size square不在chunkExample中设定，在此脚本中设定，chunkExample引用这个脚本中的这个变量
    
    private List<GameObject> _cubes;
    private List<GameObject> _chunks;
    private List<Vector2> _chunkGrid = new List<Vector2>();
    private List<Vector2> _chunkGenerate = new List<Vector2>();
    private List<Vector2> _chunkDestroy = new List<Vector2>();
    private List<Vector2> _chunkStillList;

    
    private Dictionary<Vector2, GameObject> _chunkExample = new Dictionary<Vector2, GameObject>();

  
    private Vector3 _intPos;
    private Vector3 _currentIntPos;
    private Vector3 _oldIntPos;
   

    private Vector2 _oldCameraPos;
    private Vector2 _currentCameraPos;
    private int _cameraViewDist;//用来记录camera周围能看到的位置，以格点来计算
    private List<Vector2> _newSpotList;

    

    
    void Start()
    {
       
        //产生36个chunkExample
        _chunks = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            for (int j = -2; j < 2; j++)
            {
                _chunks.Add(Instantiate(ChunkExample, new Vector3(target.transform.position.x + j * 16, 0, target.transform.position.z + i * 16), Quaternion.identity));
            }
        }
        
        
    }


    private List<Vector2> _cameraViewList;
    private List<Vector2> _chunksGridList;
    private List<Vector2> _chunksStill;
    private List<Vector2> _chunksDestroyList;
    GameObject _chunkToDestroy;


    
    
    void Update()
    {

        _currentCameraPos = UpdateCameraPos();
        if (_oldCameraPos != _currentCameraPos)
        {
            _cameraViewList = GetCameraView(_currentCameraPos);
            _chunksGridList = ConvertChunkToGrid();
            
            DestroyGenerateCal(_cameraViewList, _chunksGridList);
//            
            for (int i = 0; i < _chunkGenerate.Count; i++)
            {
                print("chunkGenerate = " + _chunkGenerate[i]);

            }
        
            for (int i = 0; i < _chunkDestroy.Count; i++)
            {
                print("chunkDestroy = " + _chunkDestroy[i]);

            }


            //销毁chunks
            foreach (Vector2 v in _chunkDestroy)
            {
                _chunkExample.TryGetValue(v, out _chunkToDestroy);
            
                Destroy(_chunkToDestroy);
                _chunks.Remove(_chunkToDestroy);
                print("destroy");

            }


            //生成chunks
            for (int i = 0; i < _chunkGenerate.Count; i++)

            {
                //生成
                _chunks.Add(Instantiate(ChunkExample, new Vector3(_chunkGenerate[i].x * 16, 0, _chunkGenerate[i].y * 16), Quaternion.identity));
            }
            _chunkGenerate.Clear();
        }

        _oldCameraPos = UpdateCameraPos();

    }




    private Vector2 UpdateCameraPos()
    {
        //得到camera所处的chunkExample坐标的位置
        int _currentCamCoordX = (int)(target.transform.position.x / 16); //currently size Square = 8
        int _currentCamCoordZ = (int)(target.transform.position.z / 16);

        print("currentCamCoord =" + _currentCamCoordX + "," + _currentCamCoordZ);

        Vector2 _currentCameraPos = new Vector2(_currentCamCoordX, _currentCamCoordZ);
        return _currentCameraPos;
    }


    private List<Vector2> ConvertChunkToGrid()
    {
        //把所有chunks的坐标都统一成格点上的单位坐标
        //并记录在了List: _chunkGrid中
        _chunkGrid = new List<Vector2>();

        foreach (GameObject g in _chunks)
        {
            int _gridX = (int)(g.transform.position.x / 16);
            int _gridY = (int)(g.transform.position.z / 16);

            Vector2 _grid = new Vector2(_gridX, _gridY);

            _chunkGrid.Add(_grid);
            print("chunksGridddddddddddddddd =" + new Vector2(_gridX, _gridY)); 
           

            
            GameObject temp;
            
            if (!_chunkExample.TryGetValue(_grid, out temp))
            {
                _chunkExample.Add(_grid, g);//用Dictionary 记录两个信息，代替上面一行
                //print("chunksGrid =" + new Vector2(_gridX, _gridY)); 
            }
        } 
        
      
        return _chunkGrid;
    }

    private List<Vector2> GetCameraView(Vector2 CamPos)
    {
        List<Vector2> _camView = new List<Vector2>();

        //找到应该生成chunks的格点们,在camera周围
        for (int i = -2; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                
                _camView.Add(new Vector2(CamPos.x + i, CamPos.y + j));
                
            }
        }
        
        return _camView;
    }

    private void DestroyGenerateCal(List<Vector2> camView, List<Vector2> chunkGrid)
    {

        for (int i = 0; i < camView.Count; i++)
        {
            _chunkGenerate.Add(camView[i]);
        }
        for (int i = 0; i < chunkGrid.Count; i++)
        {
            _chunkDestroy.Add(chunkGrid[i]);
        }

        
        List<Vector2> temp = new List<Vector2>();
        
        print("camViewCounttttttttttt = " + camView.Count);
        print("chunkGridCounttttttttttttt = " + chunkGrid.Count);

        //找到所有在camView中存在的chunkGrid的项，从_chunkGrid中删除,是要destroy的项
        //找到所有在camView中存在的chunkGrid的项，从_chunkView中删除，是要generate的项

        for (int i = 0; i < camView.Count; i++)
        {

            for (int j = 0; j < chunkGrid.Count; j++)
            {
                print("i ============" + i);
                print("cameView[i] + chunkGrid[j] = " + camView[i] + chunkGrid[j]);
                if (camView[i] == chunkGrid[j])
                {
                    print("jjjjjjjjjjjjj = " + chunkGrid[j]);
                    temp.Add(chunkGrid[j]);
                    _chunkDestroy.Remove(chunkGrid[j]);
                    _chunkGenerate.Remove(chunkGrid[j]);
                    break;
                }
            }
        }

    }

}