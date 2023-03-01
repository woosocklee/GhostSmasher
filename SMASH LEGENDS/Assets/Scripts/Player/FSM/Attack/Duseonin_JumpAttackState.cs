using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class Duseonin_JumpAttackState : State_Base
    {
        private bool IsStartAttack = false;
        private float StartAttackDelay = 0.1f;
        private float CurDelay = 0.0f;

        public override void Setting()
        {
            StateType = PLAYERSTATE.DUSEONIN_JUMPATTACK;
        }

        public override void StateEnter()
        {
            //���߿��� ���� ����
            Owner_rigidbody.velocity = Vector3.zero;
            IsStartAttack = true;

            //�ִϸ��̼� ���� IK�� ����
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.IKEvent(PLAYERSTATE.JUMPATTACK, true, Pv_ID);
                GameManager.Instance.AnimationSetLayerWeight(1, 1, Pv_ID);
                GameManager.Instance.AnimationTrigger("JumpAttack",Pv_ID);
            }
            else
            {
                OwnerIK.IKEvent(PLAYERSTATE.JUMPATTACK, true);
                Owner_animator.SetLayerWeight(1, 1);
                Owner_animator.SetTrigger("JumpAttack");
            }
        }

        public override void Update()
        {
            AnimatorStateInfo StateInfo = Owner_animator.GetCurrentAnimatorStateInfo(1);

            if (StateInfo.IsName("JumpAttack1") || StateInfo.IsName("JumpAttack2"))
            {
                InputZ();
            }
            else if (!StateInfo.IsName("JumpAttack3") && !IsStartAttack)
            {
                if(Owner.position.y > GroundPos.position.y)
                {
                    state_Machine.ChangeState(PLAYERSTATE.AIR);
                }
                else
                {
                    state_Machine.ChangeState(PLAYERSTATE.IDLE);
                }
            }

            if (IsStartAttack)
            {
                if (StartAttackDelay < CurDelay)
                {
                    IsStartAttack = false;
                    CurDelay = 0.0f;
                }

                CurDelay += Time.deltaTime;
            }
        }

        public override void StateExit()
        {
            //���߿��� ���� ����
            IsStartAttack = true;
            CurDelay = 0.0f;

            //�ִϸ��̼� ���� IK�� ����
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.IKEvent(PLAYERSTATE.JUMPATTACK, false, Pv_ID);
                GameManager.Instance.AnimationSetLayerWeight(1, 0, Pv_ID);
            }
            else
            {
                OwnerIK.IKEvent(PLAYERSTATE.JUMPATTACK, false);
                Owner_animator.SetLayerWeight(1, 0);
            }
        }

        public void InputZ()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (PhotonNetwork.IsConnected)
                {
                    GameManager.Instance.AnimationTrigger("JumpAttack", Pv_ID);
                }
                else
                {
                    Owner_animator.SetTrigger("JumpAttack");
                }
            }
        }
    }
}
