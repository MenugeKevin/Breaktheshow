using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using NCore;

namespace NGui
{
    public class InGameController : MonoBehaviour
    {

        public PauseElement pauseElement = new PauseElement();
        public HudElement hudElement = new HudElement();
        public DeathElement deathElement = new DeathElement();
        public EndOfLevelElement endOfLevelElement = new EndOfLevelElement();
        public WaitElement waitElement = new WaitElement();
        public SoundGuiElement soundGuiElement = new SoundGuiElement();

        void Awake()
        {
            App.getGameManager().CurrentInGameController = this;
            if (App.getApp().IndexCurrentChapter == -1)
            {
                initIndexChapter();
            }
        }


        // Use this for initialization
        void Start()
        {
            init();
            App.changeGameState(GameManager.eGameState.Wait);
        }

        void Update()
        {
            GameManager.eGameState state = App.getGameManager().CurrentGameState;
            if (state == GameManager.eGameState.InGame || state == GameManager.eGameState.Wait)
            {
                hudElement.playerTime.text = MathUtility.FormatTime(App.getGameManager().playerTimer);
                if (hudElement.EarthTimer < hudElement.MaxEarthTimer)
                {
                    hudElement.EarthTimer += Time.deltaTime;
                    hudElement.topSkill.earthElement.fillAmount = hudElement.EarthTimer / hudElement.MaxEarthTimer;
                    hudElement.bottomSkill.earthElement.fillAmount = hudElement.EarthTimer / hudElement.MaxEarthTimer;
                }
                if (hudElement.WindTimer < hudElement.MaxWindTimer)
                {
                    hudElement.WindTimer += Time.deltaTime;
                    hudElement.topSkill.windElement.fillAmount = hudElement.WindTimer / hudElement.MaxWindTimer;
                    hudElement.bottomSkill.windElement.fillAmount = hudElement.WindTimer / hudElement.MaxWindTimer;
                }
            }
            if (Input.GetButtonDown("Cancel") && (state
                != GameManager.eGameState.Death || state
                != GameManager.eGameState.EndOfTheLevel))
            {
                App.changeGameState(state == GameManager.eGameState.Pausing ? GameManager.eGameState.Wait : GameManager.eGameState.Pausing);
            }
        }

        void initIndexChapter()
        {
            List<Chapter> chapterList = App.getApp().chapterList;
            for (int i = 0; i < chapterList.Count; ++i)
            {
                if (chapterList[i].nameLevel == SceneManager.GetActiveScene().name)
                {
                    App.getApp().IndexCurrentChapter = i;
                    break;
                }
            }
        }

        #region Initialize 
        void init()
        {
            initPause();
            initHud();
            initDeath();
            initEndOfLevel();
        }

        void initPause()
        {
            pauseElement.init();
            pauseElement.pauseMenuElement.resumeButton.onClick.AddListener(delegate { App.changeGameState(GameManager.eGameState.Wait); });
        }

        void initHud()
        {
            hudElement.init();
        }

        void initDeath()
        {
            deathElement.init();
        }

        void initEndOfLevel()
        {
            endOfLevelElement.init();
            endOfLevelElement.retryButton.onClick.AddListener(delegate { App.changeGameState(GameManager.eGameState.Wait); });
        }
        #endregion

        #region delegate
        public void enablePauseGui(bool _active)
        {
            pauseElement.parent.SetActive(_active);
            waitElement.parent.SetActive(!_active);
            hudElement.parent.SetActive(!_active);
            pauseElement.scoreElement.playerTime.text = MathUtility.FormatTime(App.getGameManager().playerTimer);
        }

        public void enableDeathGui(bool _active)
        {
            hudElement.parent.SetActive(!_active);
            deathElement.parent.SetActive(_active);
        }

        public void enableEndOfLevelGui(bool _active)
        {
            waitElement.parent.SetActive(!_active);
            endOfLevelElement.parent.SetActive(_active);
            endOfLevelElement.scoreElement.playerTime.text = MathUtility.FormatTime(App.getCurrentChapterInfo().PlayerScore);
        }

        public void enableWaitGui(bool _active)
        {
            hudElement.parent.SetActive(!_active);
            waitElement.parent.SetActive(_active);
        }
        #endregion

        #region Element Handle
        public void setActiveFire(bool _active)
        {
            hudElement.topSkill.fireElement.gameObject.SetActive(_active);
            hudElement.bottomSkill.fireElement.gameObject.SetActive(_active);
        }

        public void setActiveWater(bool _active)
        {
            hudElement.topSkill.waterElement.gameObject.SetActive(_active);
            hudElement.bottomSkill.waterElement.gameObject.SetActive(_active);
        }

        public void setActiveWind(bool _active)
        {
            hudElement.topSkill.windElement.gameObject.SetActive(_active);
            hudElement.bottomSkill.windElement.gameObject.SetActive(_active);
        }

        public void setActiveEarth(bool _active)
        {
            hudElement.topSkill.earthElement.gameObject.SetActive(_active);
            hudElement.bottomSkill.earthElement.gameObject.SetActive(_active);
        }

        public void launchWindCooldown(float _maxdelay)
        {
            hudElement.WindTimer = 0f;
            hudElement.MaxWindTimer = _maxdelay;
        }

        public void launchEarthCooldown(float _maxdelay)
        {
            hudElement.EarthTimer = 0f;
            hudElement.MaxEarthTimer = _maxdelay;
        }

        public void setActiveCrosshair(bool _active)
        {
            hudElement.crosshair.gameObject.SetActive(_active);
        }
        #endregion

        public void playSound(string _soundName)
        {
            AudioClip clip = soundGuiElement.getAudioClipByName(_soundName);
            if (clip == null)
            {
                Debug.LogError("Can't find sound named " + _soundName + ", please insert a valid name");
                return;
            }
            soundGuiElement.playSound(clip);
        }

    }
}
