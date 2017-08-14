using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace NCore
{
    public class AppManager : MonoBehaviour
    {

        //
        public UnityEvent TogglePauseEvent;
        //

        public static AppManager _instance;

        public string mainMenuSceneName = "MainMenu";
        public string unlockedChapter = "_unlocked";

        public List<NGui.Chapter> chapterList = new List<NGui.Chapter>();
        public List<ResolutionValue> choiceResolution = new List<ResolutionValue>();

        public AudioSource musicSource;

        private SettingsManager m_settings = new SettingsManager();
        private GameManager m_gamemanager = new GameManager();
        private int indexCurrentChapter = -1;
        private const string appName = "AppManager";

        public SettingsManager Settings
        {
            get
            {
                return m_settings;
            }

            set
            {
                m_settings = value;
            }
        }

        public GameManager Gamemanager
        {
            get
            {
                return m_gamemanager;
            }

            set
            {
                m_gamemanager = value;
            }
        }

        public int IndexCurrentChapter
        {
            get
            {
                return indexCurrentChapter;
            }

            set
            {
                indexCurrentChapter = value;
            }
        }

        void Awake()
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
            create();
        }


        void create()
        {
            App.getApp();
            if (choiceResolution.Count == 0)
            {
                for (int i = 0; i < Screen.resolutions.Length; ++i)
                {
                    ResolutionValue newValue = new ResolutionValue(Screen.resolutions[i].width, Screen.resolutions[i].height);
                    choiceResolution.Add(newValue);
                }
            }

            PlayerPrefs.SetInt(chapterList[0].nameLevel + unlockedChapter, 1);
            for (int i = 1; i < chapterList.Count; ++i)
            {
#if UNITY_EDITOR
                PlayerPrefs.SetInt(chapterList[i].nameLevel + unlockedChapter, chapterList[i].Unlocked ? 1 : 0);
#else
                chapterList[i].Unlocked = PlayerPrefs.GetInt(chapterList[i].nameLevel + "_unlocked", 0) == 1 ? true : false;
#endif
            }
            App.getSettings().GraphicsOption.ChoiceResolution = choiceResolution;
            Settings.init();
            Gamemanager.init();
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            float deltaTime = Time.deltaTime;
            Settings.update(deltaTime);
            Gamemanager.update(deltaTime);
        }

        void OnApplicationQuit()
        {
            Settings.destroy();
            Gamemanager.destroy();
        }

#region singleton
        public static AppManager Instance
        {
            get
            {
                _instance = GameObject.FindObjectOfType<AppManager>();
                if (_instance == null)
                {
                    _instance = Instantiate<AppManager>(Resources.Load<AppManager>("Core/AppManager"));
                    GameObject.DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }



#endregion
    }
}
