using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Junpyo
{
    public class KillLogManager : MonoBehaviour
    {
        static KillLogManager _instance = null;

        public static KillLogManager Instance
        {
            get
            {
                // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(KillLogManager)) as KillLogManager;
                }

                return _instance;
            }
        }

        [HideInInspector] public List<PlayerController_FSM> PlayerScript;
        private int ID;
        private string MyTag;
        public Sprite[] CharacterIamge;

        [SerializeField] GameObject KillLogBase;
        [SerializeField] GameObject MyKillLog;
        [SerializeField] GameObject MyDeadLog;
        [SerializeField] GameObject RightDeadLog;
        [SerializeField] GameObject LeftKillLog;
        [SerializeField] VerticalLayoutGroup KillLogBox;

        private void Start()
        {
            foreach(PlayerController_FSM player in PlayerScript)
            {
                if(player.IsMine)
                {
                    //���� Ŭ���̾�Ʈ�� ������ �����Ѵ�.
                    ID = player.ID;
                    MyTag = player.tag;
                }
            }
        }

        public void AddPlayer(GameObject obj)
        {
            PlayerScript.Add(obj.GetComponent<Junpyo.PlayerController_FSM>());
        }

        public void KillLogAdd(int WinID, int LossID)
        {
            PlayerController_FSM WinScript = null;
            PlayerController_FSM LossScript = null;

            GameObject KillLog = GameObject.Instantiate(KillLogBase, KillLogBox.transform);

            KillLogBase killLogBase = KillLog.GetComponent<KillLogBase>();

            foreach (PlayerController_FSM win in PlayerScript)
            {
                if (win.ID == WinID)
                {
                    WinScript = win;
                }
            }

            foreach (PlayerController_FSM loss in PlayerScript)
            {
                if (loss.ID == LossID)
                {
                    LossScript = loss;
                }
            }

            //LeftLog
            GameObject LeftLog = null;
            MyLog myLog = null;

            if (ID == WinID)
            {
                LeftLog = GameObject.Instantiate(MyKillLog);
                myLog = LeftLog.GetComponent<MyLog>();
            }
            else 
            {
                LeftLog = GameObject.Instantiate(LeftKillLog);
                myLog = LeftLog.GetComponent<MyLog>();
                //�츮���� �ƴҰ�� ���򺯰�
                if (!WinScript.CompareTag(MyTag))
                {
                    myLog.Nicname.color = new Color(1, 0.3f, 0, 1);
                    myLog.Rim.color = new Color(1, 0.3f, 0, 1);
                }
            }

            //���̽��� Left�� �ڽ����� ����
            LeftLog.transform.parent = killLogBase.LeftLog.transform;

            //LocalPos �ʱ�ȭ
            LeftLog.transform.localPosition = Vector3.zero;

            //LeftLog ���� �ʱ�ȭ
            myLog.Nicname.text = WinScript.Nicname;
            myLog.CharacterImage.sprite = CharacterIamge[(int)WinScript.CharacterName];

            //RightLog
            GameObject RightLog = null;
            //LeftLog
            if (ID == LossID)
            {
                RightLog = GameObject.Instantiate(MyDeadLog);
                myLog = RightLog.GetComponent<MyLog>();
            }
            else
            {
                RightLog = GameObject.Instantiate(RightDeadLog);
                myLog = RightLog.GetComponent<MyLog>();

                //�츮���� �ƴҰ�� ���򺯰�
                if (!LossScript.CompareTag(MyTag))
                {
                    myLog.Nicname.color = new Color(1, 0.3f, 0, 1);
                    myLog.Rim.color = new Color(1, 0.3f, 0, 1);
                }
            }

            //���̽��� Left�� �ڽ����� ����
            RightLog.transform.parent = killLogBase.RightLog.transform;

            //LocalPos �ʱ�ȭ
            RightLog.transform.localPosition = Vector3.zero;

            //LeftLog ���� �ʱ�ȭ
            Debug.Log(LossScript.Nicname);
            myLog.Nicname.text = LossScript.Nicname;
            myLog.CharacterImage.sprite = CharacterIamge[(int)LossScript.CharacterName];
        }
    }
}
