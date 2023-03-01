using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    class DeadHighlightState : State_Base
    {
        private float CurTime;
        private float HiglightTime = 0.5f;

        public override void Setting() { StateType = PLAYERSTATE.DEADHIGHLIGHT; }

        public override void StateEnter()
        {
            //Player �ǰݴ����� �ʰ� ����
            Owner.gameObject.layer = LayerMask.NameToLayer("Dead");
            Owner_rigidbody.isKinematic = true;

            //Deadī�޶�� ����
            Owner_Script.CameraChange();

            //���� �ִϸ��̼��� ������Ŵ

            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Hurt", false, Pv_ID);
                GameManager.Instance.AnimationPlay(Pv_ID, true);
            }
            else
            {
                Owner_animator.SetBool("Hurt", true);
                Owner_animator.StartPlayback();
            }
        }

        public override void Update()
        {
            CurTime += Time.deltaTime;

            //�����ð��� ������ DeadFlyState�� ��ȯ
            if (HiglightTime < CurTime)
            {
                //���󰡴� ����� �ٲ�
                state_Machine.ChangeState(PLAYERSTATE.DEADFLY);
            }
        }

        public override void StateExit()
        {
            CurTime = 0.0f;

            //ī�޶� MainCamera�� ��ü
            Owner_Script.CameraChange();

            //���󰡴� �ܰ迡�� GroundCheak�� UI����
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.Dead(false, Pv_ID);
            }
            else
            {
                Owner_Script.Dead(false);
            }
        }
    }
}
