using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{


    public class Skill_BouncingSlash : Skill_Slash
    {
        [SerializeField]
        Vector2 BouncingDirection;

        public Skill_BouncingSlash(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }
        protected override Vector3 DrawDirection(GameObject otherobj)
        {
            Vector3 direction = otherobj.transform.position - ParentPlayer.transform.position;
            direction.y = 0f;
            direction.Normalize();
            direction = direction * BouncingDirection.x + Vector3.down * BouncingDirection.y;

            return direction;
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
            GameManager.Instance.MovePlayer(otherobj.transform.position + (Direction / 60), HitObj.ID);
            GameManager.Instance.Hurt(Direction, debuffDuration, AttackType, Damage, HitSoundNumber, ParentScript.ID, HitObj.ID);
            //�������� 0�� �ƴ϶�� ī�޶�ȿ�� �� HitEffect����
            if (Damage != 0)
            {
                AfterEffect(Junpyo.EFFECT.HIT, otherobj.GetComponent<Collider>());
                GameManager.Instance.CameraShaking(ParentScript.ID);
                //PlayHitSound();
            }
        }
    }
}