using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{

    public class StandUpState : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.STANDUP;
        }

        public override void StateEnter()
        {
            //���̾� �������� ����
            Owner.gameObject.layer = LayerMask.NameToLayer("Imotal");

            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("StandUp",true,Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("StandUp", true);
            }
        }

        public override void Update()
        {
            AnimatorStateInfo Info = Owner_animator.GetCurrentAnimatorStateInfo(0);

            if (Info.normalizedTime > 0.97 && Info.IsName("StandUp"))
            {
                Debug.Log("�̰Ŵ� �� ����");
                //�ִϸ��̼� ����� �������� �̾���
                state_Machine.ChangeState(PLAYERSTATE.STANDUPATTACK);
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("StandUp", false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("StandUp", false);
            }
        }
    }
}
