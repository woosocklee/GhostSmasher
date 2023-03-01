using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    class SmashState : State_Base
    {
        private float CurTime;
        private float HiglightTime = 0.3f;
        public override void Setting() { StateType = PLAYERSTATE.SMASH; }
        public override void StateEnter()
        {
            //Player �ǰݴ����� �ʰ� ����
            Owner.gameObject.layer = LayerMask.NameToLayer("Imotal");
            Owner_rigidbody.isKinematic = true;
            Owner_Script.IsUpdate = false;

            //ī�޶� ����
            Owner_Script.CameraChange();

            //���ϸ��̼� ��� ����
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationPlay(Pv_ID, true);
            }
            else
            {
                Owner_animator.StartPlayback();
            }

            CoroutineHelper.StartCoroutine(SetInitialization());

            //���޽� �ð����� ������Ʈ�� ������
            Debug.Log("���޽� ��");
            state_Machine.ChangeState(state_Machine._PrevState);
        }

        public override void Update()
        {
         /*   CurTime += Time.deltaTime;

            //�����ð��� ������ DeadFlyState�� ��ȯ
            if (HiglightTime < CurTime)
            {
                //���� State�� ���ư�����
                state_Machine.ChangeState(state_Machine._PrevState);
            }*/
        }

        public override void StateExit()
        {
            /*//Ÿ�̸� �ʱ�ȭ
            CurTime = 0.0f;

            //�ִϸ��̼� �簡��
            GameManager.Instance.AnimationStart(Pv_ID);

            //�����ۿ� �ٽ� Ȱ��ȭ ��Ű��
            Owner_rigidbody.isKinematic = false;

            //ī�޶� �ٽ� �ǵ�����
            //Owner_Script.CameraChange();

            //Layer�ٽ� �ǵ�����
            Owner.gameObject.layer = LayerMask.NameToLayer("Player");*/
        }

        IEnumerator SetInitialization()
        {
            yield return new WaitForSeconds(HiglightTime);

            Debug.Log(state_Machine._CurState);
            Debug.Log("���޽� ����");
            //�ִϸ��̼� ���
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationPlay(Pv_ID, true);
            }
            else
            {
                Owner_animator.StopPlayback();
            }

            //�����ۿ� �ٽ� Ȱ��ȭ ��Ű��
            Owner_rigidbody.isKinematic = false;

            //ī�޶� �ٽ� �ǵ�����
            Owner_Script.CameraChange();

            //Layer�ٽ� �ǵ�����
            Owner.gameObject.layer = LayerMask.NameToLayer("Player");

            //���޽� �ð����� ������Ʈ�� ������
            Owner_Script.IsUpdate = true;
        }
    }
}
