using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public enum EFFECT { DUST, DEAD, HIT, HILL, DAMAGE}
    public class EffectManager : MonoBehaviourPunCallbacks
    {
        private static EffectManager _instance;

        [SerializeField] GameObject DustEffect;
        [SerializeField] GameObject DeadEffect;
        [SerializeField] GameObject HitEffect;
        [SerializeField] GameObject DamageEffect;

        //�̸� �Ҵ�� ����Ʈ��
        Queue<GameObject> queDustEffect = new Queue<GameObject>();
        Queue<GameObject> queDeadEffect = new Queue<GameObject>();
        Queue<GameObject> queHitEffect = new Queue<GameObject>();
        Queue<GameObject> queDamageEffect = new Queue<GameObject>();

        public float magnitude;

        public static EffectManager Instance
        {
            get
            {
                // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(EffectManager)) as EffectManager;   
                }

                return _instance;
            }
        }

        private void Start()
        {
            //Effect���� Stack�� �Ҵ�
            CreateEffect(queDustEffect, DustEffect, 300);
            CreateEffect(queDeadEffect, DeadEffect, 6);
            CreateEffect(queHitEffect, HitEffect, 100);
            CreateEffect(queDamageEffect, DamageEffect, 100);
        }

        public void CreateEffect(Queue<GameObject> queEffect, GameObject effect, int number)
        {
            for(int i = 0; i < number; ++i)
            {
                //������ queue�� �Ҵ�
                queEffect.Enqueue(Instantiate(effect, transform.position, Quaternion.Euler(Vector3.zero)));
            }
        }

        public void EffectInst(EFFECT type, Vector3 pos, int dag = 0)
        {
            photonView.RPC("EffectInstRPC", RpcTarget.All, type, pos, dag);
        }

        [PunRPC]
        public void EffectInstRPC(EFFECT type, Vector3 pos, int dag)
        {
            GameObject effect = null;

            switch (type)
            {
                case EFFECT.DUST:
                    effect = queDustEffect.Dequeue();
                    effect.transform.position = pos;
                    effect.SetActive(true);
                    queDustEffect.Enqueue(effect);
                    break;
                case EFFECT.DEAD:
                    effect = queDeadEffect.Dequeue();
                    effect.transform.position = pos;
                    effect.transform.Rotate(Vector3.right * 90, Space.World);
                    effect.SetActive(true);
                    queDeadEffect.Enqueue(effect);
                    break;
                case EFFECT.HIT:
                    effect = queHitEffect.Dequeue();
                    effect.transform.position = (Vector3)Random.insideUnitSphere * magnitude + pos;
                    effect.SetActive(true);
                    queHitEffect.Enqueue(effect);
                    break;
                case EFFECT.DAMAGE:
                    effect = queDamageEffect.Dequeue();
                    effect.transform.position = (Vector3)Random.insideUnitSphere * magnitude + pos;
                    effect.GetComponent<DamageValue>().SetDamage_Value(dag);
                    effect.SetActive(true);
                    queDamageEffect.Enqueue(effect);
                    break;
            }
        }
    }
}
