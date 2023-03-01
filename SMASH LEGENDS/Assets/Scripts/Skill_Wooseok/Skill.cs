using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Junpyo;
using Photon.Pun;
using Photon.Realtime;

namespace Wooseok
{
    public abstract class Skill : MonoBehaviour
    {
        // Start is called before the first frame update
        
        

        public enum SKILLTYPE
        {
            BASE = 0,
            SKILL = 1,
            ULTIMATE = 2
        }

        [SerializeField]
        protected Juhyung.AudioManager myAudio;
        public int SkillSoundNumber;
        public int HitSoundNumber;

        public SKILLTYPE SkillType;

        //True�� ��ü�� Player�� �ڽ����� �����Ǹ� �⺻���ݰ��� Player�� �̵������� ���� ��ų�鿡 ������
        public bool IshaveParent;

        [SerializeField]
        public Collider[] cols;

        [SerializeField]
        public Skill FollowUpSkill;

        [SerializeField]
        protected float LifeTime;
        [SerializeField]
        protected float timer;
        [SerializeField]
        protected int MaxHit; // ��ų ��ü�� Ÿ���� ������ �ִ� ��� Curhit�� ���� ����ϱ�.
        [SerializeField]
        protected int curhit;

        [SerializeField]
        protected int MaxHitPerTarget; // �� ����� �ִ� ��Ÿ���� �� ��ų�� ���� �� �ִ°� üũ�ϱ�.
        [SerializeField]
        protected int MaxTargetNumber; // �ִ� �� ����� ��ų�� �¾Ƶ� �Ǵ°�?
        [SerializeField]
        public List<Pair<GameObject,int>> slappedtarget; // ������� ���� ���, ���� Ƚ��

        public GameObject ParentPlayer;
        public PlayerController_FSM ParentScript;

        [SerializeField]
        protected ATTACKTYPE AttackType;

        [SerializeField]
        public float Damage;

        [SerializeField]
        protected Vector2 startvector;

        [SerializeField]
        protected float debuffDuration;

        //�ñر� ������ ������
        [SerializeField]
        protected int Ultimatecharge;

        [SerializeField]
        protected GameObject[] ColliderArr;

        [SerializeField]
        protected Junpyo.SKILLTYPE MyJunpyoSKILLTYPE;
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        public abstract void SkillEffectOnEnter(GameObject otherobj);
        public abstract void SkillEffectOnStay(GameObject otherobj);
        public abstract void SkillEffectOnExit(GameObject otherobj);

        //����Ʈ���� Pair�� Ư�� Key��(2��° �Ű����� ���ʸ� Target targetobject)�� ���� �� ������ true�� ���Ͻ�Ű��, �ƴϸ� false�� ��ȯ��.
        protected bool GameObjectChecker<Target, other>(List<Pair<Target,other>> targetlist, Target targetobject)
        {
            bool result = false;
            foreach (Pair<Target, other> targetpair in targetlist)
            {
                if(result)
                {
                    break;
                }

                if(targetpair.Key.Equals(targetobject))
                {
                    result = true;
                }
            }
            return result;
        }


        //����Ʈ���� Pair�� Ư�� Key��(2��° �Ű����� ���ʸ� Target targetobject)�� ���� �� ������
        //�ش� �� ����Ʈ�� ���°�� �ִ��� ���Ͻ�Ű��
        //������ -1�� ��ȯ��.
        //�� �ڵ� �������� �Ƿ� �̼����� ���� �̷��� C��Ÿ�Ϸ� ��������� ���߿� ���� �������� �� �� ������ ���� ���ϰ��� ����� �����ֱ� �ٶ�.
        protected int TargetFinder<Target,Other>(List<Pair<Target, Other>> targetlist, Target targetobject)
        {
            for (int i = 0; i < targetlist.Count; i++)
            {
                if (targetlist[i].Key.Equals(targetobject))
                {
                    return i;
                }
            }
            return -1;
        }

        public Skill(GameObject ParentPlayer, Skill FollowUp)
        {
            curhit = 0;
            this.ParentPlayer = ParentPlayer;
            this.FollowUpSkill = FollowUp;
        }


