using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wooseok
{
    public class FieldItem : MonoBehaviour
    {
        // Start is called before the first frame update
    
        [SerializeField]
        Item itemEffect;
    
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
    
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                itemEffect.Item_effect(collision.gameObject.GetComponent<Player>());
                Destroy(this.gameObject);
            }
        }
    
    
    
    }


}
