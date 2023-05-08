using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //�ǉ�
    public GameObject playerPrefab;
    int[,] map;
    GameObject[,] field;//�Q�[���Ǘ��p�̔z��
    Vector2Int playerIndex = new Vector2Int(-1, -1);
    // Start is called before the first frame update
    void Start()
    {
        map = new int[,] {//�ύX�B�킩��₷��3x5�T�C�Y
            { 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0 },
        };
        field = new GameObject[map.GetLength(0), map.GetLength(1)];
        string debugText = "";
        //�ύX�B��dfor���œ񎟌��z��̏����o��
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
            debugText += "\n";//���s
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
        //�ړ��悪�͈͊O�Ȃ�ړ��s��
        if (moveTo.y < 0 || moveTo.y >= map.GetLength(0))
        {
            return false;
        }

        if(moveTo.x < 0 || moveTo.x >= map.GetLength(1))
        {
            return false;
        }

        //�ړ���ɔ���������
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box") 
        {
            //�ǂ̕����ֈړ����邩���Z�o
            Vector2Int velocity = moveTo - moveFrom;

            //�v���C���[�̈ړ��悩��A����ɐ�֔����ړ�������B
            //���̈ړ������BMoveNumber���\�b�h����MoveNumber���\�b�h���ĂсA�������ċN���Ă���B�ړ��s��bool�ŋL�^
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);

            //�����ړ����s������A�v���C���[�̈ړ������s
            if (!success)
            {
                return false;
            }
        }

        //�v���C���[�E���ւ�炸�̈ړ�����
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
