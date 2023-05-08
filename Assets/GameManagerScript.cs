using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //追加
    public GameObject playerPrefab;
    int[,] map;
    GameObject[,] field;//ゲーム管理用の配列
    Vector2Int playerIndex = new Vector2Int(-1, -1);
    // Start is called before the first frame update
    void Start()
    {
        map = new int[,] {//変更。わかりやすく3x5サイズ
            { 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0 },
        };
        field = new GameObject[map.GetLength(0), map.GetLength(1)];
        string debugText = "";
        //変更。二重for文で二次元配列の情報を出力
        for(int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y,x] == 1)
                {
                    field[y, x] = Instantiate(
                        playerPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                    );
                }
                debugText += map[y, x].ToString() + ",";
            }
            debugText += "\n";//改行
        }
        Debug.Log(debugText);
    }

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for(int x = 0;x < field.GetLength(1);x++)
            {
                if (field[y, x] != null && field[y, x].tag == "Player") 
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        //移動先が範囲外なら移動不可
        if (moveTo.y < 0 || moveTo.y >= map.GetLength(0))
        {
            return false;
        }

        if(moveTo.x < 0 || moveTo.x >= map.GetLength(1))
        {
            return false;
        }

        //移動先に箱が居たら
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box") 
        {
            //どの方向へ移動するかを算出
            Vector2Int velocity = moveTo - moveFrom;

            //プレイヤーの移動先から、さらに先へ箱を移動させる。
            //箱の移動処理。MoveNumberメソッド内でMoveNumberメソッドを呼び、処理が再起している。移動可不可をboolで記録
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);

            //もし移動失敗したら、プレイヤーの移動も失敗
            if (!success)
            {
                return false;
            }
        }

        //プレイヤー・箱関わらずの移動処理
        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x + 1, playerIndex.y));
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x - 1, playerIndex.y));
        }
    }
}
