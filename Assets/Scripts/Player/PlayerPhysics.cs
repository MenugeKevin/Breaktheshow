using UnityEngine;
using System.Collections;
using NElements;

namespace NPlayer
{
    [RequireComponent (typeof(CharacterController))]
    public class PlayerPhysics : MonoBehaviour
    {
        private CharacterController _controller;
        private Transform _transform;

        [Header ("Movement Visualization")]
        [SerializeField]        private Vector3 movement = Vector3.zero;
        [SerializeField]        private Vector3 input = Vector3.zero;
        [SerializeField]        private bool canMove;

        [Header("Movement preferences")]
        [SerializeField]        private float movement_speed = 10f;
        [SerializeField]        private float jump_power = 10f;
        [SerializeField]        private float mass = 10f;

        private float colliderHeight;
        private float jumpPowerSave;
        private float SpeedSave;
        private float MassSave;

        #region MonoBehaviour

        void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _transform = GetComponent<Transform>();
            canMove = false;
            colliderHeight = 1.5f;
            jumpPowerSave = jump_power;
            SpeedSave = movement_speed;
            MassSave = mass;
        }

        void Start()
        {
            StartCoroutine(Cor_CheckGround());
        }

        void FixedUpdate()
        {
            if (!canMove)
                return;
            Move();
        }

        #endregion

        #region Main MEthods

        private IEnumerator Cor_CheckGround()
        {
            while (true)
            {
                if (canMove)
                    checkGround();
                yield return new WaitForFixedUpdate();
            }
        }

        private void Move()
        {
            float y = movement.y;
            //  input = _transform.TransformDirection(input);
            //  movement.x = input.x * movement_speed;
            //  movement.z = input.z * movement_speed;
            movement = _transform.forward * input.z * movement_speed + _transform.right * input.x * movement_speed;
            movement.y = y;
            if (_controller.isGrounded)
            {
                movement.y = 0f;
                if (input.y > 0f)
                {
                    movement.y = jump_power;
                }
            }
            else
            {
                movement.y -= mass * Time.fixedDeltaTime;
            }
            _controller.Move(movement * Time.fixedDeltaTime);
        }

        private void checkGround()
        {
            RaycastHit[] hits;

            hits = Physics.RaycastAll(_transform.position + Vector3.down * colliderHeight, Vector3.down, 0.3f);
            if (hits.Length == 0)
            {
                _transform.parent = null;
                return;
            }
            if (hits[0].collider.gameObject.tag == MovingPlateformScript.COLLISION_TAG)
                _transform.SetParent(hits[0].collider.gameObject.transform);
            else
                _transform.parent = null;
        }

        private void ResetSpeed()
        {
            movement_speed = SpeedSave;
        }

        private void ResetJump()
        {
          //  Debug.Log("physics: Jump Power Reset");
            jump_power = jumpPowerSave;
        }

        private void ResetMass()
        {
            mass = MassSave;
        }

        #endregion

        #region Public

        public bool MegaJump(float val, float time)
        {
            if (_controller.isGrounded == false)
                return false;
          //  Debug.Log("physics: MegaJump");
            jump_power = val;
            Invoke("ResetJump", time);
            return true;
        }

        public void Dash(float val, float time)
        {
            movement_speed = val;
            mass = 0f;
            movement.y = 0f;
            Invoke("ResetSpeed", time);
            Invoke("ResetMass", time);
        }

        public void setImput(Vector3 input)
        {
            this.input = input;
        }

        public void SetCanMove(bool val)
        {
            canMove = val;
        }

        public void ToggleMove(bool _active)
        {
            canMove = _active;
        }

        public bool CanMove()
        {
            return canMove;
        }

        #endregion
    }
}
