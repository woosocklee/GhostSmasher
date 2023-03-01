using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Wooseok;
using UnityEngine.SceneManagement;

namespace Junpyo
{
    public class PlayerController_FSM : MonoBehaviourPunCallbacks , IPunObservable
    {
        //Player Componenet
        [HideInInspector] public Rigidbody playerRigidbody;
        [HideInInspector] public Animator playerAnimator;
        [HideInInspector] public Juhyung.Sound3DManager Sound3DManager;

        //FSM����� Ȱ���� StateMachine
        [HideInInspector] public State_Machine state_Machine;

        //�׶��� üũ
        public Transform GroundPos;
        private GroundCheack GroundScript;

        //ĳ������ Stat
        public PlayerInformation playerInformation;

        //Player Lookat
        [HideInInspector] public Vector3 PlayerLook;

        //Camera
        [SerializeField] GameObject CameraManager;
        [HideInInspector] public CameraTarget CameraTarget;
        [HideInInspector] public CameraManager CamManager;

        //Ik_Controller
        [HideInInspector] public Ik_Controller IK_Controller;

        //�÷��̾����UI
        [Header("PlayerConditionUI")]
        [SerializeField] public Canvas playerCanvas;
        [SerializeField] public HP_Value hp_Value;
        [SerializeField] public Image StaminaUI;
        [SerializeField] public Image HP_Bar;
        [Header("")]

        //���� HP�� �̹���
        [SerializeField] public Sprite Enemy_HP_Bar_Sprite;

        //��Ȱ UI
        [HideInInspector] public GameObject PlayerUI;
        [HideInInspector] public Transform RevivalUI;
        [HideInInspector] public Image RevivalWaitingImage;

        //AttackUI
        [HideInInspector] public SkillUI Skill_UI;
        [HideInInspector] public UltimateUI Ultimate_UI;

        //Dodge_Ui
        [HideInInspector] public Dodge_UI dodge_UI;

        //SpawnPos
        [HideInInspector] public Vector3 SpawnPos;

        //DeadFlyState�� ���󰡴� ������ ����
        [HideInInspector] public Vector3 DeadFlyDirection;

        [HideInInspector] public bool IsUpdate;

        //�����̻� ���ӽð�
        float FearTime = 5.0f;
        float CurceTime = 5.0f;

        //Netework �����ð� ����� ����
        [HideInInspector] public Vector3 networkPosition;
        [HideInInspector] public Quaternion networkRotation;
        [HideInInspector] public Vector3 networkVelocity;

        //�ڽ��� �ٶ󺸴� ������ ��Ÿ���� UI
        [SerializeField] public GameObject LookCheckFrefab;
        [HideInInspector] public GameObject LookCheck;

        [HideInInspector] public int ID;
        [HideInInspector] public bool IsMine = false;

        //ȸ�Ǳ�
        [HideInInspector] float DodgeCoolTime = 30.0f;
        //ȸ�Ǳ� ��밡�� ����
        [HideInInspector] public bool isDodge = true;

        //������ �ݶ��̴�
        [HideInInspector]  public EenemyCheck enemyCheck;

        //BounsHurtó�� ����
        [HideInInspector] public Vector3 BounsDir;
        [HideInInspector] public float BounsStrong;
        [HideInInspector] public bool AirborneRebound = true;
        [HideInInspector] public Vector3 AirborneDir;

        //������Ʈ �ٲٱ��
        public PLAYERSTATE WantState;

        //PlayerSkill
        [SerializeField] public PlayerSkill playerSkill;

        public CHARACTERNAME CharacterName;

        //ClinetNicName
        [SerializeField] public string Nicname;

        //�ڽ��� óġ�� ����� ĳ�����̸�
        [HideInInspector] public CHARACTERNAME EnemyCharacter;

        //�ڽ��� ���� �� �߶��� �� ��� Ȱ��ȭ ��Ŵ
        [HideInInspector] public bool fallDead;

        //True�� ��쿡 GroundDownState�� ���� �����ϴ�.
        [HideInInspector] public bool OnGround = false;

        //������
        [HideInInspector] public bool ItemTrigger;
        [HideInInspector] public bool GetBoom;
        public GameObject Boom;

        //���� �����̼ǿ� ������ ���󰡴� ����
        [HideInInspector] public Vector3 UseJumpDirection;

