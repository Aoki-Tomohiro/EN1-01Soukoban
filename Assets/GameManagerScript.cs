using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //�z��̐錾
    int[] map;

    void PrintArray()
    {
        //������̐錾�Ə�����
        string debugText = "";
        for (int i = 0; i < map.Length; i++)
        {
            //������Ɍ������Ă���
            debugText += map[i].ToString() + ",";
        }
        //����������������o��
        Debug.Log(debugText);
    }

    int GetPlayerIndex()
    {
        //�v�f����map.Length�Ŏ擾
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == 1)
            {
                return i;
            }
        }
        return -1;
    }

    bool MoveNumber(int number,int moveFrom,int moveTo)
    {
        //�ړ��悪�͈͊O�Ȃ�ړ��s��
        if(moveTo < 0 || moveTo >= map.Length)
        {
            //�����Ȃ��������ɏ����A���^�[������B�������^�[��
            return false;
        }
        //�ړ���ɂQ(��)��������
        if (map[moveTo] == 2)
        {
            //�ǂ̕����ֈړ����邩���Z�o
            int velocity = moveTo - moveFrom;
            //�v���C���[�̈ړ��悩��A����ɐ�ւQ(��)���ړ�������B
            //���̈ړ������BMoveNumber���\�b�h����MoveNumber���\�b�h���ĂсA�������ċN���Ă���B�ړ��s��bool�ŋL�^
            bool success = MoveNumber(2, moveTo, moveTo + velocity);
            //���������ړ����s������A�v���C���[�̈ړ������s
            if(!success)
            {
                return false;
            }
        }
        //�v���C���[�E���ւ�炸�̈ړ�����
        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        map = new int[] { 0, 0, 0, 1, 0, 2, 0, 0, 0 };
        PrintArray();
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            int playerIndex = GetPlayerIndex();

            //�ړ��������֐���
            MoveNumber(1, playerIndex, playerIndex + 1);
            PrintArray();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int playerIndex = GetPlayerIndex();

            //�ړ��������֐���
            MoveNumber(1, playerIndex, playerIndex - 1);
            PrintArray();
        }
    }
}
