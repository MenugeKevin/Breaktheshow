using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NCore
{
    [System.Serializable]
    public class ResolutionValue
    {
        public ResolutionValue()
        {
            width = 0;
            height = 0;
        }

        public ResolutionValue(int _width, int _height)
        {
            width = _width;
            height = _height;
        }

        public string getResByString()
        {
            return width.ToString() + "x" + height.ToString();
        }

        public int width;
        public int height;
    }

    [System.Serializable]
    class Graphics
    {
        List<ResolutionValue> m_choiceResolution = new List<ResolutionValue>();
        ResolutionValue m_currentResolution = new ResolutionValue();
        bool m_vSync = false;
        int m_refreshRate = 60;
        bool m_fullscreen = true;

        public ResolutionValue Resolution
        {
            get
            {
                m_currentResolution.height = PlayerPrefs.GetInt("height", Screen.height);
                m_currentResolution.width = PlayerPrefs.GetInt("width", Screen.width);
                return m_currentResolution;
            }

            set
            {
                m_currentResolution = value;
                Screen.SetResolution(m_currentResolution.width, m_currentResolution.height, m_fullscreen, RefreshRate);
                PlayerPrefs.SetInt("width", m_currentResolution.width);
                PlayerPrefs.SetInt("height", m_currentResolution.height);
            }
        }

        public bool VSync
        {
            get
            {
                m_vSync = (PlayerPrefs.GetInt("vsync", QualitySettings.vSyncCount) > 0 ? true : false);
                return m_vSync;
            }

            set
            {
                m_vSync = value;
                QualitySettings.vSyncCount = m_vSync ? 1 : 0;
                PlayerPrefs.SetInt("vsync", QualitySettings.vSyncCount);
            }
        }

        public bool Fullscreen
        {
            get
            {
                m_fullscreen = (PlayerPrefs.GetInt("fullscreen", Screen.fullScreen == true ? 1 : 0) == 1 ? true : false);
                return m_fullscreen;
            }

            set
            {
                m_fullscreen = value;
                PlayerPrefs.SetInt("fullscreen", m_fullscreen ? 1 : 0);
                Screen.SetResolution(Resolution.width, Resolution.height, m_fullscreen, RefreshRate);
            }
        }

        public int RefreshRate
        {
            get
            {
                return m_refreshRate;
            }

            set
            {
                m_refreshRate = value;
            }
        }

        public List<ResolutionValue> ChoiceResolution
        {
            get
            {
                return m_choiceResolution;
            }

            set
            {
                m_choiceResolution.Clear();
                m_choiceResolution = value;
            }
        }
    }

    [System.Serializable]
    class Controls
    {
        float m_mouseSensitivity;

        public float MouseSensitivity
        {
            get
            {
                m_mouseSensitivity = PlayerPrefs.GetFloat("sensitivity", 10f);
                return m_mouseSensitivity;
            }

            set
            {
                m_mouseSensitivity = value;
                PlayerPrefs.SetFloat("sensitivity", m_mouseSensitivity);
            }
        }
    }

    [System.Serializable]
    class Audio
    {
        float m_music;
        float m_sfx;

        public float Music
        {
            get
            {
                m_music = PlayerPrefs.GetFloat("music", 1f);
                return m_music;
            }

            set
            {
                m_music = value;
                PlayerPrefs.SetFloat("music", m_music);
                App.getApp().musicSource.volume = m_music;
            }
        }

        public float Sfx
        {
            get
            {
                m_sfx = PlayerPrefs.GetFloat("sfx", 1f);
                return m_sfx;
            }

            set
            {
                m_sfx = value;
                PlayerPrefs.SetFloat("sfx", m_sfx);
            }
        }
    }

    public class SettingsManager : Manager
    {
        private Graphics m_graphicsOption = new Graphics();
        private Controls m_controlsOption = new Controls();
        private Audio m_audioOption = new Audio();

        internal Graphics GraphicsOption
        {
            get
            {
                return m_graphicsOption;
            }
        }

        internal Controls ControlsOption
        {
            get
            {
                return m_controlsOption;
            }
        }

        internal Audio AudioOption
        {
            get
            {
                return m_audioOption;
            }
        }

        void initPlayerPref()
        {
            PlayerPrefs.SetInt("fullscreen", Screen.fullScreen ? 1 : 0);
            PlayerPrefs.SetInt("height", Screen.height);
            PlayerPrefs.SetInt("width", Screen.width);
            PlayerPrefs.SetInt("vsync", QualitySettings.vSyncCount);
        }

        public override void init()
        {
            base.init();

            initPlayerPref();

            // Graphics Init
            m_graphicsOption.RefreshRate = m_graphicsOption.RefreshRate;
            m_graphicsOption.Resolution = m_graphicsOption.Resolution;
            m_graphicsOption.Fullscreen = m_graphicsOption.Fullscreen;
            m_graphicsOption.VSync = m_graphicsOption.VSync;

            // Controls Init
            m_controlsOption.MouseSensitivity = m_controlsOption.MouseSensitivity;

            // Audio Init
            m_audioOption.Music = m_audioOption.Music;
            m_audioOption.Sfx = m_audioOption.Sfx;
        }

        public override void update(float deltaT)
        {
            base.update(deltaT);
        }

        public override void destroy()
        {
            base.destroy();
        }
    }
}
