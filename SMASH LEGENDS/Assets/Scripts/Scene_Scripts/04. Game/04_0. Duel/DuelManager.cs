using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Juhyung
{
    public class DuelManager : Junpyo.GamLogicManager
    {
        private static DuelManager _instance;

        public static DuelManager Instance
        {
            get
            {
                // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(DuelManager)) as DuelManager;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        public int RedScore;
        public int BlueScore;

        private void Awake()
        {
            Singleton();
            CreatePlayer();
        }

        private void Update()
        {
            if (GameStart)
            {
                GameTimer();
            }

            if(RedScore == 3 || BlueScore == 3)
            {
                GameSet();
            }
        }

        private void Singleton()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected override void GameTimer()
        {
            if (GameTime <= 0.0f)
            {
                return; // GameEnd
            }

            GameTime -= Time.deltaTime;

            int i_Min = (int)GameTime / 60;
            int i_Tensec = (int)(GameTime - (i_Min * 60)) / 10;
            int i_Onesec = (int)(GameTime - (i_Min * 60) - (i_Tensec * 10));

            Min.sprite = i_Num[i_Min];
            TenSec.sprite = i_Num[i_Tensec];
            OneSec.sprite = i_Num[i_Onesec];
        }

        public override void GameSet()
        {
            PhotonNetwork.AutomaticallySyncScene = false;

            if (go.GetPhotonView().IsMine)
            {
                if (RedScore == 3)
                {
                    if (go.tag.Equals("Red"))
                    {
                        PhotonNetwork.LoadLevel("05_0. TeamVictoryScene");
                    }
                    else if (go.tag.Equals("Blue"))
                    {
                        PhotonNetwork.LoadLevel("05_1. TeamLoseScene");
                    }
                }
                else if (BlueScore == 3)
                {
                    if (go.tag.Equals("Blue"))
                    {
                        PhotonNetwork.LoadLevel("05_0. TeamVictoryScene");
                    }
                    else if (go.tag.Equals("Red"))
                    {
                        PhotonNetwork.LoadLevel("05_1. TeamLoseScene");
                    }
                }
            }
        }
    }
}
