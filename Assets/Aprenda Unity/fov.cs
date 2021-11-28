using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AprendaUnity
{

    public enum viewState
    {
        Primary, Secondary
    }

    public class fov : MonoBehaviour
    {
        public GameObject AIManager; // OBJETO COM O SCRIPT DA IA DO INIMIGO
        
        public float viewRadius = 10f;
        [Range(0, 360)]
        public float viewAngle = 80f;
        [Range(0, 360)]
        public float viewAngleB = 145f;


        [Header("LayerMask")]
        public LayerMask targetMask;
        public LayerMask obstacleMask;

        public List<Transform> visibleTargets = new List<Transform>();
        public List<Transform> visibleTargetsB = new List<Transform>();


        private void Start()
        {
            StartCoroutine("FindTargets");
        }

        void FindVisibleTarget()
        {
            visibleTargets.Clear();
            visibleTargetsB.Clear();

            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);
                        AIManager.SendMessage("IsVisible", viewState.Primary, SendMessageOptions.DontRequireReceiver);
                    }
                }
                else if (Vector3.Angle(transform.forward, dirToTarget) < viewAngleB / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                    {
                        visibleTargetsB.Add(target);
                        AIManager.SendMessage("IsVisible", viewState.Secondary, SendMessageOptions.DontRequireReceiver);
                    }
                }


            }

        }

        public Vector3 DirForAnlge(float angleInDegress)
        {
            angleInDegress += transform.eulerAngles.y;
            return new Vector3(Mathf.Sin(angleInDegress * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegress * Mathf.Deg2Rad));
        }


        IEnumerator FindTargets()
        {
            while(true)
            {
                FindVisibleTarget();
                yield return new WaitForSeconds(0.02f);
            }
        }

    }
}