        [Header("�����̻�")]
        [SerializeField] GameObject HillEffect;
        [SerializeField] GameObject FearEffect;
        [SerializeField] GameObject CurceEffect;
        [SerializeField] GameObject StunEffect;
        [SerializeField] GameObject DamageEffect;
        [HideInInspector] GameObject CurEffect;
        [Header("")]

        [SerializeField] PLAYERSTATE CUrState;

        private Vector3 PreVelocity;

        [SerializeField] Sprite[] AttackSprite;

        [HideInInspector] Juhyung.AudioManager myAudio;

        protected void Awake()
        {
            //Character Setting
            playerRigidbody = GetComponent<Rigidbody>();
            playerAnimator = GetComponent<Animator>();
            IK_Controller = GetComponent<Ik_Controller>();
            GroundScript = GroundPos.GetComponent<GroundCheack>();
            playerInformation.Initialization(CharacterName);
            hp_Value.SetHP_Value((int)playerInformation.Cur_HP);
            ID = photonView.ViewID;
            myAudio = GetComponent<Juhyung.AudioManager>();
            IsUpdate = true;

            if (PhotonNetwork.IsConnected)
            {
                Nicname = photonView.Owner.NickName;
                IsUpdate = false;
            }

            IsMine = false;

            if (photonView.IsMine)
            {
                IsMine = true;

                //Camera����
                GameObject Camera = GameObject.Instantiate(CameraManager);
                Camera.name = "CameraManager";

                //ī�޶�Ÿ���� ����
                CameraTarget = GameObject.Find("CameraTarget").GetComponent<CameraTarget>();
                CameraTarget.TagetObj = transform;

                //LookChack����
                LookCheck = Instantiate<GameObject>(LookCheckFrefab, transform);

                enemyCheck = transform.GetChild(5).GetComponent<EenemyCheck>();

                //UI����
                PlayerUI = GameObject.Find("PlayerUI");
                RevivalUI = PlayerUI.transform.GetChild(3);
                RevivalWaitingImage = RevivalUI.GetChild(2).GetComponent<Image>();
                dodge_UI = PlayerUI.transform.GetChild(4).GetComponent<Dodge_UI>();

                //AttackSprite����
                PlayerUI.transform.GetChild(0).transform.GetChild(2).GetComponent<Image>().sprite = AttackSprite[0];

                Skill_UI = PlayerUI.transform.GetChild(1).GetComponent<SkillUI>();
                Skill_UI.transform.GetChild(2).GetComponent<Image>().sprite = AttackSprite[1];

                Ultimate_UI = PlayerUI.transform.GetChild(2).GetComponent<UltimateUI>();
                Ultimate_UI.MaxGage = playerInformation.UltGage_Max;
                Ultimate_UI.transform.GetChild(2).GetComponent<Image>().sprite = AttackSprite[2];

                //StateMachin �ʿ��� State�� �Ҵ�
                state_Machine = new State_Machine();
                state_Machine._Owner = transform;

                //�⺻ State�� StateMachine�� �Ҵ�
                state_Machine.StateAdd(new IdleState(), PLAYERSTATE.IDLE);
                state_Machine.ChangeState(PLAYERSTATE.IDLE);
                state_Machine.StateAdd(new RunState(), PLAYERSTATE.RUN);
                state_Machine.StateAdd(new BaseAttackState(), PLAYERSTATE.BASEATTACK);
                state_Machine.StateAdd(new JumpState(), PLAYERSTATE.JUMP);
                state_Machine.StateAdd(new LandState(), PLAYERSTATE.LAND);
                state_Machine.StateAdd(new HangState(), PLAYERSTATE.HANG);
                state_Machine.StateAdd(new HangAttackPrepare(), PLAYERSTATE.HANGATTACKPREPARE);
                state_Machine.StateAdd(new HangAttackState(), PLAYERSTATE.HANGATTACK);
                state_Machine.StateAdd(new HurtState(), PLAYERSTATE.HURT);
                state_Machine.StateAdd(new AirborneState(), PLAYERSTATE.AIRBORNE);
                state_Machine.StateAdd(new AirState(), PLAYERSTATE.AIR);
                state_Machine.StateAdd(new JumpAttackState(), PLAYERSTATE.JUMPATTACK);
                state_Machine.StateAdd(new GroundDownState(), PLAYERSTATE.GROUNDDOWN);
                state_Machine.StateAdd(new RollingState(), PLAYERSTATE.ROLLING);
                state_Machine.StateAdd(new DeadHighlightState(), PLAYERSTATE.DEADHIGHLIGHT);
                state_Machine.StateAdd(new DeadFlyState(), PLAYERSTATE.DEADFLY);
                state_Machine.StateAdd(new DeadState(), PLAYERSTATE.DEAD);
                state_Machine.StateAdd(new SmashState(), PLAYERSTATE.SMASH);
                state_Machine.StateAdd(new SkillState(), PLAYERSTATE.SKILL);
                state_Machine.StateAdd(new JumpSkillState(), PLAYERSTATE.JUMPSKILL);
                state_Machine.StateAdd(new DodgeState(), PLAYERSTATE.DODGE);
                state_Machine.StateAdd(new StunState(), PLAYERSTATE.STUN);
                state_Machine.StateAdd(new CurceState(), PLAYERSTATE.CURCE);
                state_Machine.StateAdd(new UltimateState(), PLAYERSTATE.ULTIMATE);
                state_Machine.StateAdd(new DropState(), PLAYERSTATE.DROP);
                state_Machine.StateAdd(new BounsHurtState(), PLAYERSTATE.BOUNSHURT);
                state_Machine.StateAdd(new StandUpState(), PLAYERSTATE.STANDUP);
                state_Machine.StateAdd(new StandUpAttackState(), PLAYERSTATE.STANDUPATTACK);
                state_Machine.StateAdd(new DodgeAirState(), PLAYERSTATE.DODGEAIR);
                state_Machine.StateAdd(new UseJumpStationState(), PLAYERSTATE.USE_JUMPSTATION);
                state_Machine.StateAdd(new GetItemState(), PLAYERSTATE.GETITEM);
                state_Machine.StateAdd(new ThrowState(), PLAYERSTATE.THROW);

                //ī�޶� ����
                CamManager = Camera.GetComponent<CameraManager>();
                CamManager.target = CameraTarget.transform;
                CamManager.transform.position = CamManager.transform.forward * -9.0f + transform.position;
            }

            gameObject.name = photonView.ViewID.ToString();
        }

