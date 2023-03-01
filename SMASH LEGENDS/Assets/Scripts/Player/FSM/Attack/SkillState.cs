using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class SkillState : State_Base
    {
        public override void Setting()
        {
            StateType = PLAYERSTATE.SKILL;
        }

        public override void StateEnter()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Skill", true, Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.SKILL, true, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Skill",true);
                Owner_Script.IK_Controller.IKEvent(PLAYERSTATE.SKILL, true);
            }

            //���ʿ��� ��ų ����� �˸�
            Owner_Script.UseSkill();
        }

        public override void Update()
        {
            if (Owner_animator.GetCurrentAnimatorStateInfo(0).IsName("SkillAttack") &&
                Owner_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                //��ų �ִϸ��̼��� ���� �� �� IdleState�� ��ȯ
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationBool("Skill", false, Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.SKILL, false, Pv_ID);
            }
            else
            {
                Owner_animator.SetBool("Skill", false);
                Owner_Script.IK_Controller.IKEvent(PLAYERSTATE.SKILL, false);
            }
        }
    }
}
