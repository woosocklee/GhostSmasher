using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Junpyo;

namespace Wooseok
{ 

    public class Item_Bomb : FieldItem
    {
        Skill Bomb;
        Skill Ex_Skill;


        public override void Effect(PlayerController_FSM player)
        {
            //�÷��̾��� �÷��̾� ��ų���� ��Ÿ�� Ex_Skill�� �־�α�. �׸��� ��ų ��ü�ϱ�.
            //player.playerInformation.playerSkill.
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
    }
}