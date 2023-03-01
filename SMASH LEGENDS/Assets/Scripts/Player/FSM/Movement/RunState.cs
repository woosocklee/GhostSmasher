using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class RunState : State_Base
    {
        //�밢�� �̵�����
        [SerializeField] private float SlopeTime = 0.05f;
        [SerializeField] private float SlopeCheckTimer = 0.0f;
        private bool isSlope = false;
        private Vector3 SlopeDirecion;

        public override void Setting() { StateType = PLAYERSTATE.RUN; }

        public override void StateEnter()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationFloat("Speed", 5, Pv_ID);
            }
            else
            {
                Owner_animator.SetFloat("Speed", 5);
            }

            MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            Owner.LookAt(Owner.position + MoveDirection);
        }

        public override void Update()
        {
            //�ӵ��� 0�ϰ�� Idle���·� ���ư�
            MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            //�밢�� �����϶�
            if (MoveDirection.x != 0 && MoveDirection.z != 0)
            {
                SlopeDirecion = MoveDirection;
                isSlope = true;
            }
            //�밢�� ������ �ƴҶ�
            else if (Mathf.Abs(MoveDirection.z + MoveDirection.x) == 1)
            {
                if (isSlope)
                {
                    //�����ð��� �ڳ����������� �밢�������� �����Ѵ�.
                    SlopeCheckTimer += Time.deltaTime;
                    if (SlopeTime <= SlopeCheckTimer)
                    {
                        isSlope = false;
                        SlopeCheckTimer = 0.0f;
                    }
                }
                else
                    SlopeCheckTimer = 0.0f;
            }
            else
            {
                //Ű���� �밢�������� �ƴҽ� SlopeDirecion�ʱ�ȭ
                SlopeDirecion = Vector3.zero;
            }

            //�� �̵������̶� �ٸ��� ������ȭ �� Idle�����϶��� ������ �������� Idle���� ����
            if (isSlope)
            {
                MoveDirection = SlopeDirecion;
            }

            //Player�� ���� ����
            if (Owner_Look != MoveDirection)
            {
                Owner_Look = MoveDirection;
                Owner.LookAt(Owner.position + Owner_Look);
            }

            //�̵���Ű���
            Owner_rigidbody.velocity = new Vector3(MoveDirection.x * Owner_Script.playerInformation.CurRunSpeed,
                 Owner_rigidbody.velocity.y,
                MoveDirection.z * Owner_Script.playerInformation.CurRunSpeed);

            //Ű���� ���� ������Ʈ ��ȯ

            //���������� �� �ൿ�Ҵ�
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (!Owner_Script.playerInformation.Fear)
                {
                    if (!Owner_Script.GetBoom)
                    {
                        Owner_rigidbody.velocity = Vector3.zero;
                        state_Machine.ChangeState(PLAYERSTATE.BASEATTACK);
                    }
                    else
                    {
                        state_Machine.ChangeState(PLAYERSTATE.THROW);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.X) && !Owner_Script.playerInformation.Fear)
            {
                if (!Owner_Script.GetBoom &&
                    Owner_Script.playerInformation.SkillOn)
                {
                    state_Machine.ChangeState(PLAYERSTATE.SKILL);
                }
                else if (Owner_Script.GetBoom)
                {
                    state_Machine.ChangeState(PLAYERSTATE.THROW);
                }

            }
            else if (Input.GetKeyDown(KeyCode.C) && !Owner_Script.playerInformation.Fear)
            {
                if (!Owner_Script.GetBoom &&
                    Owner_Script.playerInformation.UltimateOn)
                {
                    if (Owner_Script.playerInformation.UtimatePrepare)
                    {
                        state_Machine.ChangeState(PLAYERSTATE.ULTIMATEPREPARE);
                    }
                    else
                    {
                        state_Machine.ChangeState(PLAYERSTATE.ULTIMATE);
                    }
                }
                else if(Owner_Script.GetBoom)
                {
                    state_Machine.ChangeState(PLAYERSTATE.THROW);
                }
            }
            //�ӵ��� 0�ϰ�� Idle���·� ���ư�
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!Owner_Script.playerInformation.Fear)
                {
                    state_Machine.ChangeState(PLAYERSTATE.JUMP);
                }
            }
            else if (MoveDirection.magnitude == 0)
            {
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
            }
        }

        public override void FixedUpdate()
        {
            //Player��ġ
            //Owner_rigidbody.velocity = MoveDirection * Owner_Script.playerInformation.CurRunSpeed;
            //Owner_rigidbody.velocity = Owner.forward * Owner_Script.playerInformation.CurRunSpeed;
            //Owner_rigidbody.MovePosition(Owner.position + Owner.forward * Owner_Script.playerInformation.CurRunSpeed * Time.deltaTime);
            // Owner_rigidbody.velocity = new Vector3(MoveDirection.x * Owner_Script.playerInformation.CurRunSpeed,0, MoveDirection.z * Owner_Script.playerInformation.CurRunSpeed);
        }


        public override void StateExit()
        {
            //���ǵ� ���� ���� 
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationFloat("Speed", 0, Pv_ID);
            }
            else
            {
                Owner_animator.SetFloat("Speed", 0);
            }

            //Owner_rigidbody.velocity = Vector3.zero;
        }


    }
}
