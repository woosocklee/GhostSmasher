using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    class RollingState : State_Base
    {
        public override void Setting() { StateType = PLAYERSTATE.ROLLING; }

        public override void StateEnter()
        {
            //�������� �Է��� Ű������ ������
            MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            Owner.LookAt(Owner.position + MoveDirection);

            //������ ���� �������¸� ���� Layer��ü
            Owner.gameObject.layer = LayerMask.NameToLayer("Imotal");

            //�ִϸ��̼��� ��ø�Ǵ� ���װ� �־ animator Play����� ���
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationTrigger("Rolling", Pv_ID);
                GameManager.Instance.PlayClip("Rolling", Pv_ID);
            }
            else
            {
                Owner_animator.SetTrigger("Rolling");
                Owner_animator.Play("Rolling", 0);
            }

            //�̵��ϴ� �Ÿ�
            //Owner_rigidbody.velocity = Owner.forward * 12.0f;
        }

        public override void Update()
        {
            //�����Ⱑ ������ Ÿ�ֿ̹� Ű�Է� ����
            if (Owner_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationResetTrigger("Rolling", Pv_ID);
            }
            else
            {
                Owner_animator.ResetTrigger("Rolling");
            }

            //�ִϸ��̼��� �����鼭 �������� ����
            Owner.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
