using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using NCore;

namespace NGui
{
    public class MainMenuController : MonoBehaviour
    {

        public MainElement mainElement = new MainElement();
        public SettingsElement settingsElement = new SettingsElement();
        public ChapterElement chapterElement = new ChapterElement();
        public SoundGuiElement soundGuiElement = new SoundGuiElement();

        void Awake()
        {
        }

        void Start()
        {
            init();
        }

        #region Init
        void init()
        {
            initMainMenu();
            initSettingsMenu();
            initChapterMenu();
            App.changeGameState(GameManager.eGameState.MainMenu);
        }

        void initMainMenu()
        {
            mainElement.init();
            mainElement.newGameButton.onClick.AddListener(launchNewGame);
            mainElement.chapterSelectButton.onClick.AddListener(openChapterSelection);
            mainElement.settingsButton.onClick.AddListener(openSettings);
            mainElement.quitButton.onClick.AddListener(quitGame);
        }

        void initSettingsMenu()
        {
            settingsElement.init();
            settingsElement.returnButton.onClick.AddListener(returnToMainMenu);
        }

        void initChapterMenu()
        {
            chapterElement.init();
            for (int i = 0; i < App.getApp().chapterList.Count; ++i)
            {

                Button chapt = Instantiate(chapterElement.chapterPrefab);
                chapt.name += (i + 1);
                chapt.transform.SetParent(chapterElement.parent.transform);
                chapt.GetComponentInChildren<Text>().text = "" + (i + 1);
                string nameChapt = App.getApp().chapterList[i].nameLevel;
                chapt.onClick.AddListener(delegate { LoadChapter(nameChapt); });
                chapt.interactable = App.getApp().chapterList[i].Unlocked;

            }
            chapterElement.returnButton.onClick.AddListener(returnToMainMenu);
        }
        #endregion

        #region delegate function
        void launchNewGame()
        {
            App.getApp().IndexCurrentChapter = 0;
            SceneManager.LoadScene(App.getApp().chapterList[0].nameLevel);
        }

        void openChapterSelection()
        {
            chapterElement.parent.SetActive(true);
            mainElement.parent.SetActive(false);
        }

        void openSettings()
        {
            settingsElement.parent.SetActive(true);
            mainElement.parent.SetActive(false);
        }

        void quitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        }

        void returnToMainMenu()
        {
            chapterElement.parent.SetActive(false);
            settingsElement.parent.SetActive(false);
            mainElement.parent.SetActive(true);
        }

        void LoadChapter(string chapter)
        {
            for (int i = 0; i < App.getApp().chapterList.Count; ++i)
            {
                if (App.getApp().chapterList[i].nameLevel == chapter)
                {
                    App.getApp().IndexCurrentChapter = i;
                }
            }
            SceneManager.LoadScene(chapter);
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