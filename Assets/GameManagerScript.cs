using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //�z��̐錾
    int[] map;

    // Start is called before the first frame update
    void Start()
    {
        map = new int[] { 0, 0, 0, 1, 0, 0, 0, 0, 0 };
        //������̐錾�Ə�����
        string debugText = "";
        for(int i = 0; i < map.Length;i++)
        {
            //������Ɍ������Ă���
            debugText += map[i].ToString() + ",";
        }
        //����������������o��
        Debug.Log(debugText);
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            //������Ȃ��������̂��߂�-1�ŏ�����
            int playerIndex = -1;
            //�v�f����map.Length�Ŏ擾
            for(int i = 0; i < map.Length; i++)
            {
                if (map[i] == 1)
                {
                    playerIndex = i;
                    break;
                }
            }

            //playerIndex+1�̃C���f�b�N�X�̂��̂ƌ�������̂ŁAplayerIndex-1��肳��ɏ������C���f�b�N�X�̎��̂݌����������s��
            if(playerIndex < map.Length - 1)
            {
                map[playerIndex + 1] = 1;
                map[playerIndex] = 0;
            }

            string debugText = "";
            for (int i = 0; i < map.Length; i++)
            {
                //������Ɍ������Ă���
                debugText += map[i].ToString() + ",";
            }
            //����������������o��
            Debug.Log(debugText);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //������Ȃ��������̂��߂�-1�ŏ�����
            int playerIndex = -1;
            //�v�f����map.Length�Ŏ擾
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == 1)
                {
                    playerIndex = i;
                    break;
                }
            }

            //playerIndex+1�̃C���f�b�N�X�̂��̂ƌ�������̂ŁAplayerIndex-1��肳��ɏ������C���f�b�N�X�̎��̂݌����������s��
            if (playerIndex > 0)
            {
                map[playerIndex - 1] = 1;
                map[playerIndex] = 0;
            }

            string debugText = "";
            for (int i = 0; i < map.Length; i++)
            {
                //������Ɍ������Ă���
                debugText += map[i].ToString() + ",";
            }
            //����������������o��
            Debug.Log(debugText);
        }
    }
}
