using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class CoroutineHelper : MonoBehaviour
    {
        private static MonoBehaviour monoInstance;

        //Awake�޼��尡 ȣ�� �� �Ŀ� �ѹ��� ����
        [RuntimeInitializeOnLoadMethod]
        private static void Initializer()
        {
            monoInstance = new GameObject($"[{nameof(CoroutineHelper)}]").AddComponent<CoroutineHelper>();
            DontDestroyOnLoad(monoInstance.gameObject);
        }

        //���� �ڷ�ƾ�� ������ ���� public new Static�� ���
        public new static Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return monoInstance.StartCoroutine(coroutine);
        }

        public new static void StopCoroutine(Coroutine coroutine)
        {
            monoInstance.StopCoroutine(coroutine);
        }
    }
}