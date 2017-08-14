using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using NCore;

namespace NPlayer
{
    public enum eSkill
    {
        FIRE,
        WATER,
        EARTH,
        AIR
    }

    [RequireComponent (typeof(PlayerPhysics))]
    public class PlayerController : MonoBehaviour
    {
        public static string COLLISION_TAG = "Player";

        private PlayerPhysics _physics;
        private Transform _transform;

        [Header ("Global skill preferences")]
        [SerializeField]        private Transform Hands = null;

        [Header ("Fire Skill preference")]
        [SerializeField]        private GameObject FireSkill_prefab = null;
        [SerializeField]        private float fire_impulse_force = 10f;

        [Header ("Water Skill")]
        [SerializeField]        private GameObject WaterSkill_prefab = null;
        [SerializeField]        private float water_impulse_force = 10f;

        [Header("Dash skill preference")]
        [SerializeField]        private float Dash_speed = 20f;
        [SerializeField]        private float Dash_cooldown = 5f;
        [SerializeField]        private float Dash_duration = 5f;

        [Header("Mega Jump skill preference")]
        [SerializeField]        private float MegaJump_power = 10f;
        [SerializeField]        private float MegaJump_cooldown = 5f;

        [Header ("Mouse Preferences")]
        [SerializeField]        private Transform CameraObject = null;
        [SerializeField]        private float X_Sensivity = 10f;
        [SerializeField]        private float Y_Sensivity = 10f;
        [SerializeField]        private float Y_min = -60f;
        [SerializeField]        private float Y_Max = 60f;

        public UnityEvent DeathEvent;
        public UnityEvent StartEvent;

        private Vector3 start_position;
        private Quaternion start_rotation;
        private Vector3 input;
        private float camera_angle = 0f;

        private bool DashCooldownDone = true;
        private bool MegaJumpCooldownDone = true;

        [Header ("Debug")]
       // [SerializeField]        private bool isPaused = false;
        [SerializeField]        private bool isRunning = false;
        [SerializeField]        private bool isDashing = false;
        [SerializeField]        private bool isMegaJumping = false;
        [SerializeField]        private bool FireEnabled = false;
        [SerializeField]        private bool WaterEnabled = false;
        [SerializeField]        private bool MegaJumpEnabled = false;
        [SerializeField]        private bool DashEnabled = false;

        #region MonoBehaviour

        void Awake()
        {
            _physics = GetComponent<PlayerPhysics>();
            _transform = GetComponent<Transform>();
            input = Vector3.zero;
        }

        void Start()
        {
            start_position = _transform.position;
            start_rotation = _transform.rotation;
            App.getApp().TogglePauseEvent.RemoveAllListeners();
            App.getApp().TogglePauseEvent.AddListener(restrictMovement);
        }

        void Update()
        {
            if (App.getGameManager().CurrentGameState == GameManager.eGameState.Pausing)
                return;
            HandleMouseInput();
            if (!_physics.CanMove())
            {
                if (Input.anyKeyDown 
                    && (App.getGameManager().CurrentGameState == GameManager.eGameState.Wait 
                    || App.getGameManager().CurrentGameState == GameManager.eGameState.Death))
                {
                    StartEvent.Invoke();
                    App.changeGameState(GameManager.eGameState.InGame);
                }
                return;
            }
            if (!isDashing)
            {
                getKeyBoardInput();
                HandleSkills();
            }
            else
                input = Vector3.forward;
            if (isMegaJumping)
                input.y = 1f;
            _physics.setImput(input);
        }

        #endregion

        #region Main Methods

        private void getKeyBoardInput()
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.z = Input.GetAxisRaw("Vertical");
            input.y = Input.GetButton("Jump") ? 1f : 0f;
            isRunning = Input.GetButton("Run");
            if (isRunning)
                input.z *= 2;
        }
        
