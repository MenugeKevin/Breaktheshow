using UnityEngine;
using System.Collections;
using System;

namespace NElements
{
    public class GridScript : Interactible
    {
        [Header("Parameters")]
        [SerializeField]        private float speed = 1f;
        [SerializeField]        private Vector3 triggeredPosition = new Vector3(0, -5, 0);

        private Vector3 targetPosition;

        private Vector3 start_pos;
        private Vector3 goal;
        private Transform _transform;

        void Awake()
        {
            _transform = GetComponent<Transform>();
            start_pos = _transform.position;
            targetPosition = start_pos + triggeredPosition;
        }

        protected override void Init()
        {
            goal = start_pos;
        }

        public override void Trigger()
        {
            base.Trigger();
            StopAllCoroutines();
            goal = goal == targetPosition ? start_pos : targetPosition;
            StartCoroutine(Cor_Movement());
        }

        protected override void Reset()
        {
            _transform.position = start_pos;
            Init();
        }

        private IEnumerator Cor_Movement()
        {
            while (Vector3.Distance(_transform.position, goal) > 0.01f)
            {
                _transform.position = Vector3.Lerp(_transform.position, goal, speed * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
