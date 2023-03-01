using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    public class TeamChecker : MonoBehaviour
    {
        // �̱��� ������ ����ϱ� ���� �ν��Ͻ� ����
        private static TeamChecker _instance;
        // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
        public static TeamChecker Instance
        {
            get
            {
                // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(TeamChecker)) as TeamChecker;

                    if (_instance == null)
                        Debug.Log("no Singleton teamchecker");
                }
                return _instance;
            }
        }
        public bool TeamCheck(string team1, string team2)
        {
            if ((team1 == "Blue" && team2 == "Red") || (team1 == "Red" && team2 == "Blue"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}