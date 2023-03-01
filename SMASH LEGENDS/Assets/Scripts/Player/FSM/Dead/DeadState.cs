using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Junpyo
{
    class DeadState : State_Base
    {
        [SerializeField] private float CurTime;
        private float ReviveWaitingTime = 5.0f;
        private GameObject RevivalUI;
        private Image WaitingImage;
        private RevivalUI revival_UI_Script;

        public override void Setting()
        {
            StateType = PLAYERSTATE.DEAD;
            RevivalUI = Owner_Script.RevivalUI.gameObject;
            WaitingImage = Owner_Script.RevivalWaitingImage;
            revival_UI_Script = RevivalUI.transform.GetChild(0).GetComponent<RevivalUI>();
        }
        public override void StateEnter()
        {
            //��ȰUI����
            RevivalUI.SetActive(true);

            //������ ���� ���� �ƴ϶�� ���� �ڽ��� �׿����� ǥ����
            if (!Owner_Script.fallDead)
            {
                revival_UI_Script.SetRevivalUI(Owner_Script.CharacterName, Owner_Script.EnemyCharacter);
            }
            else
            {
                revival_UI_Script.gameObject.SetActive(false);
                Owner_Script.fallDead = false;
            }

            if (PhotonNetwork.IsConnected)
            {
                EffectManager.Instance.EffectInst(EFFECT.DEAD, Owner.transform.position);
            }
        }

        public override void Update()
        {
            //��Ȱ�ð��� �ʱ�ȭ
            CurTime += Time.deltaTime;

            //uI�� ���� ��Ȱ���ð� ǥ��
            WaitingImage.fillAmount = (CurTime / ReviveWaitingTime);

            //������ �ð��� ���ð��� �ѱ�� ��Ȱ
            if (ReviveWaitingTime < CurTime)
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            //Ÿ�̸� �ʱ�ȭ
            CurTime = 0.0f;

            //Player���� �ʱ�ȭ
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.Dead(true, Pv_ID);
            }
            else
            {
                Owner_Script.Dead(true);
            }

            Owner_rigidbody.isKinematic = false;
            Owner_rigidbody.useGravity = true;

            Owner_Script.playerInformation.Cur_HP = Owner_Script.playerInformation.HP_Max;

            //SpawnPos�� ��ġ ����
            Owner.position = Owner_Script.SpawnPos;
            //Owner.position -= new Vector3(0, Owner.position.y - 5.0f, 0);

            //PlayerAnimation �ٽ� �缺
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationPlay(Pv_ID, true);
                GameManager.Instance.HP(Owner_Script.playerInformation.HP_Max, Pv_ID);
            }
            else
            {
                Owner_animator.StopPlayback();
            }

            //Ui�� GroundCheak��� Ȱ��ȭ
            HP_Bar.fillAmount = 1.0f;

            //RevivalUi ��Ȱ��ȭ �� �ʱ�ȭ
            RevivalUI.gameObject.SetActive(false);
            revival_UI_Script.gameObject.SetActive(true);
            WaitingImage.fillAmount = 1;       

            //LookCheck Ȱ��ȭ
            Owner_Script.LookCheck.SetActive(true);

            //Layer�� Player�� ����
            Owner.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
