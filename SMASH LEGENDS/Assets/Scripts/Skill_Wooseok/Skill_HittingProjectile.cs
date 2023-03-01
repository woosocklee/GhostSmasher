using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    public class Skill_HittingProjectile : Skill_ProjectileGoUnder
    {
        
        protected Skill_HittingProjectile(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }

        [SerializeField]
        Vector3 compensationvector;

        protected override void Start()
        {
            base.Start();
            this.transform.position += compensationvector;
        }
        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            Debug.Log("��ų ������ �ε���");
            if ((ParentPlayer.tag != otherobj.tag)
                && otherobj.layer == LayerMask.NameToLayer("Player")
                && ParentPlayer != otherobj
                && MaxHit > curhit
                && MaxTargetNumber > slappedtarget.Count
                && !GameObjectChecker(slappedtarget, otherobj)
                )
            {
                Debug.Log("�÷��̾� ���� ��ü�� ����");
                slappedtarget.Add(new Pair<GameObject, int>(otherobj, 1));
                if (otherobj.layer == LayerMask.NameToLayer("Player"))
                {
                    HitEnemy(otherobj);
                    GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
                }
                else if (otherobj.layer == LayerMask.NameToLayer("Coffin"))
                {
                    HitCoffin(otherobj);
                }
                curhit++;
                isdestroy = true;
            }
        }
        public override void SkillEffectOnStay(GameObject otherobj)
        {
            this.SkillEffectOnEnter(otherobj);
        }
    }
}