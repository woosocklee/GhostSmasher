using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    class HangAttackState : State_Base
    {
        public override void Setting() { StateType = PLAYERSTATE.HANGATTACK; }

        public override void StateEnter()
        {
            //Player��ġ Ground��ġ�� ����
            //Owner.position = GroundPos.position;

            //�ִϸ��̼� ���
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("HangAttack", true, Pv_ID);
                GameManager.Instance.AnimationBool("isAttack", true, Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.HANGATTACK, true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("HangAttack", true);
                Owner_animator.SetBool("IsAttack", true);
                OwnerIK.IKEvent(PLAYERSTATE.HANGATTACK, true);
            }
        }

        public override void Update()
        {
            //������ ���� �� �� Idle���·� ��ȯ
            if (Owner_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f && Owner_animator.GetCurrentAnimatorStateInfo(0).IsName("HangAttack"))
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            //�ִϸ��̼� ���
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("HangAttack", false, Pv_ID);
                GameManager.Instance.AnimationBool("isAttack", false, Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.HANGATTACK, true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("HangAttack", false);
                Owner_animator.SetBool("IsAttack", false);
                OwnerIK.IKEvent(PLAYERSTATE.HANGATTACK, false);
            }

            Owner.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
