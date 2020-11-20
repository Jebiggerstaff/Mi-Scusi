using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SpeedTutorMainMenuSystem
{
    public class MenuController : MonoBehaviour
    {
        #region Default Values
        [Header("Default Menu Values")]
        [SerializeField] private float defaultBrightness;
        [SerializeField] private float defaultVolume;
        [SerializeField] private int defaultSen;
        [SerializeField] private bool defaultInvertY;

        [Header("Levels To Load")]
        public string _newGameButtonLevel;
        private string levelToLoad;

        private int menuNumber;

        float upNavDelay = 0;
        float downNavDelay = 0;
        float leftNavDelay = 0;
        float rightNavDelay = 0;
        #endregion

        #region Menu Dialogs
        [Header("Main Menu Components")]
        [SerializeField] private GameObject menuDefaultCanvas;
        [SerializeField] private GameObject GeneralSettingsCanvas;
        [SerializeField] private GameObject AchievementsCanvas;
        [SerializeField] private GameObject graphicsMenu;
        [SerializeField] private GameObject soundMenu;
        [SerializeField] private GameObject gameplayMenu;
        [SerializeField] private GameObject controlsMenu;
        [SerializeField] private GameObject confirmationMenu;
        [SerializeField] private GameObject customizationMenu;
        [SerializeField] private GameObject loadingScreenCanvas;
        [SerializeField] private GameObject ResumeGameBtn;
        [Space(10)]
        [Header("Menu Popout Dialogs")]
        [SerializeField] private GameObject noSaveDialog;
        [SerializeField] private GameObject newGameDialog;
        [SerializeField] private GameObject loadGameDialog;
        #endregion

        #region Slider Linking
        [Header("Menu Sliders")]
        [SerializeField] private Text controllerSenText;
        [SerializeField] private Slider controllerSenSlider;
        public float controlSenFloat = 2f;
        [Space(10)]
        [SerializeField] private Brightness brightnessEffect;
        [SerializeField] private Slider brightnessSlider;
        [SerializeField] private Text brightnessText;
        [Space(10)]
        [SerializeField] private Text volumeText;
        [SerializeField] private Slider volumeSlider;
        [Space(10)]
        [SerializeField] private Toggle invertYToggle;
        [Header("Loading Screen Slider")]
        [SerializeField] private Slider progressSlider;
        #endregion

        float originalGameTime;
        float oldAxis;

        [Header("SceneTransitions")]
        public OverlayScene overlayScene;
        public bool needResumeButton = false;

        public GameObject newGameBtn;
        public GameObject exitGameBtn;
        public GameObject firstLevelBtn;
        public GameObject firstLoadBtn;
        public GameObject achievementFirstBtn;
        public GameObject customizeFirstButton;

        public GameObject newGameScroller;

        [Space]
        public StandaloneInputModule kbEventSystem;
        public StandaloneInputModule ctrEventSystem;

        #region Initialisation - Button Selection & Menu Order
        private void Start()
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(newGameBtn);
            menuNumber = 1;
            originalGameTime = Time.timeScale;
            Time.timeScale = 0.0001f;
            //ControllerCheck();
        }
        #endregion

        //MAIN SECTION
        public IEnumerator ConfirmationBox()
        {
            confirmationMenu.SetActive(true);
            yield return new WaitForSeconds(2);
            confirmationMenu.SetActive(false);
        }

        public void onEnable()
        {
            originalGameTime = Time.timeScale;
            Time.timeScale = 0.0001f;
            GoBackToMainMenu();
            //ControllerCheck();
        }
        void onDisable()
        {

        }


        bool ControllerCheck()
        {
            var joystickNames = Input.GetJoystickNames();
            foreach (string joystickName in joystickNames)
                Debug.Log(joystickName);
            if (joystickNames.Length != 0)
            {
                Debug.Log("Controller connected.... switching to controller controls");
                //ctrEventSystem.SetActive(true);
                //kbEventSystem.SetActive(false);
                //EventSystem.current.currentInputModule = ctrEventSystem;

                return true;
            }
            else if (joystickNames.Length == 0)
            {
                Debug.Log("No controller found.... using keyboard controls");
                //EventSystem.current.currentInputModule = kbEventSystem;

                return false;

            }
            else
                return false;
        }




        private void Update()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (Input.GetAxis("Cancel") > oldAxis && !overlayScene.CancelDelay)
            {
                overlayScene.CancelDelay = true;
                overlayScene.cancelDelayCount = 0.1f;

                if (menuNumber == 2 || menuNumber == 7 || menuNumber == 8 || menuNumber == 9 || menuNumber == 10)
                {
                    GoBackToMainMenu();
                    ClickSound();
                }

                else if (menuNumber == 3 || menuNumber == 4 || menuNumber == 5)
                {
                    GoBackToOptionsMenu();
                    ClickSound();
                }

                else if (menuNumber == 6) //CONTROLS MENU
                {
                    GoBackToGameplayMenu();
                    ClickSound();
                }
                else if (menuNumber == 1)
                {
                    if (needResumeButton)
                    {
                        resumeGame();
                    }
                }
            }

            oldAxis = Input.GetAxis("Cancel");

            if (ResumeGameBtn.activeSelf == false && needResumeButton)
            {
                ResumeGameBtn.SetActive(true);
            }


            if(needResumeButton)
            {
                Navigation newNav = newGameBtn.GetComponent<Button>().navigation;
                newNav.selectOnUp = ResumeGameBtn.GetComponent<Button>();
                newGameBtn.GetComponent<Button>().navigation = newNav;

                Navigation secNav = exitGameBtn.GetComponent<Button>().navigation;
                secNav.selectOnDown = ResumeGameBtn.GetComponent<Button>();
                exitGameBtn.GetComponent<Button>().navigation = secNav;
            }
            else
            {
                Navigation newNav = newGameBtn.GetComponent<Button>().navigation;
                newNav.selectOnUp = exitGameBtn.GetComponent<Button>();
                newGameBtn.GetComponent<Button>().navigation = newNav;

                Navigation secNav = exitGameBtn.GetComponent<Button>().navigation;
                secNav.selectOnDown = newGameBtn.GetComponent<Button>();
                exitGameBtn.GetComponent<Button>().navigation = secNav;
            }


            upNavDelay -= Time.unscaledDeltaTime;
            downNavDelay -= Time.unscaledDeltaTime;
            leftNavDelay -= Time.unscaledDeltaTime;
            rightNavDelay -= Time.unscaledDeltaTime;

            if (ControllerCheck())
            {
                var btn = EventSystem.current.currentSelectedGameObject;

                if (btn != null)
                {

                    float x = Mathf.Abs(Input.GetAxis("HorizontalCon"));
                    float y = Mathf.Abs(Input.GetAxis("VerticalCon"));

                    if (x > 0 || y > 0)
                    {
                        if(x > y)
                        {
                            if(Input.GetAxis("HorizontalCon") > 0)
                            {
                                navRight();
                            }
                            else
                            {
                                navLeft();
                            }
                        }
                        else
                        {
                            if (Input.GetAxis("VerticalCon") > 0)
                            {
                                navUp();
                            }
                            else
                            {
                                navDown();
                            }
                        }

                        

                    }
                    if(x == 0)
                    {
                        rightNavDelay = 0;
                        leftNavDelay = 0;
                    }
                    if(y == 0)
                    {
                        upNavDelay = 0;
                        leftNavDelay = 0;
                    }

                }
            }






        }

        void navUp()
        {
            var btn = EventSystem.current.currentSelectedGameObject;
            if(btn.GetComponent<Button>().navigation.selectOnUp != null && upNavDelay <= 0)
            {

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(btn.GetComponent<Button>().navigation.selectOnUp.gameObject);
                upNavDelay = 0.25f;
            }


        }
        void navDown()
        {
            var btn = EventSystem.current.currentSelectedGameObject;
            if (btn.GetComponent<Button>().navigation.selectOnDown != null && downNavDelay <= 0)
            {

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(btn.GetComponent<Button>().navigation.selectOnDown.gameObject);
                downNavDelay = 0.25f;
            }

        }
        void navLeft()
        {
            var btn = EventSystem.current.currentSelectedGameObject;
            if (btn.GetComponent<Button>().navigation.selectOnLeft != null && leftNavDelay <= 0)
            {

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(btn.GetComponent<Button>().navigation.selectOnLeft.gameObject);
                leftNavDelay = 0.25f;
            }

        }
        void navRight()
        {
            var btn = EventSystem.current.currentSelectedGameObject;
            if (btn.GetComponent<Button>().navigation.selectOnRight != null && rightNavDelay <= 0)
            {

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(btn.GetComponent<Button>().navigation.selectOnRight.gameObject);
                rightNavDelay = 0.25f;
            }

        }
        private void ClickSound()
        {
            GetComponent<AudioSource>().Play();
        }

        #region Menu Mouse Clicks
        public void MouseClick(string buttonType)
        {
            if (buttonType == "Controls")
            {
                gameplayMenu.SetActive(false);
                controlsMenu.SetActive(true);
                menuNumber = 6;
            }

            if (buttonType == "Graphics")
            {
                GeneralSettingsCanvas.SetActive(false);
                graphicsMenu.SetActive(true);
                menuNumber = 3;
            }

            if (buttonType == "Sound")
            {
                GeneralSettingsCanvas.SetActive(false);
                soundMenu.SetActive(true);
                menuNumber = 4;
            }

            if (buttonType == "Gameplay")
            {
                GeneralSettingsCanvas.SetActive(false);
                gameplayMenu.SetActive(true);
                menuNumber = 5;
            }

            if (buttonType == "Exit")
            {
                Debug.Log("YES QUIT!");
                Application.Quit();
            }

            if (buttonType == "Options")
            {
                menuDefaultCanvas.SetActive(false);
                GeneralSettingsCanvas.SetActive(true);
                menuNumber = 2;
            }

            if (buttonType == "LoadGame")
            {
                menuDefaultCanvas.SetActive(false);
                loadGameDialog.SetActive(true);
                menuNumber = 8;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstLoadBtn);
            }

            if (buttonType == "NewGame")
            {
                menuDefaultCanvas.SetActive(false);
                newGameDialog.SetActive(true);

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstLevelBtn);

                menuNumber = 7;
            }

            if (buttonType == "Achievements")
            {
                menuDefaultCanvas.SetActive(false);
                AchievementsCanvas.SetActive(true);
                menuNumber = 9;

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(achievementFirstBtn);

            }

            if (buttonType == "Customize")
            {
                menuDefaultCanvas.SetActive(false);
                customizationMenu.SetActive(true);
                menuNumber = 10;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(customizeFirstButton);
            }
        }
        #endregion

        public void VolumeSlider(float volume)
        {
            AudioListener.volume = volume;
            volumeText.text = volume.ToString("0.0");
        }

        public void VolumeApply()
        {
            PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
            Debug.Log(PlayerPrefs.GetFloat("masterVolume"));
            StartCoroutine(ConfirmationBox());
        }

        public void BrightnessSlider(float brightness)
        {
            brightnessEffect.brightness = brightness;
            brightnessText.text = brightness.ToString("0.0");
        }

        public void BrightnessApply()
        {
            PlayerPrefs.SetFloat("masterBrightness", brightnessEffect.brightness);
            Debug.Log(PlayerPrefs.GetFloat("masterBrightness"));
            StartCoroutine(ConfirmationBox());
        }

        public void ControllerSen()
        {
            controllerSenText.text = controllerSenSlider.value.ToString("0");
            controlSenFloat = controllerSenSlider.value;
        }

        public void GameplayApply()
        {
            if (invertYToggle.isOn) //Invert Y ON
            {
                PlayerPrefs.SetInt("masterInvertY", 1);
                Debug.Log("Invert" + " " + PlayerPrefs.GetInt("masterInvertY"));
            }

            else if (!invertYToggle.isOn) //Invert Y OFF
            {
                PlayerPrefs.SetInt("masterInvertY", 0);
                Debug.Log(PlayerPrefs.GetInt("masterInvertY"));
            }

            PlayerPrefs.SetFloat("masterSen", controlSenFloat);
            Debug.Log("Sensitivity" + " " + PlayerPrefs.GetFloat("masterSen"));

            StartCoroutine(ConfirmationBox());
        }

        #region ResetButton
        public void ResetButton(string GraphicsMenu)
        {
            if (GraphicsMenu == "Brightness")
            {
                brightnessEffect.brightness = defaultBrightness;
                brightnessSlider.value = defaultBrightness;
                brightnessText.text = defaultBrightness.ToString("0.0");
                BrightnessApply();
            }

            if (GraphicsMenu == "Audio")
            {
                AudioListener.volume = defaultVolume;
                volumeSlider.value = defaultVolume;
                volumeText.text = defaultVolume.ToString("0.0");
                VolumeApply();
            }

            if (GraphicsMenu == "Graphics")
            {
                controllerSenText.text = defaultSen.ToString("0");
                controllerSenSlider.value = defaultSen;
                controlSenFloat = defaultSen;

                invertYToggle.isOn = false;

                GameplayApply();
            }
        }
        #endregion

        #region Dialog Options - This is where we load what has been saved in player prefs!
        public void ClickNewGameDialog(string ButtonType)
        {
            if (ButtonType == "Yes")
            {
                StartCoroutine(loadALevel());
            }

            if (ButtonType == "No")
            {
                GoBackToMainMenu();
            }
        }
        public void SelectLevel(string levelName)
        {
            _newGameButtonLevel = levelName;
            PlayerPrefs.SetString("SavedLevel", levelName);
            ClickNewGameDialog("Yes");
        }
        IEnumerator loadALevel()
        {


            //***OLD LOADING BAR****//
            //TurnOnLoadingScreen();

            //****NEW LOADING******//
            FindObjectOfType<StartSceneManager>().FadeScene(false);

            Time.timeScale = 1;




            /*OLD METHOD
            AsyncOperation async = SceneManager.LoadSceneAsync(_newGameButtonLevel);

            while (!async.isDone)
            {
                progressSlider.value = async.progress;
                yield return null;
            }
            */

            //NEW METHOD//
            yield return new WaitForSeconds(1.5f);
            FindObjectOfType<StartSceneManager>().swapScenes(_newGameButtonLevel);
            

        }
        public void resumeGame()
        {
            needResumeButton = false;
            Time.timeScale = 1;
            gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void ClickLoadGameDialog(string ButtonType)
        {
            if (ButtonType == "Yes")
            {
                if (PlayerPrefs.HasKey("SavedLevel"))
                {
                    Debug.Log("I WANT TO LOAD THE SAVED GAME");
                    //LOAD LAST SAVED SCENE
                    levelToLoad = PlayerPrefs.GetString("SavedLevel");
                    SceneManager.LoadScene(levelToLoad);
                }

                else
                {
                    Debug.Log("Load Game Dialog");
                    menuDefaultCanvas.SetActive(false);
                    loadGameDialog.SetActive(false);
                    noSaveDialog.SetActive(true);
                }
            }

            if (ButtonType == "No")
            {
                GoBackToMainMenu();
            }
        }



        public void LoadCharacter()
        {
            var modelLoaders = FindObjectsOfType<CharacterModelLoader>();
            foreach(var modelLoader in modelLoaders)
            {
                modelLoader.LoadAllModels();
            }
        }
        public void SetHat(int cosmetic)
        {
            PlayerPrefs.SetInt("Costume_Hat", cosmetic);
            LoadCharacter();
        }
        public void SetShirt(int cosmetic)
        {
            PlayerPrefs.SetInt("Costume_Shirt", cosmetic);
            LoadCharacter();
        }
        public void SetCoat(int cosmetic)
        {
            PlayerPrefs.SetInt("Costume_Coat", cosmetic);
            LoadCharacter();
        }
        public void SetPants(int cosmetic)
        {
            PlayerPrefs.SetInt("Costume_Pants", cosmetic);
            LoadCharacter();
        }
        public void SetBackpack(int cosmetic)
        {
            PlayerPrefs.SetInt("Costume_Backpack", cosmetic);
            LoadCharacter();
        }
        public void SetAccessory(int cosmetic)
        {
            PlayerPrefs.SetInt("Costume_Accessory", cosmetic);
            LoadCharacter();
        }

        #endregion

        #region Back to Menus
        public void GoBackToOptionsMenu()
        {
            GeneralSettingsCanvas.SetActive(true);
            graphicsMenu.SetActive(false);
            soundMenu.SetActive(false);
            gameplayMenu.SetActive(false);

            GameplayApply();
            BrightnessApply();
            VolumeApply();

            menuNumber = 2;
        }

        public void GoBackToMainMenu()
        {
            menuDefaultCanvas.SetActive(true);
            newGameDialog.SetActive(false);
            loadGameDialog.SetActive(false);
            noSaveDialog.SetActive(false);
            GeneralSettingsCanvas.SetActive(false);
            AchievementsCanvas.SetActive(false);
            graphicsMenu.SetActive(false);
            soundMenu.SetActive(false);
            gameplayMenu.SetActive(false);
            customizationMenu.SetActive(false);
            loadingScreenCanvas.SetActive(false);
            menuNumber = 1;

            EventSystem.current.SetSelectedGameObject(null);

            if (needResumeButton)
            {
                ResumeGameBtn.SetActive(true);

                EventSystem.current.SetSelectedGameObject(ResumeGameBtn);
            }
            else
            {
                
                ResumeGameBtn.SetActive(false);

                EventSystem.current.SetSelectedGameObject(newGameBtn);
            }
        }

        public void TurnOnLoadingScreen()
        {
            menuDefaultCanvas.SetActive(false);
            newGameDialog.SetActive(false);
            loadGameDialog.SetActive(false);
            noSaveDialog.SetActive(false);
            GeneralSettingsCanvas.SetActive(false);
            AchievementsCanvas.SetActive(false);
            graphicsMenu.SetActive(false);
            soundMenu.SetActive(false);
            gameplayMenu.SetActive(false);
            customizationMenu.SetActive(false);
            loadingScreenCanvas.SetActive(true);
        }

        public void GoBackToGameplayMenu()
        {
            controlsMenu.SetActive(false);
            gameplayMenu.SetActive(true);
            menuNumber = 5;
        }

        public void ClickQuitOptions()
        {
            GoBackToMainMenu();
        }

        public void ClickNoSaveDialog()
        {
            GoBackToMainMenu();
        }

        public void CenterScrollRect(GameObject selected)
        {
            var scrollRect = newGameScroller.GetComponent<ScrollRect>();
            scrollRect.content.localPosition = new Vector3(-selected.transform.localPosition.x + scrollRect.GetComponent<RectTransform>().rect.width/2, scrollRect.content.localPosition.y, scrollRect.content.localPosition.z);
        }

        #endregion
    }
}