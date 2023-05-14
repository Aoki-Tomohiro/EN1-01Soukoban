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
    //�z��̐錾
    int[,] map; //���x���f�U�C���p�̔z��
    GameObject[,] field; //�Q�[���Ǘ��p�̔z��

    //void PrintArray()
    //{
    //    //������̐錾�Ə�����
    //    string debugText = "";
    //    for (int i = 0; i < map.Length; i++)
    //    {
    //        //������Ɍ������Ă���
    //        debugText += map[i].ToString() + ",";
    //    }
    //    //����������������o��
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
        //�ړ��悪�͈͊O�Ȃ�ړ��s��
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0))
        {
            //�����Ȃ��������ɏ����A���^�[������B�������^�[��
            return false;
        }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1))
        {
            //�����Ȃ��������ɏ����A���^�[������B�������^�[��
            return false;
        }

        //Box�^�O�������Ă�����ċN����
        if (field[moveTo.y,moveTo.x] != null && field[moveTo.y,moveTo.x].tag == "Box")
        {
            //�ǂ̕����ֈړ����邩���Z�o
            Vector2Int velocity = moveTo - moveFrom;
            //�v���C���[�̈ړ��悩��A����ɐ�ւQ(��)���ړ�������B
            //���̈ړ������BMoveNumber���\�b�h����MoveNumber���\�b�h���ĂсA�������ċN���Ă���B�ړ��s��bool�ŋL�^
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
            //���������ړ����s������A�v���C���[�̈ړ������s
            if (!success)
            {
                return false;
            }
        }
        //GameObject�̍��W(position)���ړ������Ă���C���f�b�N�X�̓���ւ�
        field[moveFrom.y, moveFrom.x].transform.position =
            new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    bool IsCleard()
    {
        //Vector2Int�^�̉ϒ��z��̍쐬
        List<Vector2Int> goals = new List<Vector2Int>();

        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                //�i�[�ꏊ���ۂ��𔻒f
                if (map[y,x] == 3)
                {
                    //�i�[�ꏊ�̃C���f�b�N�X���T���Ă���
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        for(int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if(f == null || f.tag != "Box")
            {
                //��ł������Ȃ�������������B��
                return false;
            }
        }

        //�������B���łȂ���Ώ����B��
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
            debugText += "\n";//���s
        }
        Debug.Log(debugText);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            //�ړ��������֐���
            MoveNumber("Player", new Vector2Int(playerIndex.x, playerIndex.y), new Vector2Int(playerIndex.x + 1, playerIndex.y));

            //�����N���A���Ă�����
            if (IsCleard())
            {
                //�Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            //�ړ��������֐���
            MoveNumber("Player", new Vector2Int(playerIndex.x, playerIndex.y), new Vector2Int(playerIndex.x - 1, playerIndex.y));

            //�����N���A���Ă�����
            if (IsCleard())
            {
                //�Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            //�ړ��������֐���
            MoveNumber("Player", new Vector2Int(playerIndex.x, playerIndex.y), new Vector2Int(playerIndex.x, playerIndex.y - 1));

            //�����N���A���Ă�����
            if (IsCleard())
            {
                //�Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            //�ړ��������֐���
            MoveNumber("Player", new Vector2Int(playerIndex.x, playerIndex.y), new Vector2Int(playerIndex.x, playerIndex.y + 1));

            //�����N���A���Ă�����
            if (IsCleard())
            {
                //�Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
                clearText.SetActive(true);
            }
        }
    }
}
