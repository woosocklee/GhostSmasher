using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class UltimateState : State_Base 
    {
        float Magnitude = 19;

        public override void Setting()
        {
            StateType = PLAYERSTATE.ULTIMATE;
        }

        public override void StateEnter()
        {
            Debug.Log("�ñر� ���");
            //Layer��ä
            Owner.gameObject.layer = LayerMask.NameToLayer("SuperArmer");

            //�ñر� ������ 0���� �ʱ�ȭ �� ���µ� �ʱ�ȭ
            Owner_Script.UseUtimate();


            //�ִϸ��̼� ���
            if(PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Ultimate", true,Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Ultimate", true);
            }
        }

        public override void Update()
        {
            if (Owner_rigidbody.velocity.y <= 0.2f)
            {
                Owner_rigidbody.velocity -= new Vector3(0, Time.deltaTime * Magnitude, 0);
            }

            AnimatorStateInfo info = Owner_animator.GetCurrentAnimatorStateInfo(0);

            if (info.normalizedTime > 0.97f && info.IsName("Ultimate"))
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Ultimate", false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Ultimate", false);
            }

            Owner.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
