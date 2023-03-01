using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class State_Machine
    {
        Transform Owner;
        Dictionary<PLAYERSTATE, State_Base> StateMap = new Dictionary<PLAYERSTATE, State_Base>();
        PLAYERSTATE CurState;
        PLAYERSTATE PrevState;

        public Transform _Owner
        {
            get { return Owner; }
            set { Owner = value; }
        }

        public PLAYERSTATE _CurState
        {
            get { return CurState; }
            set { CurState = value; }
        }

        public PLAYERSTATE _PrevState
        {
            get { return PrevState; }
            set { PrevState = value; }
        }


        public void Update()
        {
            StateMap[CurState].Update();
        }

        public void FixedUpdate()
        {
            StateMap[CurState].FixedUpdate();
        }

        public void StateAdd(State_Base state,PLAYERSTATE type)
        {
            state.Init(Owner, type,this);
            StateMap.Add(type, state);
        }

        public void ChangeState(PLAYERSTATE newState)
        {
            if (!StateMap.ContainsKey(newState))
            {
                //���� Ű���� ����
                Debug.Log("State Key Error");
                return;
            }

            if((_CurState == PLAYERSTATE.DEAD) &&
               (_CurState == PLAYERSTATE.DEADHIGHLIGHT) &&
               (_CurState == PLAYERSTATE.DEADFLY))
            {
                return;
            }

            //���� ������Ʈ�� ���� State�� ��
            PrevState = CurState;
            CurState = newState;

            //�� State Exit�� �� State Enter
            StateMap[PrevState].StateExit();
            StateMap[CurState].StateEnter();
        }

    }
}
