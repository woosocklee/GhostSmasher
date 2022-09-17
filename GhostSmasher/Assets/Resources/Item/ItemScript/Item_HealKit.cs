using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    public class Item_HealKit : Item
    {
    
        [SerializeField]
        int hp;
        public override void Item_effect(Player player)
        {
            player.HP += hp;
        }
    }

}

