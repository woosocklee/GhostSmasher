using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public class TestManager : MonoBehaviourPunCallbacks
    {
        private static TestManager _instance;

        public static TestManager Instance
        {
            get
            {
                // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(TestManager)) as TestManager;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        [SerializeField] private List<Sprite> i_Num;                                    // 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 sprite
        [SerializeField] public List<GameObject> PlayerPref;                            // PlayerPrefabs
        [SerializeField] public List<GameObject> spownPoint = new List<GameObject>();   // Player SpawnPoint

        [SerializeField] private float StartDelayTime = 0.0f;                           // Occupation Trigger On before DelayTime
        
        [SerializeField] private GameObject OccupationTrigger;                          // Occupation Range

        private GameObject go;                                                          // Player
        private int MyIndex;                                                            // My SpawnPos Idx

        private float GameTime = 180.0f;

        private void Awake()
        {
            Singleton();
            CreatePlayer();
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

        private void CreatePlayer()
        {
            if (PhotonNetwork.InRoom)
            {
                int actorNum = PhotonNetwork.LocalPlayer.ActorNumber;
                Player[] sortedPlayers = PhotonNetwork.PlayerList;

                for (int i = 0; i < sortedPlayers.Length; ++i)
                {
                    if (sortedPlayers[i].ActorNumber == actorNum)
                    {
                        GameManager.Instance.i_PlayerID = i;
                        go = PhotonNetwork.Instantiate
                            (
                                PlayerPref[(int)GameManager.Instance.e_SetChar].name, 
                                spownPoint[GameManager.Instance.i_PlayerID].transform.position, 
                                spownPoint[GameManager.Instance.i_PlayerID].transform.rotation
                            );

                        break;
                    }
                }

                if (GameManager.Instance.i_PlayerID % 2 == 0)
                {
                    photonView.RPC("SetTagRPC", RpcTarget.All, go.GetComponent<PhotonView>().ViewID, "Blue");
                }
                else
                {
                    photonView.RPC("SetTagRPC", RpcTarget.All, go.GetComponent<PhotonView>().ViewID, "Red");
                }
            }
        }

        // RPC Funtion
        [PunRPC]
        void SetTagRPC(int playerIndex, string tag)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject obj in players)
            {
                if (obj.GetComponent<PhotonView>().ViewID == playerIndex)
                {
                    obj.gameObject.tag = tag;
                    obj.GetComponent<Junpyo.PlayerController_FSM>().SpawnPos = spownPoint[MyIndex].transform.position;
                    obj.GetComponent<Junpyo.PlayerController_FSM>().SpawnPos = spownPoint[GameManager.Instance.i_PlayerID].transform.position;

                    GameManager.Instance.AddPlayer(obj);
                }
            }
        }
    }
}
