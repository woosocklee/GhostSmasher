using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class GroundCheack : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        private LineRenderer lineRenderer;

        //�׶��� üũ��
        public Ray GroundCheackRay = new Ray();
        public RaycastHit GroundHit = new RaycastHit();
        public LayerMask layerMask;
        private float LastY;
        [HideInInspector]public bool Hang;
        public Vector3 NetworkPos;
        [SerializeField] bool IsLine;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            GroundCheackRay.direction = -playerTransform.up;
            Hang = false;
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            if(Hang)
            {
                return;
            }

            //Ray��ġ �ʱ�ȭ
            Vector3 GroundPos = playerTransform.position;
            GroundCheackRay.origin = new Vector3(GroundPos.x, GroundPos.y + 0.01f, GroundPos.z);

            //�׶��� üũ
            if (Physics.Raycast(GroundCheackRay, out GroundHit, 100, layerMask))
            {
                //�ǳ���ġ ����
                GroundPos = GroundHit.point;
                GroundPos.y += 0.05f;
                LastY = GroundPos.y;
                transform.position = GroundPos;

                if (IsLine)
                {
                    //���̶� ������ ���� ���� ǥ���� �� �Ѵ�.
                    if (Vector3.Distance(transform.position, playerTransform.position) > 0.1f)
                    {
                        lineRenderer.enabled = true;
                        //�������� ǥ��
                        lineRenderer.SetPosition(0, playerTransform.position);
                        lineRenderer.SetPosition(1, GroundPos);
                    }
                    else
                    {
                        lineRenderer.enabled = false;
                    }
                }
            }
            //�ؿ� �ƹ��͵� ���� �ʴ� ���
            else
            {
                transform.position = new Vector3(transform.position.x, LastY, transform.position.z);

                if (IsLine)
                {
                    lineRenderer.enabled = false;
                }
            }         
        }

        public void LineSet()
        {
            IsLine = !IsLine;

            lineRenderer.enabled = IsLine;
        }

        public void ColorChange()
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
    }
}
