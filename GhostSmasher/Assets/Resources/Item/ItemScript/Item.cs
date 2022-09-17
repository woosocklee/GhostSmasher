using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    //해당 플레이어는 임시 클래스. 다른 사람이 만든 "진짜" 플레이어가 오기 전까지 땜빵으로 사용될 예정.
    public class Player : MonoBehaviour
    {
        int hp;
        public int HP
        {
            get
            {
                return hp;
            }
            set
            {
                this.hp = value;
            }
        }

    
    }


    public abstract class Item : MonoBehaviour
    {
        // Start is called before the first frame update
    
        public abstract void Item_effect(Player player);
    
    
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {

        }
    }



}