        private void HandleMouseInput()
        {
            float y_angle = -Input.GetAxis("Mouse Y") * Y_Sensivity;

            _transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * X_Sensivity);
            y_angle = Mathf.Clamp(y_angle, Y_min - camera_angle, Y_Max - camera_angle);
            camera_angle += y_angle;
            CameraObject.Rotate(Vector3.right, y_angle);
        }

        private void HandleSkills()
        {
            if (Input.GetMouseButtonDown(0) && FireEnabled)
                launch_fire_skill();
            else if (Input.GetMouseButtonDown(1) && WaterEnabled)
                launch_water_skill();
            else if (Input.GetButton("Dash") && DashEnabled)
                launch_dash_skill();
            else if (Input.GetButton("MegaJump") && MegaJumpEnabled)
                launch_megajump_skill();
        }

        #endregion

        #region SKill Handling

        private void launch_fire_skill()
        {
            Debug.Log("Fire Skill");
            GameObject inst = Instantiate(FireSkill_prefab, Hands.position, Quaternion.identity) as GameObject;
            inst.GetComponent<Rigidbody>().AddForce(CameraObject.forward * fire_impulse_force, ForceMode.Impulse);
        }

        private void launch_water_skill()
        {
            Debug.Log("Water Skill");
            GameObject inst = Instantiate(WaterSkill_prefab, Hands.position, Quaternion.identity) as GameObject;
            inst.GetComponent<Rigidbody>().AddForce(CameraObject.forward * water_impulse_force, ForceMode.Impulse);
        }

        private void launch_dash_skill()
        {
            if (!DashCooldownDone)
                return;
            DashCooldownDone = false;
            Debug.Log("Dash Skill");
            isDashing = true;
            _physics.Dash(Dash_speed, Dash_duration);
            input = Vector3.forward;
            // HUD feedback cooldownD
            App.getGameManager().CurrentInGameController.launchWindCooldown(Dash_cooldown);
            StartCoroutine(Cor_DashCooldown());
            Invoke("StopDash", Dash_duration);
        }

        private void launch_megajump_skill()
        {
            if (!MegaJumpCooldownDone)
                return;
            if (!_physics.MegaJump(MegaJump_power, 1f))
                return;
            MegaJumpCooldownDone = false;
            Debug.Log("MegaJump Skill");
            isMegaJumping = true;
            // HUD feedback cooldown
            App.getGameManager().CurrentInGameController.launchEarthCooldown(MegaJump_cooldown);
            StartCoroutine(Cor_MegaJumpCooldown());
            Invoke("StopMegaJump", 0.5f);
        }

        private void ResetDashCooldown()
        {
            DashCooldownDone = true;
          //  Debug.Log("Dash COOLDOWN DONE");
        }

        private void ResetMegaJumpCooldown()
        {
            MegaJumpCooldownDone = true;
          //  Debug.Log("MegaJump COOLDOWN DONE");
        }

        private void StopDash()
        {
            isDashing = false;
            _physics.Dash(1f, 0.2f);
        }

        private void StopMegaJump()
        {
            isMegaJumping = false;
        }

        private IEnumerator Cor_DashCooldown()
        {
            yield return new WaitForSeconds(Dash_cooldown);
            ResetDashCooldown();
        }

        private IEnumerator Cor_MegaJumpCooldown()
        {
            yield return new WaitForSeconds(MegaJump_cooldown);
            ResetMegaJumpCooldown();
        }

        #endregion

        #region Public

        public void kill()
        {
            Debug.LogWarning("Player Killed");
            App.changeGameState(GameManager.eGameState.Death);
        //    _physics.SetCanMove(false);
            reset();
        }

        public void reset()
        {
            DeathEvent.Invoke();
            _transform.position = start_position;
            _transform.rotation = start_rotation;
            ResetMegaJumpCooldown();
            ResetDashCooldown();
            enableSkill(eSkill.AIR, false);
            enableSkill(eSkill.WATER, false);
            enableSkill(eSkill.FIRE, false);
            enableSkill(eSkill.EARTH, false);
            StopAllCoroutines();
        }

        public void restrictMovement()
        {
            _physics.SetCanMove(false);
        }

        public void enableSkill(eSkill skill, bool val = true)
        {
            NGui.InGameController gameHUDController = App.getGameManager().CurrentInGameController;
            Debug.Log("Obtained Skill " + skill);
            switch (skill)
            {
                case eSkill.AIR:
                    DashEnabled = val;
                    // Adding skill HUD feedback
                    gameHUDController.setActiveWind(val);
                    break;
                case eSkill.WATER:
                    WaterEnabled = val;
                    // Adding skill HUD feedback
                    gameHUDController.setActiveCrosshair(val);
                    gameHUDController.setActiveWater(val);
                    break;
                case eSkill.FIRE:
                    FireEnabled = val;
                    // Adding skill HUD feedback
                    gameHUDController.setActiveCrosshair(val);
                    gameHUDController.setActiveFire(val);
                    break;
                case eSkill.EARTH:
                    MegaJumpEnabled = val;
                    // Adding skill HUD feedback
                    gameHUDController.setActiveEarth(val);
                    break;
                default:
                    Debug.LogError("skill type error : WTF");
                    return;
            }
        }

        #endregion
    }
}
