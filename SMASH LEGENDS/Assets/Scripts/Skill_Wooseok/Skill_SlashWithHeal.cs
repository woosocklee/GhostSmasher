using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    public class Skill_SlashWithHeal : Skill_Slash
    {
        [SerializeField]
        int EndSkillSoundNumber;

        [SerializeField]
        int EndSkillHitNumber;

        public Skill_SlashWithHeal(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }
        
        public void HealSelf()
        {
            //��븦 ������ ����
            Vector3 Direction = Vector3.zero;
            GameManager.Instance.Hurt(Direction, debuffDuration, ATTACKTYPE.HEAL,-ParentScript.playerInformation.HP_Max * 0.1f, -1,ParentScript.ID, ParentScript.ID);
        }

        public override void PlaySkillSound()
        {
            if(timer >= 0.45f)
            {
                if (EndSkillSoundNumber != -1)
                {
                    myAudio.Play(Juhyung.SoundManager.Instance.AttSound[EndSkillSoundNumber]);
                }
            }
            else
            {
                base.PlaySkillSound();
            }
        }

        void EndHitSound()
        {
            if (EndSkillHitNumber != -1)
            {
                myAudio.Play(Juhyung.SoundManager.Instance.AttSound[EndSkillHitNumber]);
            }
        }

        protected override void HitEnemy(GameObject otherobj)
        {
            Junpyo.PlayerController_FSM HitObj = otherobj.GetComponent<Junpyo.PlayerController_FSM>();

            //��븦 ������ ����
            Vector3 Direction;

            //���� ��ü�� �ڽ��� ���͸� �̿��� �ڽſ��� ������ü ������ �̵��ϴ� ���͸� ����
            Direction = DrawDirection(otherobj);
            //���� ��ü�� �ǰ��Լ��� ȣ��

            GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
            Debug.Log("HurtOn");
            if (timer >= 0.5f)
            {
                GameManager.Instance.Hurt(Direction, debuffDuration, AttackType, Damage, EndSkillHitNumber, ParentScript.ID, HitObj.ID);
            }
            else
            {
                GameManager.Instance.Hurt(Direction, debuffDuration, AttackType, Damage, HitSoundNumber, ParentScript.ID, HitObj.ID);
            }
            

            //�������� 0�� �ƴ϶�� ī�޶�ȿ�� �� HitEffect����
            if (Damage != 0)
            {
                AfterEffect(Junpyo.EFFECT.HIT, otherobj.GetComponent<Collider>());
                GameManager.Instance.CameraShaking(ParentScript.ID);

            }
        }
    }
}