using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTemp : MonoBehaviour
{
    [Header("�ӷ�")]
    [SerializeField]
    float speed;
    public enum AXIS { X = 0, Y = 1, Z = 2}

    [Header("����")]
    [SerializeField]
    AXIS axis;

    private void FixedUpdate()
    {
        switch (axis)
        {
            case AXIS.X:
                this.transform.position += this.transform.right * speed * Time.fixedDeltaTime;
                break;
            case AXIS.Y:
                this.transform.position += this.transform.up * speed * Time.fixedDeltaTime;
                break;
            case AXIS.Z:
                this.transform.position += this.transform.forward * speed * Time.fixedDeltaTime;
                break;
        }
    }
}
