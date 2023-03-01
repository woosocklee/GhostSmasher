using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class LandState : State_Base
    {
        private float JumpStopDelay = 0.3f;
        private float CurDelay = 0.0f;

        public override void Setting() { StateType = PLAYERSTATE.LAND; }

        public override void StateEnter()
        {
            //������ ��ð���
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationTrigger("Land", Pv_ID);
            }
            else
            {
                Owner_animator.SetTrigger("Land");
            }
            Owner_rigidbody.velocity = Vector3.zero;
        }

        public override void Update()
        {
            CurDelay += Time.deltaTime;

            //Land���°� ���� �� Idle�� ��ü
            if (CurDelay > JumpStopDelay)
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            CurDelay = 0.0f;

            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationResetTrigger("Land", Pv_ID);
            }
            else
            {
                Owner_animator.ResetTrigger("Land");
            }
        }
    }
}
