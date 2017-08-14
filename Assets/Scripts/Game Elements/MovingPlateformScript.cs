using UnityEngine;
using System.Collections;
using NPlayer;
using System;

namespace NElements
{
    public class MovingPlateformScript : Interactible
    {
        public static string COLLISION_TAG = "Plateform";
        
        [Header("Movement parameters")]
        [SerializeField]        private float Smooth = 0.3f;
        [SerializeField]        private Vector3 TargetStart = Vector3.zero;
        [SerializeField]        private Vector3 TargetEnd = Vector3.zero;

        [Header ("Movement Debug")]
        [SerializeField]        private Vector3 goal;
        [SerializeField]        private Vector3 last;
        [SerializeField]        private bool isMoving;

        private Transform _transform;
        private Vector3 start_pos;
        private Vector3 velocity = Vector3.zero;

        void Awake()
        {
            _transform = GetComponent<Transform>();
            start_pos = _transform.position;
        }

        protected override void Init()
        {
            goal = TargetStart;
            last = TargetEnd;
            isMoving = false;
        }

        public override void Trigger()
        {
            Debug.LogError("TRIGGER");
            base.Trigger();
            isMoving = !isMoving;
            if (isMoving)
                StartCoroutine(Cor_Movement());
            else
                StopAllCoroutines();
        }

        protected override void Reset()
        {
            Debug.LogError("RESET");
            _transform.position = start_pos;
            Init();
            StopAllCoroutines();
        }

        private IEnumerator Cor_Movement()
        {
            Vector3 tmp;

            while (true)
            {
                if (Vector3.Distance(_transform.position, goal) < 0.1f)
                {
                    tmp = last;
                    last = goal;
                    goal = tmp;
                }
                _transform.position = Vector3.SmoothDamp(_transform.position, goal, ref velocity, Smooth);
             
                yield return new WaitForFixedUpdate();
            }
        }
    }
}