using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Junpyo;
using System.ComponentModel;
using Photon.Pun;

namespace Wooseok
{
    public class Skill_CursedCoffin : Skill
    {
        Rigidbody myrigidbody;
        [SerializeField]
        float Range;
        [SerializeField]
        float interval;
        [SerializeField]
        Dictionary<GameObject, Pair<bool, float>> PlayerTimer = new Dictionary<GameObject, Pair<bool, float>>();
        [SerializeField] bool Ground = false;
        [SerializeField] GameObject[] Effect;
        bool Start;

        [SerializeField]
        Skill_Coffin CoffinData;
        Skill_CursedCoffin(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {

        }
        protected override void Awake()
        {
            base.Awake();
            PlayerTimer = new Dictionary<GameObject, Pair<bool, float>>();
            myrigidbody = this.gameObject.GetComponent<Rigidbody>();
        }
        
        public override void FollowUp()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Ground && (other.gameObject.layer == LayerMask.NameToLayer("Ground")))
            {
                Ground = true;
                PlaySkillSound();
                timer = 0f;
                transform.position = new Vector3(transform.position.x, other.bounds.max.y, transform.position.z);

                //����Ʈ �ѱ�
                foreach (GameObject effect in Effect)
                {
                    effect.SetActive(true);
                }

                //�ݶ��̴� �ѱ�
                foreach (GameObject effect in ColliderArr)
                {
                    effect.SetActive(true);
                }

                myrigidbody.isKinematic = true;
                Start = true;
            }
        }

        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            if (Ground)
            {
                if (
                    otherobj != this.ParentPlayer
                    && !ParentPlayer.CompareTag(otherobj.tag)
                    && otherobj.layer == LayerMask.NameToLayer("Player")
                    )
                {
                    //ó�� ���ö� Dictionary�� ���
                    if (!PlayerTimer.ContainsKey(otherobj))
                    {
                        PlayerTimer.Add(otherobj, new Pair<bool, float>(true, 0.0f));
                    }
                    //��ϵ� ��ü�� �ð� ����
                    else
                    {
                        Pair<bool, float> temp_pair = PlayerTimer[otherobj];
                        temp_pair.Key = true;
                        PlayerTimer[otherobj] = temp_pair;
                    }
                }
            }
        }

        public override void SkillEffectOnExit(GameObject otherobj)
        {
            if (Ground)
            {
                //��ü�� ������ ���ϱ� ���� �����ȿ� ���ٴ� ���� �˸��� Ű���� false�� ������
                if (PlayerTimer.ContainsKey(otherobj))
                {
                    Pair<bool, float> pair = PlayerTimer[otherobj];
                    pair.Key = false;
                    PlayerTimer[otherobj] = pair;
                }
            }
            //
        }

        public override void SkillEffectOnStay(GameObject otherobj)
        {
            if (Ground)
            {
                //�ȵ��� ��ü�� ���
                if (
                    otherobj != this.ParentPlayer
                    && !ParentPlayer.CompareTag(otherobj.tag)
                    && otherobj.layer == LayerMask.NameToLayer("Player")
                )
                {
                    if (!PlayerTimer.ContainsKey(otherobj))
                    {                 
                        PlayerTimer.Add(otherobj, new Pair<bool, float>(true, 0.0f));
                    }
                    else
                    {
                        Pair<bool, float> pair = PlayerTimer[otherobj];
                        pair.Key = true;
                        PlayerTimer[otherobj] = pair;
                    }
                }


                //���ֻ��� �ο�
                if (
                    otherobj.layer == LayerMask.NameToLayer("Player") &&
                    !ParentPlayer.CompareTag(otherobj.tag) &&
                    PlayerTimer.ContainsKey(otherobj)
                    )
                {

                    Pair<bool, float> keyvalue;
                    PlayerTimer.TryGetValue(otherobj, out keyvalue);

                    if (keyvalue.Value > interval)
                    {
                        HitEnemy(otherobj);
                        GameManager.Instance.GagePus(Ultimatecharge, ParentScript.ID);
                        keyvalue.Value = 0.0f;
                        PlayerTimer[otherobj] = keyvalue;
                    }
                }
            }
        }

        //Update is called once per frame
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (Ground)
            {
                //�ڵ� ��Ȱ��
                
                Pair<bool, float> pair;
                if (PlayerTimer.Count > 0)
                {
                    //��ųʸ� Ư���� ���� ������ ��������� ���� ���� �� ���� �ް� �����Ŀ� ��ųʸ��� ����
                    var keylist = new List<GameObject>(PlayerTimer.Keys);
                    for (int i = 0; i < PlayerTimer.Count; i++)
                    {
                        pair = PlayerTimer[keylist[i]];
                        pair.Value += pair.Key ? Time.fixedDeltaTime : 0.0f;
                        PlayerTimer[keylist[i]] = pair;
                    }
                }
                if(Start)
                {
                    //����Ʈ ������ ���� ��������� ��ƼŬ�� �̹� �ִ� ����̱⿡ �����ʿ��� ���� �ִ�. Paticle.LifeSize
                    Effect[0].transform.localScale = new Vector3((Effect[0].transform.localScale.x + (Time.fixedDeltaTime * 11f)),
                        1, (Effect[0].transform.localScale.z + (Time.fixedDeltaTime *11f)));
                    
                    if(Effect[0].transform.localScale.x >= 3.7f)
                    {
                        Start = false;
                        Effect[0].transform.localScale = new Vector3(3.7f, 1f, 3.7f);
                    }
                }
            }
        }

        public override void Restart()
        {
            this.tag = this.ParentPlayer.tag;

            if(CoffinData != null)
            {
                //���� ���� �ʱ�ȭ
                if (CoffinData.CurHP <= 0)
                {
                    CoffinData.CurHP = CoffinData.MaxHP;
                }
            }
            
            //��ġ ����
            if(!IshaveParent)
            {
                this.gameObject.transform.position = (ParentPlayer.transform.position
                + this.ParentPlayer.transform.forward * Range) + new Vector3(0, startvector.y, 0);
            }
            else
            {
                this.gameObject.transform.localPosition = Vector3.zero;
            }

            //���� �ʱ�ȭ
            timer = 0f;
            curhit = 0;
            slappedtarget.Clear();
            PlayerTimer.Clear();

            //����Ʈ �ʱ�ȭ
            Effect[0].transform.localScale = Vector3.zero;

            SetCollider(true);

            Ground = false;
            myrigidbody.isKinematic = false;
            myrigidbody.velocity = Vector3.zero;
            
        }

        public override void PlaySkillSound()
        {
            base.PlaySkillSound();
            if (HitSoundNumber != -1)
            {
                myAudio.Play(Juhyung.SoundManager.Instance.AttSound[HitSoundNumber], Juhyung.AudioManager.DEFINE.REPEAT);
            }
        }
    }
}