using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class ChePeSyuUltimateState : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.CHEPESYULTIMATE;
        }

        public override void StateEnter()
        {
            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Ultimate", true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Ultimate", true);
            }
        }

        public override void Update()
        {
            //ü�佴�� ���� ���� ��� �ִϸ��̼��� �ٽ� �����
            if(GroundPos.position.y > Owner.position.y)
            {
                if (PhotonNetwork.IsConnected)
                {
                    GameManager.Instance.AnimationPlay(Pv_ID, true);
                }
                else
                {
                    Owner_animator.StopPlayback();
                }
            }

            //�ִϸ��̼� �ð��� ������ Idle���·� ��ȯ
            if(Owner_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95)
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            base.StateExit(); if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Ultimate", false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Ultimate", false);
            }

            Owner.gameObject.layer = LayerMask.NameToLayer("Player");

            //���� �ʹ����� ������ ���� ���
            if(state_Machine._CurState == PLAYERSTATE.DEAD)
            {
                GameManager.Instance.Attack((int)SKILLTYPE.ULTIMATE, false, Pv_ID);
            }
        }
    }
}
