using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class UnitiChan : PlayerController_FSM
    {

        void Start()
        {       
            playerInformation.Initialization(GameManager.Instance._CharacterName);

            //��ų �־��ֱ�

        }
    }
}
