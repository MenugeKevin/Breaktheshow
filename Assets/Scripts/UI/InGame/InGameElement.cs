using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using NCore;

namespace NGui
{
    #region Score
    [System.Serializable]
    public class ScoreElement : MenuElement
    {
        public Text goldTime;
        public Text silverTime;
        public Text bronzeTime;
        public Text playerTime;

        public override void init()
        {
            base.init();
            clear();
            Chapter chapter = App.getApp().chapterList[App.getApp().IndexCurrentChapter];
            if (chapter != null)
            {
                goldTime.text = MathUtility.FormatTime(chapter.goldMedal);
                silverTime.text = MathUtility.FormatTime(chapter.silverMedal);
                bronzeTime.text = MathUtility.FormatTime(chapter.bronzeMedal);
                playerTime.text = MathUtility.FormatTime(App.getGameManager().playerTimer);
            }
        }

        public override void clear()
        {
            base.clear();
        }

    }
    #endregion

    [System.Serializable]
    public class SkillGui
    {
        public Image fireElement;
        public Image waterElement;
        public Image windElement;
        public Image earthElement;
    }

    #region HUD
    [System.Serializable]
    public class HudElement : MenuElement
    {
        public Text playerTime;
        public Image crosshair;
        public SkillGui topSkill;
        public SkillGui bottomSkill;

        private float m_windTimer = 0f;
        private float m_earthTimer = 0f;
        private float m_maxWindTimer = 0f;
        private float m_maxEarthTimer = 0f;

        #region getter and setter
        public float MaxWindTimer
        {
            get
            {
                return m_maxWindTimer;
            }

            set
            {
                m_maxWindTimer = value;
            }
        }

        public float WindTimer
        {
            get
            {
                return m_windTimer;
            }

            set
            {
                m_windTimer = value;
            }
        }

        public float EarthTimer
        {
            get
            {
                return m_earthTimer;
            }

            set
            {
                m_earthTimer = value;
            }
        }

        public float MaxEarthTimer
        {
            get
            {
                return m_maxEarthTimer;
            }

            set
            {
                m_maxEarthTimer = value;
            }
        }
        #endregion

        #region override
        public override void init()
        {
            base.init();
            crosshair.gameObject.SetActive(false);
            topSkill.fireElement.gameObject.SetActive(false);
            topSkill.waterElement.gameObject.SetActive(false);
            topSkill.windElement.gameObject.SetActive(false);
            topSkill.earthElement.gameObject.SetActive(false);
            bottomSkill.fireElement.gameObject.SetActive(false);
            bottomSkill.waterElement.gameObject.SetActive(false);
            bottomSkill.windElement.gameObject.SetActive(false);
            bottomSkill.earthElement.gameObject.SetActive(false);
            playerTime.text = MathUtility.FormatTime(App.getGameManager().playerTimer);
        }

        public override void clear()
        {
            base.clear();
        }
        #endregion
    }
    #endregion

    #region Pause
    [System.Serializable]
    public class PauseMenuElement : MenuElement
    {
        public Text chapterNameText;
        public Button resumeButton;
        public Button settingsButton;
        public Button returnToMMButton;

        public override void init()
        {
            base.init();
            clear();
            returnToMMButton.onClick.AddListener(returnToMainMenu);
            chapterNameText.text = App.getCurrentChapterInfo().nameLevel;
        }

        public override void clear()
        {
            base.clear();
            resumeButton.onClick.RemoveAllListeners();
            settingsButton.onClick.RemoveAllListeners();
            returnToMMButton.onClick.RemoveAllListeners();
        }


        void returnToMainMenu()
        {
            App.getGameManager().returnToMainMenu();
        }
    }

    [System.Serializable]
    public class PauseElement : MenuElement
    {
        public PauseMenuElement pauseMenuElement;
        public SettingsElement settingsElement;
        public ScoreElement scoreElement;

        public override void init()
        {
            base.init();
            pauseMenuElement.init();
            settingsElement.init();
            scoreElement.init();
            pauseMenuElement.settingsButton.onClick.AddListener(openSettings);
            settingsElement.returnButton.onClick.AddListener(returnToPauseMenu);
        }

        public override void clear()
        {
            base.clear();
        }

        void openSettings()
        {
            settingsElement.parent.SetActive(true);
            pauseMenuElement.parent.SetActive(false);
        }

        void returnToPauseMenu()
        {
            settingsElement.parent.SetActive(false);
            pauseMenuElement.parent.SetActive(true);
        }

    }
    #endregion

    #region Death
    [System.Serializable]
    public class DeathElement : MenuElement
    {
        public override void init()
        {
            base.init();
        }

        public override void clear()
        {
            base.clear();
        }
    }
    #endregion

    #region Wait
    [System.Serializable]
    public class WaitElement : MenuElement
    {
        public override void init()
        {
            base.init();
        }

        public override void clear()
        {
            base.clear();
        }
    }
    #endregion

    #region EndOfLevel
    [System.Serializable]
    public class EndOfLevelElement : MenuElement
    {
        public ScoreElement scoreElement;
        public Button retryButton;
        public Button returnToMMButton;
        public Button nextLevelButton;

        public override void init()
        {
            base.init();
            clear();
            scoreElement.init();
            returnToMMButton.onClick.AddListener(returnToMainMenu);
            nextLevelButton.onClick.AddListener(loadNextLevel);
        }

        public override void clear()
        {
            base.clear();
            retryButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.RemoveAllListeners();
            returnToMMButton.onClick.RemoveAllListeners();
        }

        void returnToMainMenu()
        {
            App.getGameManager().returnToMainMenu();
        }

        void loadNextLevel()
        {
            int index = App.getApp().IndexCurrentChapter + 1;
            Debug.Log("Changing Scene to " + index);
            if (App.getApp().chapterList.Count > index)
            {
                string nextLevelName = App.getApp().chapterList[index].nameLevel;
                App.getApp().chapterList[index].Unlocked = true;
                App.getApp().IndexCurrentChapter = index;
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                returnToMainMenu();
            }
        }
    }
    #endregion
}
