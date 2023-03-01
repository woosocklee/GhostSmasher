using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Junpyo;


namespace Wooseok
{
    

    public abstract class Add_On : MonoBehaviour
    {
        public enum ADD_ON_TYPE
        {
            GENERAL = 0,
            SPECIAL = 1,
            ULTIMATE = 2,

            END = 99
        }



        // Start is called before the first frame update
        public enum ADD_ON_TIMING
        {
            BEFORESKILL = 0, // ��ų ��ü�� ��ü�ϴ� ����µ�
            AFTERSKILL = 1, //  ��ų�� �ܼ� ���常 �ٲٴ� ����µ�
            INTERACTION = 2, // ������ ���ǿ� ���� �ߵ��ϴ� ����µ�

            END = 99
        }

        public ADD_ON_TIMING Timing;
        public ADD_ON_TYPE Type;
        //public int level = 0;
        

        public abstract void Effect(PlayerController_FSM user, PlayerController_FSM target = null);
    }
}