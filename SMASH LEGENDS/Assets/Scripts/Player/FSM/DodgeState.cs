using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Junpyo
{
    public class DodgeState : State_Base
    {
        float DodgeSpeed = 15.0f;
        float Timer;
        float Magnitude = 19;

        public override void Setting() { StateType = PLAYERSTATE.DODGE; }

        public override void StateEnter()
        {
            //Rigidbody�ʱ�ȭ �� �߷� ����
            Owner_rigidbody.velocity = Vector3.zero;
            //Owner_rigidbody.useGravity = false;
            //�������� ����
            Owner.gameObject.layer = LayerMask.NameToLayer("Imotal");
            //Ű�Է¹ޱ�
            Owner.LookAt(Owner.position + MoveDirection);



            //�ִϸ��̼� ���
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationTrigger("Dodge",Pv_ID);
            }
            else
            {
                Owner_animator.SetTrigger("Dodge");
            }

            Owner_rigidbody.velocity = (DodgeSpeed * -Owner.forward) + new Vector3(0, 5f, 0);
        }

        public override void Update()
        {
            //�������·� �̵�
            if (0.05 <= Timer)
            {
                Owner_rigidbody.velocity -= new Vector3(0, Time.deltaTime  * Magnitude, 0);
                //Owner_rigidbody.velocity -= new Vector3(0, Time.deltaTime * -Magnitude, 0);
            }

            AnimatorStateInfo info = Owner_animator.GetCurrentAnimatorStateInfo(0);

            //Dodge�ð� üũ
            Timer += Time.deltaTime;

            //ȸ�Ǳ� �ð��� ������ �ڵ����� ���ƿ�
            if ((info.normalizedTime > 0.97f) && info.IsName("Dodge"))
            {
                //�������� �ƴ����� �Ǵ� �� StateChange
                if (Mathf.Abs(GroundPos.position.y - Owner.position.y) >= 0.1f)
                {
                    state_Machine.ChangeState(PLAYERSTATE.DODGEAIR);
                }
                else
                {
                    state_Machine.ChangeState(PLAYERSTATE.IDLE);
                }
            }


        }

        public override void StateExit()
        {
            Timer = 0.0f;

            //Layer�缳��
            Owner.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
