using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class StandUpAttackState : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.STANDUPATTACK;
        }

        public override void StateEnter()
        {
            //Ű �� �ޱ�
            MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            //Ű������ �ü� ����
            Owner.LookAt(Owner.position + MoveDirection);

            //�ִϸ��̼� ��� 
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("StandUpAttack", true, Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.STANDUPATTACK, true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("StandUpAttack", true);
                OwnerIK.IKEvent(PLAYERSTATE.STANDUPATTACK, true);
            }

        }

        public override void Update()
        {
            AnimatorStateInfo Info = Owner_animator.GetCurrentAnimatorStateInfo(0);

            if (Info.normalizedTime > 0.97 && Info.IsName("StandUpAttack"))
            {
                //�ִϸ��̼� ����� �������� �̾���
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            //���̷� �ٽ� Player�� ��ü
            Owner.gameObject.layer = LayerMask.NameToLayer("Player");

            //�ִϸ��̼� ����
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("StandUpAttack", false, Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.STANDUPATTACK, false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("StandUpAttack", false);
                OwnerIK.IKEvent(PLAYERSTATE.STANDUPATTACK, false);
            }
        }
    }
}
