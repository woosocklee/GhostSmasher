using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    //�ش� �÷��̾�� �ӽ� Ŭ����. �ٸ� ����� ���� "��¥" �÷��̾ ���� ������ �������� ���� ����.
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

