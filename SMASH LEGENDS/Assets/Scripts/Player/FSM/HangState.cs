using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Junpyo
{
    public class HangState : State_Base
    {
        //3�ʷ� �����Ǿ� ����
        private float HangMaxTime = 3.0f;
        private float HangCurTime;
        public Image StaminaUI;
        private Transform LookCheck;
        private GroundCheack GroundCheackScript;

        public override void Setting()
        {
            StateType = PLAYERSTATE.HANG;
            StaminaUI = Owner_Script.StaminaUI;
            LookCheck = Owner_Script.LookCheck.transform;
            GroundCheackScript = GroundPos.GetComponent<GroundCheack>();
        }

        public override void StateEnter()
        {
            Owner_Canvas.GetComponent<Billboard>().Hang = true;

            //���¹̳�UI Ȱ��ȭ
            StaminaUI.enabled = true;

            //velocity�� �ʱ��� �� �߷� ����
            Owner_rigidbody.velocity = Vector3.zero;
            Owner_rigidbody.useGravity = false;

            //Owner_animator.SetBool("Hang", true);
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Hang", true, Pv_ID);
                GameManager.Instance.IKEvent(StateType, true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Hang", true);
                OwnerIK.IKEvent(StateType, true);
            }

            //LookCheck��ġ ����
            LookCheck.localPosition -= new Vector3(0, Owner_Script.playerInformation.Hang_Y, 0);

            Owner.gameObject.layer = LayerMask.NameToLayer("HangAttack");

            GroundCheackScript.Hang = true;
            GroundCheackScript.LineSet();
        }

        public override void Update()
        {
            //Hang�ִ�ð��� �Ѿ�� Hang���¸� Ǯ��
            if (HangCurTime >= HangMaxTime)
            {
                //Fall���·� ����
                state_Machine.ChangeState(PLAYERSTATE.DROP);
                return;
            }

            //GroundCheck ��ġ ����
            GroundPos.position = Owner.position + new Vector3(0, -Owner_Script.playerInformation.Hang_Y + 0.2f, 0);

            //�Ŵٸ��� �ð� ����
            HangCurTime += Time.deltaTime;

            //�ð��� ���� ���׹̳� �� ǥ��
            StaminaUI.fillAmount = 1 - (HangCurTime / HangMaxTime);

            //������ ����ġ�� �÷��̾� ��ġ�� �ʱ�ȭ
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Owner.position = GroundPos.position + new Vector3(0, 0.4f, 0) + Owner.forward * 0.2f;
                state_Machine.ChangeState(PLAYERSTATE.JUMP);
                Owner.gameObject.layer = LayerMask.NameToLayer("Player");
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                //HangAttack����
                state_Machine.ChangeState(PLAYERSTATE.HANGATTACKPREPARE);
            }
        }

        public override void StateExit()
        {
            //üũŸ�� �ʱ�ȭ
            HangCurTime = 0;

            //�߷� Ȱ��ȭ
            Owner_rigidbody.useGravity = true;

            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Hang", false, Pv_ID);
                GameManager.Instance.IKEvent(StateType, false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Hang", false);
                OwnerIK.IKEvent(StateType, false);
            }

            //���׹̳� ǥ�� �� ĵ���� ��ġ �ٽ� �÷��̷� ��ġ�� ����
            StaminaUI.enabled = false;

            Owner_Canvas.GetComponent<Billboard>().Hang = false;
            //LookCheck��ġ ����
            LookCheck.localPosition += new Vector3(0, Owner_Script.playerInformation.Hang_Y, 0);

            GroundCheackScript.Hang = false;
            GroundCheackScript.LineSet();
        }
    }
}
