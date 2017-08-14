using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace NGui
{
    #region Base Class
    [System.Serializable]
    public class MenuElement
    {
        public GameObject parent;

        public virtual void init()
        {

        }

        public virtual void clear()
        {

        }
    }

    #endregion

    #region Main

    [System.Serializable]
    public class MainElement : MenuElement
    {
        public Button newGameButton;
        public Button chapterSelectButton;
        public Button settingsButton;
        public Button quitButton;

        public override void init()
        {
            base.init();
            clear();
        }

        public override void clear()
        {
            newGameButton.onClick.RemoveAllListeners();
            chapterSelectButton.onClick.RemoveAllListeners();
            settingsButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }
    }

    #endregion

    #region Settings

    [System.Serializable]
    public class GraphicsElement : MenuElement
    {
        public Toggle vSyncToggle;
        public Toggle fullscreenToggle;
        public Dropdown resolutionDropDown;

        public override void init()
        {
            base.init();
            vSyncToggle.isOn = App.getSettings().GraphicsOption.VSync;
            fullscreenToggle.isOn = App.getSettings().GraphicsOption.Fullscreen;
            List<string> optionsList = new List<string>();
            int value = 0;
            for (int i = 0; i < App.getSettings().GraphicsOption.ChoiceResolution.Count; ++i)
            {
                optionsList.Add(App.getSettings().GraphicsOption.ChoiceResolution[i].width.ToString()
                                + "x"
                                + App.getSettings().GraphicsOption.ChoiceResolution[i].height.ToString());
                if (optionsList[i] == App.getSettings().GraphicsOption.Resolution.getResByString())
                {
                    value = i;
                }
            }
            resolutionDropDown.ClearOptions();
            resolutionDropDown.AddOptions(optionsList);
            resolutionDropDown.value = value;
            vSyncToggle.onValueChanged.AddListener(changeValueVSync);
            fullscreenToggle.onValueChanged.AddListener(changeValueFullscreen);
            resolutionDropDown.onValueChanged.AddListener(changeValueResolution);
        }

        public override void clear()
        {
            base.clear();
            vSyncToggle.onValueChanged.RemoveAllListeners();
            fullscreenToggle.onValueChanged.RemoveAllListeners();
            resolutionDropDown.onValueChanged.RemoveAllListeners();
        }

        void changeValueVSync(bool _value)
        {
            App.getSettings().GraphicsOption.VSync = _value;
        }

        void changeValueFullscreen(bool _value)
        {
            App.getSettings().GraphicsOption.Fullscreen = _value;
        }

        void changeValueResolution(int _value)
        {
            App.getSettings().GraphicsOption.Resolution = App.getSettings().GraphicsOption.ChoiceResolution[_value];
        }
    }

    [System.Serializable]
    public class ControlsElement : MenuElement
    {
        public Slider mouseSensibility;
        public override void init()
        {
            base.init();
            clear();
            mouseSensibility.value = App.getSettings().ControlsOption.MouseSensitivity;
            mouseSensibility.onValueChanged.AddListener(changeValueMouseSensibility);
        }

        public override void clear()
        {
            base.clear();
            mouseSensibility.onValueChanged.RemoveAllListeners();
        }

        void changeValueMouseSensibility(float _value)
        {
            App.getSettings().ControlsOption.MouseSensitivity = _value;
        }
    }

    [System.Serializable]
    public class AudioElement : MenuElement
    {
        public Slider musicVolume;
        public Slider sfxVolume;

        public override void init()
        {
            base.init();
            clear();
            musicVolume.value = App.getSettings().AudioOption.Music;
            sfxVolume.value = App.getSettings().AudioOption.Sfx;
            musicVolume.onValueChanged.AddListener(changeValueMusic);
            sfxVolume.onValueChanged.AddListener(changeValueSfx);
        }

        public override void clear()
        {
            base.clear();
            musicVolume.onValueChanged.RemoveAllListeners();
            sfxVolume.onValueChanged.RemoveAllListeners();
        }

        void changeValueMusic(float _value)
        {
            App.getSettings().AudioOption.Music = _value;
        }
        void changeValueSfx(float _value)
        {
            App.getSettings().AudioOption.Sfx = _value;
        }
    }

    [System.Serializable]
    public class SettingsElement : MenuElement
    {
        public Button returnButton;
        public GraphicsElement graphicsElem = new GraphicsElement();
        public ControlsElement controlsElem = new ControlsElement();
        public AudioElement audioElem = new AudioElement();


        public override void init()
        {
            base.init();

            clear();

            // Initialize Graphics Element
            graphicsElem.init();

            // Initialize Controls Element
            controlsElem.init();

            // Initialize Audio Element
            audioElem.init();
        }

        public override void clear()
        {
            returnButton.onClick.RemoveAllListeners();
        }
    }

    #endregion

    #region Chapter
    [System.Serializable]
    public class Chapter
    {
        public string nameLevel;
        public Sprite ImageButton;
        public float goldMedal;
        public float silverMedal;
        public float bronzeMedal;
        private float m_playerScore;
        [SerializeField]
        private bool m_unlocked;

        public float PlayerScore
        {
            get
            {
                m_playerScore = PlayerPrefs.GetFloat(nameLevel + "_score", 0f);
                return m_playerScore;
            }

            set
            {
                m_playerScore = value;
                PlayerPrefs.SetFloat(nameLevel + "_score", m_playerScore);
            }
        }

        public bool Unlocked
        {
            get
            {
                //m_unlocked = PlayerPrefs.GetInt(nameLevel + App.getApp().unlockedChapter, 0) == 1 ? true : false;
                return m_unlocked;
            }

            set
            {
                m_unlocked = value;
                PlayerPrefs.SetInt(nameLevel + App.getApp().unlockedChapter, m_unlocked ? 1 : 0);
            }
        }
    }


    [System.Serializable]
    public class ChapterElement : MenuElement
    {
        [Tooltip("Prefab of the button for each chapter")]
        public Button chapterPrefab;

        public Button returnButton;

        public override void init()
        {
            base.init();
            clear();
        }

        public override void clear()
        {
            returnButton.onClick.RemoveAllListeners();
        }
    }
    #endregion

    #region Audio
    [System.Serializable]
    public class SoundGuiElement : MenuElement
    {
        public AudioSource audioSource;
        public List<AudioClip> soundGuiList =  new List<AudioClip>();
        
        public override void init()
        {
            base.init();
            audioSource.volume = App.getSettings().AudioOption.Sfx;
            audioSource.loop = false;
        }

        public override void clear()
        {
            base.clear();
        }

        public void updateVolume()
        {
            audioSource.volume = App.getSettings().AudioOption.Sfx;
        }

        public void playSound(AudioClip clip)
        {
            audioSource.volume = App.getSettings().AudioOption.Sfx;
            audioSource.clip = clip;
            audioSource.Play();
        }

        public AudioClip getAudioClipByName(string _name)
        {
            for (int i = 0; i < soundGuiList.Count; ++i)
            {
                if (soundGuiList[i].name == _name)
                {
                    return soundGuiList[i];
                }
            }
            return null;
        }
    }
    #endregion
}