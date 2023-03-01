using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wooseok
{
    public class TrueLove_JumpAttack : Skill_Slash
    {
        public TrueLove_JumpAttack(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
        }

        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            if (
                (ParentPlayer.tag != otherobj.tag)
                && ParentPlayer != otherobj
                && MaxHit > curhit
                && MaxTargetNumber > slappedtarget.Count
                && !GameObjectChecker(slappedtarget, otherobj)
                )
            {
                Debug.Log("SlappedTarget: " + slappedtarget);
                Debug.Log("otherobj: " + otherobj);

                if (IsItEnemy(otherobj))
                {
                    //���� ���ߴµ� ���������� TrueLove�� State�� JumpAttackSuccess�� ��ü
                    HitEnemy(otherobj);
                    GameManager.Instance.StateChange(Junpyo.PLAYERSTATE.TRUELOVE_JUMPATTACKSUCCESS, ParentScript.ID);
                    slappedtarget.Add(new Pair<GameObject, int>(otherobj, 1));
                    curhit++;
                }
                else if((otherobj.layer == LayerMask.NameToLayer("Coffin")))
                {
                    HitCoffin(otherobj);
                    slappedtarget.Add(new Pair<GameObject, int>(otherobj, 1));
                    curhit++;
                }
            }
        }
    }
}
