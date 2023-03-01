using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    class DeadFlyState : State_Base
    {
        //DeadLine
        private float Daed_Y = 15.0f;
        private float FlySpeed = 50.0f;
        private Vector3 FlyDirection;

        public override void Setting() { StateType = PLAYERSTATE.DEADFLY; }

        public override void StateEnter()
        {
            //���� ���������� ���ư��� ����
            Owner_rigidbody.velocity = Vector3.zero;
            Owner_Script.LookCheck.SetActive(false);
            FlyDirection = Owner_Script.DeadFlyDirection;
            FlyDirection.y = 0.5f;
        }

        public override void Update()
        {
            //���� Prefab�� �����ϸ鼭 ����
            if (PhotonNetwork.IsConnected)
            {
                EffectManager.Instance.EffectInst(EFFECT.DUST, Owner.transform.position);
            }

            Owner.Translate(FlyDirection * FlySpeed * Time.deltaTime, Space.World);

            //������� ���� �� DeadState�� ��ȯ
            if (Owner.position.y > Daed_Y)
            {
                state_Machine.ChangeState(PLAYERSTATE.DEAD);
            }
        }

        public override void StateExit()
        {
            //�̸� Hurt Bool�� false�� ����
            Owner_rigidbody.velocity = Vector3.zero;

            //����Ʈ ��ȯ
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Hurt", false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Hurt", false);
            }
        }
    }
}
