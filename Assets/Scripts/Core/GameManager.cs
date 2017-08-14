using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace NCore
{
    public class GameManager : Manager
    {
        public enum eGameState
        {
            MainMenu = 0,
            InGame,
            Pausing,
            Wait,
            Death,
            EndOfTheLevel,
        }
        public float playerTimer;

        private delegate void changeStateDelegate();
        changeStateDelegate[] delegates = new changeStateDelegate[6];
        private eGameState currentGameState;
        private NGui.InGameController currentInGameController;


        public eGameState CurrentGameState
        {
            get
            {
                return currentGameState;
            }

            set
            {
                    currentGameState = value;
                    delegates[(int)currentGameState]();
            }
        }

        public NGui.InGameController CurrentInGameController
        {
            get
            {
                return currentInGameController;
            }

            set
            {
                currentInGameController = value;
            }
        }

        public override void init()
        {
            base.init();
            delegates[(int)eGameState.MainMenu] = changeStateToMainMenu;
            delegates[(int)eGameState.InGame] = changeStateToInGame;
            delegates[(int)eGameState.Pausing] = changeStateToPause;
            delegates[(int)eGameState.Death] = changeStateToDeath;
            delegates[(int)eGameState.Wait] = changeStateToWait;
            delegates[(int)eGameState.EndOfTheLevel] = changeStateToEndOfLevel;
        }

        public override void update(float deltaT)
        {
            base.update(deltaT);
            if (CurrentGameState == eGameState.InGame)
            {
                playerTimer += Time.deltaTime;
            }
        }

        public void returnToMainMenu()
        {
            SceneManager.LoadScene(App.getApp().mainMenuSceneName);
            CurrentGameState = eGameState.MainMenu;
        }

        public override void destroy()
        {
            base.destroy();
        }

        void changeStateToMainMenu()
        {
            Debug.Log("State changed to : MainMenu");
            playerTimer = 0;
            Cursor.visible = true;
        }

        void changeStateToInGame()
        {
            Debug.Log("State changed to : Ingame");
            if (CurrentInGameController)
            {
                CurrentInGameController.enableDeathGui(false);
                CurrentInGameController.enableEndOfLevelGui(false);
                CurrentInGameController.enablePauseGui(false);
                currentInGameController.enableWaitGui(false);
                Cursor.visible = false;
            }
            else
            {
                Debug.LogError("HUD reference not specified");
            }
        }

        void changeStateToWait()
        {
            Debug.Log("State changed to : Wait");
            if (CurrentInGameController)
            {
                CurrentInGameController.enableEndOfLevelGui(false);
                CurrentInGameController.enablePauseGui(false);
                currentInGameController.enableWaitGui(true);
                App.getApp().TogglePauseEvent.Invoke(); /////
                Cursor.visible = false;
            }
            else
            {
                Debug.LogError("HUD reference not specified");
            }
        }

        void changeStateToPause()
        {
            Debug.Log("State changed to : Pause");
            if (CurrentInGameController)
            {
                CurrentInGameController.enablePauseGui(true);
                App.getApp().TogglePauseEvent.Invoke(); /////
                Cursor.visible = true;
            }
            else
            {
                Debug.LogError("HUD reference not specified");
            }
        }

        void changeStateToDeath()
        {
            Debug.Log("State changed to : Death");
            if (CurrentInGameController)
            {
                CurrentInGameController.enableDeathGui(true);
                playerTimer = 0;
                Cursor.visible = false;
            }
            else
            {
                Debug.LogError("HUD reference not specified");
            }
        }

        void changeStateToEndOfLevel()
        {
            Debug.Log("State changed to : EOL");
            if (CurrentInGameController)
            {
                CurrentInGameController.enableDeathGui(false);
                CurrentInGameController.enableEndOfLevelGui(true);
                App.getCurrentChapterInfo().PlayerScore = playerTimer;
                CurrentInGameController.endOfLevelElement.scoreElement.playerTime.text = MathUtility.FormatTime(App.getGameManager().playerTimer);
                playerTimer = 0;
                Cursor.visible = true;
            }
            else
            {
                Debug.LogError("HUD reference not specified");
            }
        }
    }
}