        protected void Update()
        {
            if (photonView.IsMine)
            {
                if (IsUpdate)
                {
                    //���� �ڽ��� State�� Ȯ���ϴ� �뵵
                    CUrState = state_Machine._CurState;

                    //������ ��ȣ�ۿ�
                    if (ItemTrigger && 
                        Input.GetKeyDown(KeyCode.Z))
                    {
                        if (((state_Machine._CurState == PLAYERSTATE.IDLE) ||
                        (state_Machine._CurState == PLAYERSTATE.RUN)))
                        {
                            ItemTrigger = false;
                            Juhyung.ItemManager.Instance.s_PlayerNum = this;
                            state_Machine.ChangeState(PLAYERSTATE.GETITEM);
                        }
                    }

                    state_Machine.Update();

                    //ȸ�Ǳ�
                    if (Input.GetKeyDown(KeyCode.LeftShift) && 
                        (state_Machine._CurState != PLAYERSTATE.ULTIMATE) &&
                        (state_Machine._CurState != PLAYERSTATE.ULTIMATEPREPARE))
                    {
                        //�ൿ �ʱ�ȭ �� ��� ������ üũ����
                        ReSet();
                        StartCoroutine(DodgeDelay());

                        //������� ��ų ĵ��
                        if ((state_Machine._CurState == PLAYERSTATE.SKILL) && playerInformation.SkillCansle)
                        {
                            GameManager.Instance.Attack((int)SKILLTYPE.SKILL, false, ID);
                        }
                        else if (((state_Machine._CurState == PLAYERSTATE.JUMPSKILL) || (state_Machine._CurState == PLAYERSTATE.GANGNIM_JUMPSKILLSTATE))
                            && playerInformation.JumpSkillCansle)
                        {
                            GameManager.Instance.Attack((int)SKILLTYPE.CURSKILL, false, ID);
                        }
                        else if ((state_Machine._CurState == PLAYERSTATE.BASEATTACK))
                        {
                            GameManager.Instance.Attack((int)SKILLTYPE.CURSKILL, false, ID);
                        }

                        state_Machine.ChangeState(PLAYERSTATE.DODGE);
                    }
                }

                //StateȮ�� ������ ���� �ڵ�
                if (Input.GetKeyDown(KeyCode.P))
                {
                    WantStateChnage();
                }
            }
        }

