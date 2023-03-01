using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace Junpyo
{
    public class PlayerParametors : MonoBehaviourPun, IPunObservable
    {
        public Canvas UI_Canvas;
        [SerializeField] GroundCheack GroundScript;
        [SerializeField] LineRenderer Line;
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //���̾� ����ȭ
                stream.SendNext(gameObject.layer);

                //Ui��ġ ����ȭ
                stream.SendNext(UI_Canvas.transform.localPosition);

                //Line ����ȭ
                stream.SendNext(Line.enabled);
            }
            else
            {
                //���̾� ����ȭ
                gameObject.layer = (int)stream.ReceiveNext();

                //���̾� ����ȭ
                UI_Canvas.transform.localPosition = (Vector3)stream.ReceiveNext();

                //Line ����ȭ
                Line.enabled = (bool)stream.ReceiveNext();
            }
        }
    }
}
