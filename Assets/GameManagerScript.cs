using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //配列の宣言
    int[] map;

    // Start is called before the first frame update
    void Start()
    {
        map = new int[] { 0, 0, 0, 1, 0, 0, 0, 0, 0 };
        //文字列の宣言と初期化
        string debugText = "";
        for(int i = 0; i < map.Length;i++)
        {
            //文字列に結合していく
            debugText += map[i].ToString() + ",";
        }
        //結合した文字列を出力
        Debug.Log(debugText);
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            //見つからなかった時のために-1で初期化
            int playerIndex = -1;
            //要素数はmap.Lengthで取得
            for(int i = 0; i < map.Length; i++)
            {
                if (map[i] == 1)
                {
                    playerIndex = i;
                    break;
                }
            }

            //playerIndex+1のインデックスのものと交換するので、playerIndex-1よりさらに小さいインデックスの時のみ交換処理を行う
            if(playerIndex < map.Length - 1)
            {
                map[playerIndex + 1] = 1;
                map[playerIndex] = 0;
            }

            string debugText = "";
            for (int i = 0; i < map.Length; i++)
            {
                //文字列に結合していく
                debugText += map[i].ToString() + ",";
            }
            //結合した文字列を出力
            Debug.Log(debugText);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //見つからなかった時のために-1で初期化
            int playerIndex = -1;
            //要素数はmap.Lengthで取得
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == 1)
                {
                    playerIndex = i;
                    break;
                }
            }

            //playerIndex+1のインデックスのものと交換するので、playerIndex-1よりさらに小さいインデックスの時のみ交換処理を行う
            if (playerIndex > 0)
            {
                map[playerIndex - 1] = 1;
                map[playerIndex] = 0;
            }

            string debugText = "";
            for (int i = 0; i < map.Length; i++)
            {
                //文字列に結合していく
                debugText += map[i].ToString() + ",";
            }
            //結合した文字列を出力
            Debug.Log(debugText);
        }
    }
}
