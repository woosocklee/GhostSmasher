using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    class GroundDownState : State_Base
    {
        public override void Setting() { StateType = PLAYERSTATE.GROUNDDOWN; }

        public override void StateEnter()
        {
            Owner_rigidbody.velocity = Vector3.zero;
            //�ִϸ��̼�
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationTrigger("GroundDown", Pv_ID);
                GameManager.Instance.PlayClip("GroundDown", Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.GROUNDDOWN, true, Pv_ID);
            }
            else
            {
                Owner_animator.SetTrigger("GroundDown");
                Owner_animator.Play("GroundDown", 0);
                OwnerIK.IKEvent(PLAYERSTATE.GROUNDDOWN, true);
            }
        }

        public override void Update()
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
            {
                //������
                state_Machine.ChangeState(PLAYERSTATE.ROLLING);
            }
            else if(Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("ü���� ��");
                //StandUpAttak
                state_Machine.ChangeState(PLAYERSTATE.STANDUP);
            }
        }

        public override void StateExit()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationResetTrigger("GroundDown", Pv_ID);
                GameManager.Instance.IKEvent(PLAYERSTATE.GROUNDDOWN, false, Pv_ID);
            }
            else
            {
                Owner_animator.ResetTrigger("GroundDown");
                OwnerIK.IKEvent(PLAYERSTATE.GROUNDDOWN, false);
            }
        }
    }
}