        protected virtual void FixedUpdate()
        {
            if (timer < LifeTime)
            {
                timer += Time.fixedDeltaTime;
            }
            else
            {
                SetCollider(false);
            }
            if(myAudio)
            {
                if (!myAudio.PlayCheck() && timer >= LifeTime)
                {
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                if(timer >= LifeTime)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }

        //���ӸŴ����� �����ϰ��ϻ�
        protected virtual void HitEnemy(GameObject otherobj)
        {
            PlayerController_FSM HitObj = otherobj.GetComponent<PlayerController_FSM>();

            //��븦 ������ ����
            Vector3 Direction;

            //���� ��ü�� �ڽ��� ���͸� �̿��� �ڽſ��� ������ü ������ �̵��ϴ� ���͸� ����
            Direction = DrawDirection(otherobj);
            //���� ��ü�� �ǰ��Լ��� ȣ��

            GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
            Debug.Log("HurtOn");
            GameManager.Instance.Hurt(Direction, debuffDuration, AttackType, Damage, HitSoundNumber, ParentScript.ID, HitObj.ID);

            //�������� 0�� �ƴ϶�� ī�޶�ȿ�� �� HitEffect����
            if (Damage != 0)
            {
                AfterEffect(EFFECT.HIT, otherobj.GetComponent<Collider>());
                GameManager.Instance.CameraShaking(ParentScript.ID);
                //PlayHitSound();
            }
            curhit++;
        }
        protected void HitFail()
        {
            Debug.Log("Hitfail");
        }

        //�������� üũ�ϱ�. �����̸� true ������.
        protected bool IsImmortal(GameObject otherobj)
        {
            if (otherobj.layer == LayerMask.NameToLayer("Imotal"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //��븦 ������ ���� ���ϱ�.
        //���� ��ü�� �ڽ��� ���͸� �̿��� �ڽſ��� ������ü ������ �̵��ϴ� ���͸� ����
        protected virtual Vector3 DrawDirection(GameObject otherobj)
        {
            Vector3 direction = otherobj.transform.position - ParentPlayer.transform.position;
            direction.y = 0;
            direction.Normalize();

            return direction;
        }


        protected virtual void Awake()
        {
            slappedtarget = new List<Pair<GameObject, int>>();

            if(FollowUpSkill != null)
            {
                FollowUpSkill.GetComponent<Skill>().ParentPlayer = this.ParentPlayer;
            }
            if(!IshaveParent)
            {
                transform.parent = null;
            }
            GetCols();
            myAudio = this.GetComponent<Juhyung.AudioManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //Ŭ���̾�Ʈ�� �����Ͱ� �ƴϸ� ���� ������ ������Ŭ���̾�Ʈ���� ó������
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            Debug.Log("SkillEffect On Enter");
            SkillEffectOnEnter(other.gameObject); //������ �ٷ� ȿ���� ��Ÿ���� ����
        }
        private void OnTriggerStay(Collider other)
        {
            //Ŭ���̾�Ʈ�� �����Ͱ� �ƴϸ� ���� ������ ������Ŭ���̾�Ʈ���� ó������
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            SkillEffectOnStay(other.gameObject); //���Ǳ� ������, ȿ���� ��Ÿ���� ����
        }
        private void OnTriggerExit(Collider other)
        {
            //Ŭ���̾�Ʈ�� �����Ͱ� �ƴϸ� ���� ������ ������Ŭ���̾�Ʈ���� ó������
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            SkillEffectOnExit(other.gameObject); //������ Ż���� �� ȿ���� ��Ÿ���� ����
        }

        public abstract void FollowUp();

        public abstract void Restart();
        public void SkillStop()
        {
            this.gameObject.SetActive(false);
        }
        
        public void UltimategageCharge()
        {
            ParentScript.UtimateGageUp(Ultimatecharge);
        }

        public void SetParent(GameObject parent)
        {
            ParentPlayer = parent;
            ParentScript = ParentPlayer.GetComponent<PlayerController_FSM>();
        }

        public virtual void HitCoffin(GameObject Coffin)
        {
            if(Coffin.layer == LayerMask.NameToLayer("Coffin"))
            {
               
                Skill_Coffin thisCoffin = Coffin.GetComponent<Skill_Coffin>();
                if(thisCoffin.CurHP <= Damage)
                {
                    GameManager.Instance.Attack((int)Junpyo.SKILLTYPE.ULTIMATE, false, Coffin.GetComponent<Skill>().ParentScript.ID);
                }
                thisCoffin.CurHP -= Damage;
                AfterEffect(Junpyo.EFFECT.HIT, Coffin.GetComponent<Collider>());
                PlayHitSound();
                curhit++;
            }
            else
            {
                return;
            }
        }

        public virtual bool IsItEnemy(GameObject obj)
        {
            if (((obj.layer == LayerMask.NameToLayer("Player")) ||
                (obj.layer == LayerMask.NameToLayer("SuperArmer"))) ||
                (obj.layer == LayerMask.NameToLayer("Coffin")) &&
               !(obj.CompareTag(ParentPlayer.tag)) &&
               (obj != ParentPlayer))
            {
                return true;
            }

            return false;
        }

        public virtual bool CanIHit(GameObject obj)
        {
            if (
                GameObjectChecker(slappedtarget, obj) &&
                curhit < MaxHit &&
                MaxTargetNumber > slappedtarget.Count
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetCollider(bool ison)
        {
            foreach(var col in cols)
            {
                col.enabled = (ison);
            }
        }

        public void AfterEffect(EFFECT effecttype, Collider Target)
        {
            EffectManager.Instance.EffectInst(effecttype, Target.bounds.ClosestPoint(this.gameObject.transform.position));
        }

        protected virtual void GetCols()
        {
            cols = GetComponentsInChildren<Collider>();
        }
        public virtual void PlaySkillSound()
        {
            if(SkillSoundNumber != -1)
            {
                myAudio.Play(Juhyung.SoundManager.Instance.AttSound[SkillSoundNumber]);
            }
        }
        public virtual void PlayHitSound()
        {
            if(HitSoundNumber != -1)
            {
                myAudio.Play(Juhyung.SoundManager.Instance.HitSound[HitSoundNumber]);
            }
        }
    }
    

}

