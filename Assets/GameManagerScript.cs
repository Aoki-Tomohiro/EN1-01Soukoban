using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject clearText;
    public GameObject goalPrefab;
    //配列の宣言
    int[,] map; //レベルデザイン用の配列
    GameObject[,] field; //ゲーム管理用の配列

    //void PrintArray()
    //{
    //    //文字列の宣言と初期化
    //    string debugText = "";
    //    for (int i = 0; i < map.Length; i++)
    //    {
    //        //文字列に結合していく
    //        debugText += map[i].ToString() + ",";
    //    }
    //    //結合した文字列を出力
    //    Debug.Log(debugText);
    //}

    Vector2Int GetPlayerIndex()
    {
       for(int y = 0; y < field.GetLength(0); y++)
       {
            for(int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y,x] == null)
                {
                    continue;
                }
                if (field[y,x].tag == "Player")
                {
                    return new Vector2Int(x,y);
                }
            }
       }
        return new Vector2Int(-1, -1);
    }

    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        //移動先が範囲外なら移動不可
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0))
        {
            //動けない条件を先に書き、リターンする。早期リターン
            return false;
        }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1))
        {
            //動けない条件を先に書き、リターンする。早期リターン
            return false;
        }

        //Boxタグを持っていたら再起処理
        if (field[moveTo.y,moveTo.x] != null && field[moveTo.y,moveTo.x].tag == "Box")
        {
            //どの方向へ移動するかを算出
            Vector2Int velocity = moveTo - moveFrom;
            //プレイヤーの移動先から、さらに先へ２(箱)を移動させる。
            //箱の移動処理。MoveNumberメソッド内でMoveNumberメソッドを呼び、処理を再起している。移動可不可をboolで記録
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
            //もし箱が移動失敗したら、プレイヤーの移動も失敗
            if (!success)
            {
                return false;
            }
        }
        //GameObjectの座標(position)を移動させてからインデックスの入れ替え
        field[moveFrom.y, moveFrom.x].transform.position =
            new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    bool IsCleard()
    {
        //Vector2Int型の可変長配列の作成
        List<Vector2Int> goals = new List<Vector2Int>();

        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                //格納場所か否かを判断
                if (map[y,x] == 3)
                {
                    //格納場所のインデックスを控えておく
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        for(int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if(f == null || f.tag != "Box")
            {
                //一つでも箱がなかったら条件未達成
                return false;
            }
        }

        //条件未達成でなければ条件達成
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        map = new int[,]
        {
            { 0, 0, 0, 0, 0 },
            { 0, 3, 1, 3, 0 },
            { 0, 0, 2, 0, 0 },
            { 0, 2, 3, 2, 0 },
            { 0, 0, 0, 0, 0 },
        };

        field = new GameObject
            [
            map.GetLength(0),
            map.GetLength(1)
            ];

        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y,x] = Instantiate(
                        playerPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity);
                }

                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity);
                }

                if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(
                        goalPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0.01f),
                        Quaternion.identity);
                }
            }
        }

        string debugText = "";
        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y, x].ToString() + ",";
            }
            debugText += "\n";//改行
        }
        Debug.Log(debugText);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            //移動処理を関数化
            MoveNumber("Player", new Vector2Int(playerIndex.x, playerIndex.y), new Vector2Int(playerIndex.x + 1, playerIndex.y));

            //もしクリアしていたら
            if (IsCleard())
            {
                //ゲームオブジェクトのSetActiveメソッドを使い有効化
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            //移動処理を関数化
            MoveNumber("Player", new Vector2Int(playerIndex.x, playerIndex.y), new Vector2Int(playerIndex.x - 1, playerIndex.y));

            //もしクリアしていたら
            if (IsCleard())
            {
                //ゲームオブジェクトのSetActiveメソッドを使い有効化
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            //移動処理を関数化
            MoveNumber("Player", new Vector2Int(playerIndex.x, playerIndex.y), new Vector2Int(playerIndex.x, playerIndex.y - 1));

            //もしクリアしていたら
            if (IsCleard())
            {
                //ゲームオブジェクトのSetActiveメソッドを使い有効化
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            //移動処理を関数化
            MoveNumber("Player", new Vector2Int(playerIndex.x, playerIndex.y), new Vector2Int(playerIndex.x, playerIndex.y + 1));

            //もしクリアしていたら
            if (IsCleard())
            {
                //ゲームオブジェクトのSetActiveメソッドを使い有効化
                clearText.SetActive(true);
            }
        }
    }
}
