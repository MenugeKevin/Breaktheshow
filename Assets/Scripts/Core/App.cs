using UnityEngine;
using System.Collections;
using NCore;

public class App {

    public static AppManager getApp()
    {
        return AppManager.Instance;
    }

    public static SettingsManager getSettings()
    {
        return getApp().Settings;
    }

    public static NGui.Chapter getCurrentChapterInfo()
    {
        return getApp().chapterList[App.getApp().IndexCurrentChapter];
    }

    public static GameManager getGameManager()
    {
        return getApp().Gamemanager;
    }

    public static void changeGameState(GameManager.eGameState _newState)
    {
        App.getGameManager().CurrentGameState = _newState;
    }
}