        protected void FixedUpdate()
        {
            if (photonView.IsMine)
            {
                state_Machine.FixedUpdate();
            }
            else
            {
                playerRigidbody.velocity = networkVelocity;

                //position��Ʈ��ũ �����ð��� �����Ͽ� Pos����
                if ((playerRigidbody.position - networkPosition).magnitude < 0.5f)
                {
                    playerRigidbody.position = Vector3.Lerp(GetComponent<Rigidbody>().position, networkPosition, 10 * Time.fixedDeltaTime);
                }
                else
                {
                    playerRigidbody.position = networkPosition;
                }

                playerRigidbody.rotation = networkRotation;
            }

        }

        //�Ŵ޸��� �ý���
        public void Hang(Vector3 HangPosition, Vector3 Angel)
        {
            if (photonView.IsMine)
            {
                transform.position = HangPosition;
                transform.rotation = Quaternion.Euler(Angel);
                state_Machine.ChangeState(PLAYERSTATE.HANG);          
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (photonView.IsMine)
            {
                //������ �� ���
                if (other.transform.CompareTag("JumpStation"))
                {
                    UseJumpDirection = other.transform.up;
                    state_Machine.ChangeState(PLAYERSTATE.USE_JUMPSTATION);
                }
                //�������� ���
                else if ((other.gameObject.layer == LayerMask.NameToLayer("Item")))
                {
                    ItemTrigger = true;
                    Juhyung.ItemManager.Instance.s_Getitem = other.name;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (photonView.IsMine)
            {
                //�������ݶ��̴��ϰ� ��������
                if ((other.gameObject.layer == LayerMask.NameToLayer("Item")))
                {
                    ItemTrigger = false;
                    Juhyung.ItemManager.Instance.s_Getitem = null;
                }
            }
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (photonView.IsMine)
            {
                //���� ��� �ִ� ���¿��� ��� �϶� 
                if (collision.transform.tag == "Ground" || collision.transform.CompareTag("Stairs"))
                {
                    //Ground�� y������ �ڽ��� y���� ������ ��.
                    if ((collision.collider.bounds.max.y - 0.1f) <= transform.position.y)
                    {
                        //���߿� �ִ� State���϶��� �˻�
                        if ((state_Machine._CurState == PLAYERSTATE.JUMPATTACK) ||
                            (state_Machine._CurState == PLAYERSTATE.AIR) ||
                            (state_Machine._CurState == PLAYERSTATE.HURT) ||
                            (state_Machine._CurState == PLAYERSTATE.JUMP) ||
                            (state_Machine._CurState == PLAYERSTATE.DODGEAIR) ||
                            (state_Machine._CurState == PLAYERSTATE.USE_JUMPSTATION) ||
                            (state_Machine._CurState == PLAYERSTATE.THROW) ||
                            (state_Machine._CurState == PLAYERSTATE.TRUELOVE_JUMPATTACKSUCCESS))
                        {
                            transform.position = new Vector3(transform.position.x, collision.collider.bounds.max.y + 0.1f, transform.position.z);
                            state_Machine.ChangeState(PLAYERSTATE.LAND);
                        }

                        //Airborn
                        if ((state_Machine._CurState == PLAYERSTATE.AIRBORNE))
                        {
                            if (OnGround)
                            {
                                state_Machine.ChangeState(PLAYERSTATE.GROUNDDOWN);
                            }
                            else
                            {
                                AirborneRebound = false;
                                playerRigidbody.velocity = new Vector3(-AirborneDir.x, AirborneDir.y, -AirborneDir.z);
                            }
                        }
                    }
                }

                //��ܰ��� �浹ó�� Check
                if (collision.transform.CompareTag("Stairs") &&
                    (state_Machine._CurState == PLAYERSTATE.RUN))
                {
                    if ((collision.transform.position.y + 0.4f) > transform.position.y)
                    {
                        transform.position += new Vector3(0, 0.3f, 0);
                    }
                }
            }
        }

        public void OnCollisionStay(Collision collision)
        {
            if (photonView.IsMine)
            {
                //
                if (collision.transform.CompareTag("Ground") || collision.transform.CompareTag("Stairs"))
                {
                    //ȸ�Ǳ� ����ϰ� ���� ���� ��������� ó��
                    if((state_Machine._CurState == PLAYERSTATE.DODGEAIR))
                    {
                        transform.position = new Vector3(transform.position.x, collision.collider.bounds.max.y + 0.1f, transform.position.z);
                        state_Machine.ChangeState(PLAYERSTATE.LAND);
                    }

                    if (OnGround)
                    {
                        //Ground�� y������ �ڽ��� y���� ���� ��� pos�� ����ؼ� ����.
                        if (collision.collider.bounds.max.y > transform.position.y)
                        {
                            if ((state_Machine._CurState == PLAYERSTATE.JUMP) ||
                                (state_Machine._CurState == PLAYERSTATE.DODGEAIR))
                            {
                                transform.position += new Vector3(0, -0.03f, 0);
                            }
                        }
                        else if(state_Machine._CurState == PLAYERSTATE.AIRBORNE)
                        {
                            state_Machine.ChangeState(PLAYERSTATE.GROUNDDOWN);
                        }
                    }
                    //���󰡴� ��Ȳ�� �ٷ� �ٸ� ���� �浹�ϸ� �ε��� ������ �ݴ�������� ƨ��
                    else if ((state_Machine._CurState == PLAYERSTATE.AIRBORNE) && AirborneRebound)
                    {
                        AirborneRebound = false;
                        playerRigidbody.velocity = new Vector3(-AirborneDir.x, AirborneDir.y, -AirborneDir.z);
                    }

                    //��ܰ��� �浹ó�� Check
                    if (collision.transform.CompareTag("Stairs") &&
                        (state_Machine._CurState == PLAYERSTATE.RUN))
                    {
                        if ((collision.transform.position.y + 0.4f) > transform.position.y)
                        {
                            transform.position += new Vector3(0, 0.3f, 0);
                        }
                    }
                }
            }
        }

        public void Hurt(Vector3 direction, float debuff, ATTACKTYPE type, float damage)
        {
            if (photonView.IsMine)
            {
                //���� �Ƹ��� ��� �������� �ް� �ǰ��� ���� �ʴ´�.
                //�⺻���·� �ʱ�ȭ
                ReSet();

                //HP�� 0�Ͻ� �״� ����
                if (playerInformation.Cur_HP <= 0)
                {
                    gameObject.layer = LayerMask.NameToLayer("Dead");
                    DeadFlyDirection = direction;
                    state_Machine.ChangeState(PLAYERSTATE.DEADHIGHLIGHT);
                }
                else
                {
                    if (damage > 0)
                    {
                        //Ÿ�ݰ� ����
                        EffectManager.Instance.EffectInst(EFFECT.DAMAGE, transform.position, (int)damage);
                        CamManager.CameraShaking();

                        //���۾Ƹ��� ��� ������ó���� �ޱ� ������ Ż��
                        if (gameObject.layer == LayerMask.NameToLayer("SuperArmer"))
                        {
                            return;
                        }

                        playerRigidbody.velocity = Vector3.zero;

                        //��ų�� ������̿����� ��ų����� ������Ŵ
                        if ((state_Machine._CurState == PLAYERSTATE.SKILL) && playerInformation.SkillCansle)
                        {
                            GameManager.Instance.Attack((int)SKILLTYPE.SKILL, false, ID);
                        }
                        else if(((state_Machine._CurState == PLAYERSTATE.JUMPSKILL) || (state_Machine._CurState == PLAYERSTATE.GANGNIM_JUMPSKILLSTATE))
                            && playerInformation.JumpSkillCansle)
                        {
                            GameManager.Instance.Attack((int)SKILLTYPE.CURSKILL, false, ID);
                        }
                        else if((state_Machine._CurState == PLAYERSTATE.BASEATTACK))
                        {
                            GameManager.Instance.Attack((int)SKILLTYPE.CURSKILL, false, ID);
                        }

                        //Ult_Gage Puse
                        if (!playerInformation.UltimateOn)
                        {
                            UtimateGageUp(5);
                        }

                        //������ �浹ó���� ����� �ϱ����� ���Ƿ� ���� �ø�
                        playerRigidbody.MovePosition(transform.position + new Vector3(0, 0.2f, 0));
                    }

                    //Ataacktype���� �޴� ȿ���� �ٸ��� ����
                    switch (type)
                    {
                        case ATTACKTYPE.LIGHTATTACK:
                            {
                                playerRigidbody.MovePosition(transform.position + new Vector3(0, 0.2f, 0));
                                playerRigidbody.velocity = (direction * 2.0f) + new Vector3(0, 3.0f, 0);
                                state_Machine.ChangeState(PLAYERSTATE.HURT);
                            }
                            break;

                        case ATTACKTYPE.MIDDLEATTACK:
                            {
                                playerRigidbody.MovePosition(transform.position + new Vector3(0, 0.2f, 0));
                                playerRigidbody.velocity = (direction * 10.0f) + new Vector3(0, 6.0f, 0);
                                state_Machine.ChangeState(PLAYERSTATE.AIRBORNE);
                            }
                            break;

                        case ATTACKTYPE.HEAVYATTACK:
                            { 
                            playerRigidbody.MovePosition(transform.position + new Vector3(0, 0.2f, 0));
                            playerRigidbody.velocity = (direction * 15.0f) + new Vector3(0, 7.0f, 0);
                            state_Machine.ChangeState(PLAYERSTATE.AIRBORNE);
                            }
                            break;

                        case ATTACKTYPE.STUN:
                            {
                                playerRigidbody.MovePosition(transform.position + new Vector3(0, 0.2f, 0));
                                playerRigidbody.velocity = (direction * 2.0f) + new Vector3(0, 3.0f, 0);
                                state_Machine.ChangeState(PLAYERSTATE.STUN);
                            }
                            break;

                        case ATTACKTYPE.CURSE:
                            {
                                if (playerInformation.Curce)
                                {
                                    StopCoroutine(CurceDuration());
                                    playerInformation.CurRunSpeed = playerInformation.RunSpeed;
                                    playerInformation.CurJumpDistance = playerInformation.JumpDistance;
                                    playerInformation.Curce = false;
                                }

                                StartCoroutine(CurceDuration());
                            }
                            break;

                        case ATTACKTYPE.BOUNCE:
                            {
                                BounsDir = direction;
                                BounsDir.y = -debuff * 3;
                                BounsStrong = debuff;
                                playerRigidbody.velocity = (BounsDir * debuff);
                                state_Machine.ChangeState(PLAYERSTATE.BOUNSHURT);
                            }
                            break;

                        case ATTACKTYPE.STIFF:
                            {
                                playerRigidbody.velocity = new Vector3(0, 0.5f, 0);
                                state_Machine.ChangeState(PLAYERSTATE.HURT);
                            }
                            break;

                        case ATTACKTYPE.FEAR:
                            {
                                if (playerInformation.Fear)
                                {
                                    StopCoroutine(FearDuration());
                                    playerInformation.Fear = false;
                                }

                                StartCoroutine(FearDuration());

                                playerRigidbody.MovePosition(transform.position + new Vector3(0, 0.2f, 0));
                                playerRigidbody.velocity = (direction * 15.0f) + new Vector3(0, 7.0f, 0);
                                state_Machine.ChangeState(PLAYERSTATE.AIRBORNE);
                            }
                            break;

                        case ATTACKTYPE.HEAL:
                            {
                                StartCoroutine(HellDuration());
                            }
                            break;
                    }
                }
            }
        }

        public void CameraChange()
        {
            if (photonView.IsMine)
            {
                CamManager.CameraChange();
            }
        }     

        //Ʈ�緯�꿡���� ������� �Լ��� ��� �ذ����� ����ؾ��Ѵ�.
        public void ChangeState(PLAYERSTATE type)
        {
            if (photonView.IsMine)
            {
                state_Machine.ChangeState(type);
            }
        }

        //������ óġ�� �� ȣ��
        public void SmashTrigger()
        {
            if (photonView.IsMine)
            {
                //ĳ���Ͱ� ���߱��� �ڽ��� Velocity�� ����
                PreVelocity = playerRigidbody.velocity;

                //Player �ǰݴ����� �ʰ� ����
                gameObject.layer = LayerMask.NameToLayer("Smash");
                playerRigidbody.isKinematic = true;

                //������Ʈ ����
                IsUpdate = false;

                //ī�޶� Ÿ�� �ڽ����� ����
                CameraTarget.transform.position = transform.position;

                //Smashī�޶�� ��ȯ�ϸ鼭 ȭ�� ����
                CameraChange();

                //���ϸ��̼� ����
                GameManager.Instance.AnimationPlay(photonView.ViewID,false);

                //Smash�ð��� ��� �����ð� ���� �������� �ڷ�ƾ����
                StopCoroutine("SetInitialization");
                StartCoroutine(SetInitialization());
            }
        }

        IEnumerator SetInitialization()
        {
            yield return new WaitForSeconds(0.5f);

            //�ִϸ��̼� ���� ��Ȱ��ȭ 
            GameManager.Instance.AnimationPlay(photonView.ViewID, true);

            //�����ۿ� Ȱ��ȭ
            playerRigidbody.isKinematic = false;

            //ī�޶� �ٽ� �ǵ�����
            CameraChange();

            //Layer�ٽ� �ǵ�����
            gameObject.layer = LayerMask.NameToLayer("Player");

            //������Ʈ �簡��
            IsUpdate = true;

            //������ Velocity���� �����Ͽ� �ڿ������� ����
            playerRigidbody.velocity = PreVelocity;
        }

        //����
        IEnumerator FearDuration()
        {
            GameManager.Instance.ConditionEffectOn(ATTACKTYPE.FEAR, true, ID);
            playerInformation.Fear = true;
            yield return new WaitForSeconds(FearTime);

            GameManager.Instance.ConditionEffectOn(ATTACKTYPE.FEAR, false, ID);
            playerInformation.Fear = false;
        }

        //����
        IEnumerator CurceDuration()
        {
            playerInformation.CurRunSpeed *= 0.5f;
            playerInformation.CurJumpDistance *= 0.5f;
            playerInformation.Curce = true;
            GameManager.Instance.ConditionEffectOn(ATTACKTYPE.CURSE, true, ID);

            yield return new WaitForSeconds(CurceTime);

            playerInformation.CurRunSpeed = playerInformation.RunSpeed;
            playerInformation.CurJumpDistance = playerInformation.JumpDistance;
            playerInformation.Curce = false;
            GameManager.Instance.ConditionEffectOn(ATTACKTYPE.CURSE, false, ID);
        }

        //ȸ��
        IEnumerator HellDuration()
        {
            GameManager.Instance.ConditionEffectOn(ATTACKTYPE.HEAL, true, ID);

            yield return new WaitForSeconds(0.45f);

            GameManager.Instance.ConditionEffectOn(ATTACKTYPE.HEAL, false, ID);
        }

        //����ȭ�� �ʿ��� ���׵��� IsMine��ü���� �ٸ� Ŭ���̾�Ʈ�� �ִ� ��ü�鿡�� ������ �����Ѵ�.
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //��ġ����ȭ
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(GetComponent<Rigidbody>().velocity);

                //���¹̳�
                stream.SendNext(StaminaUI.enabled);
                stream.SendNext(StaminaUI.fillAmount);
                stream.SendNext(playerInformation.Curce);
                stream.SendNext(playerInformation.UltimateOn);
            }
            else
            {
                networkPosition = (Vector3)stream.ReceiveNext();
                networkRotation = (Quaternion)stream.ReceiveNext();
                networkVelocity = (Vector3)stream.ReceiveNext();

                //���¹̳�
                StaminaUI.enabled = (bool)stream.ReceiveNext();
                StaminaUI.fillAmount = Mathf.MoveTowards((float)stream.ReceiveNext(), StaminaUI.fillAmount, Time.deltaTime);
                playerInformation.Curce = (bool)stream.ReceiveNext();
                playerInformation.UltimateOn = (bool)stream.ReceiveNext();
            }
        }
        public void Dead(bool b)
        {
            //óġ���ϸ� ĳ���� �� �� UI���� ��Ȱ��ȭ
            //�ݴ�� ��Ƴ��� ��� Ȱ��ȭ
            transform.GetChild(0).gameObject.SetActive(b);
            playerCanvas.enabled = b;
            GroundPos.gameObject.SetActive(b);
            playerRigidbody.useGravity = false;

            if (photonView.IsMine)
            {
                LookCheck.SetActive(false);
            }
        }

        public void Fall()
        {
            if(photonView.IsMine)
            {
                //�߶��ϴ� ����Ʈ ���� �� DeadState����
                GameManager.Instance.Dead(false, photonView.ViewID);
                playerInformation.Cur_HP = 0;
                GameManager.Instance.HP(playerInformation.Cur_HP, ID);
                fallDead = true;
                state_Machine.ChangeState(PLAYERSTATE.DEAD);
            }
        }

        IEnumerator DodgeDelay()
        {
            dodge_UI.UseDodge();
            isDodge = false;

            yield return new WaitForSeconds(DodgeCoolTime);

            isDodge = true;
        }

        //�� ����
        public void TemmDivision(string tag)
        {
            if (!CompareTag(tag))
            {
                ChangeLayersRecursively(transform.GetChild(0), "Red");

                GroundScript.ColorChange();

                HP_Bar.sprite = Enemy_HP_Bar_Sprite;
            }
        }

        //�ڽ� ���̾���� �ٲٴ� �Լ�
        public static void ChangeLayersRecursively(Transform trans, string name)
        {
            trans.gameObject.layer = LayerMask.NameToLayer(name);

            foreach (Transform child in trans)
            {
                ChangeLayersRecursively(child, name);
            }
        }

        public void UseSkill()
        {
            Skill_UI.UseSkill(playerInformation.Skill_Time);
            playerInformation.SkillOn = false;      
            StartCoroutine(SkillCootime());
        }

        IEnumerator SkillCootime()
        {
            yield return new WaitForSeconds(playerInformation.Skill_Time);
            playerInformation.SkillOn = true;
        }

        public void UseUtimate()
        {
            playerInformation.Cur_UltGage = 0;
            playerInformation.UltimateOn = false;
            Ultimate_UI.UseUtimate();
        }

        public void UtimateGageUp(int gage)
        {
            if (IsMine)
            {
                if (!playerInformation.UltimateOn)
                {
                    if (playerInformation.Cur_UltGage + gage >= playerInformation.UltGage_Max)
                    {
                        playerInformation.Cur_UltGage = 0;
                        playerInformation.UltimateOn = true;
                    }
                    else
                    {
                        playerInformation.Cur_UltGage += gage;
                    }

                    Ultimate_UI.UtimateGageUp(gage);
                }
            }
        }

        //�����ڿ� �Լ� ������ State�� ��� ��ȯ
        public void WantStateChnage()
        {
            state_Machine.ChangeState(WantState);
        }
        
        public void LineSet()
        {
            GroundScript.LineSet();
        }

        //�ʱ���·� �ʱ�ȭ
        void ReSet()
        {
            playerAnimator.StopPlayback();
            playerRigidbody.isKinematic = false;
            playerRigidbody.useGravity = true;
        }

        //�ڽ��� ���¿� ���� Ȱ��ȭ �Ǵ� Effectȣ�� �Լ�
        public void ConditionEffectOn(ATTACKTYPE condition, bool on)
        {
            if (on && (CurEffect != null))
            {
                CurEffect.SetActive(false);
            }

            switch (condition)
            {
                case ATTACKTYPE.CURSE:
                    CurceEffect.SetActive(on);
                    CurEffect = CurceEffect;
                    break;
                case ATTACKTYPE.STUN:
                    StunEffect.SetActive(on);
                    CurEffect = StunEffect;
                    break;
                case ATTACKTYPE.FEAR:
                    FearEffect.SetActive(on);
                    CurEffect = FearEffect;
                    break;
                case ATTACKTYPE.HEAL:
                    HillEffect.SetActive(on);
                    CurEffect = HillEffect;
                    break;
            }
        }

        //������ Ÿ�̹��� ��ź�� ���ִ� �뵵�� ����
        public void ThrowBomb()
        {
            Boom.SetActive(false);
            GetBoom = false;
        }

        public void Heal(int h)
        {
            if (IsMine)
            {
                StartCoroutine(HellDuration());

                //Hpȸ��
                if (h == 0)
                {
                    if (playerInformation.Cur_HP + 1500.0f >= playerInformation.HP_Max)
                    {
                        playerInformation.Cur_HP = playerInformation.HP_Max;
                        HP_Bar.fillAmount = playerInformation.Cur_HP / playerInformation.HP_Max;
                        hp_Value.SetHP_Value((int)playerInformation.HP_Max);
                    }
                    else
                    {
                        playerInformation.Cur_HP += 1500.0f;
                        HP_Bar.fillAmount = playerInformation.Cur_HP / playerInformation.HP_Max;
                        hp_Value.SetHP_Value((int)playerInformation.Cur_HP);
                    }

                    GameManager.Instance.HP(playerInformation.Cur_HP, ID);
                }
                //UltGage ȸ��
                else
                {
                    UtimateGageUp(40);
                }

                ItemTrigger = false;
            }
        }

        public void PlayHitSound(int number)
        {
            if(number != -1)
            {
                myAudio.Play(Juhyung.SoundManager.Instance.HitSound[number]);
            }
        }

    }
}

