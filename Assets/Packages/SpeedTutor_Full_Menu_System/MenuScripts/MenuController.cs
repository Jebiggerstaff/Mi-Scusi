using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Audio;

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

        #region Level Unlocking
        public const string LevelUnlockedPref = "UnlockedLevels";
        public const string LevelBuffer = "$%";
        #endregion


        [Header("Audio Controls")]
        public AudioMixer ScusiAudio;
        public AudioMixerGroup Master;
        public AudioMixerGroup SoundEffects;
        public AudioMixerGroup Music;
        public AudioMixerGroup Ambience;
        public Slider MasterSlide;
        public Slider EffectSlide;
        public Slider MusicSlide;
        public Slider AmbienceSlide;




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
        public GameObject CustomizeScroller;

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


            if(!PlayerPrefs.HasKey(LevelUnlockedPref))
            {
                PlayerPrefs.SetString(LevelUnlockedPref, "");
            }

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


        }
        private MiScusiActions controls;
        private void Awake()
        {
            controls = new MiScusiActions();
            controls.Enable();

            SetAllVolumes();
        }


        bool ControllerCheck()
        {
            if (Gamepad.current!=null)
            {
                Debug.Log("Controller connected.... switching to controller controls");
                //ctrEventSystem.SetActive(true);
                //kbEventSystem.SetActive(false);
                //EventSystem.current.currentInputModule = ctrEventSystem;

                return true;
            }
            else
                return false;
        }




        private void Update()
        {
            if(Time.timeScale <= 0.01)
            {

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }

            if ((controls.UI.PauseMenu.ReadValue<float>() > oldAxis || controls.UI.MenuReturn.triggered) && !overlayScene.CancelDelay)
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

            oldAxis = controls.UI.PauseMenu.ReadValue<float>();

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
                    /*
                    float x = Mathf.Abs(controls.Player.MoveX.ReadValue<float>());
                    float y = Mathf.Abs(controls.Player.MoveY.ReadValue<float>());

                    if (x > 0 || y > 0)
                    {
                        if(x > y)
                        {
                            if(controls.Player.MoveX.ReadValue<float>() > 0)
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
                            if (controls.Player.MoveY.ReadValue<float>() > 0)
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
                    */
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
                PlayerPrefs.Save();
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
            FindObjectOfType<StartSceneManager>().TurnOnPlayerCamera();
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
            PlayerPrefs.Save();
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
                newGameBtn.GetComponent<Text>().text = "Level Select";
                EventSystem.current.SetSelectedGameObject(ResumeGameBtn);
            }
            else
            {
                
                ResumeGameBtn.SetActive(false);

                newGameBtn.GetComponent<Text>().text = "Play Game";
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

            if (controls.UI.Click.triggered)
                selected.GetComponent<Button>().onClick.Invoke();
        }
        public void CenterCustomizeRect()
        {

            var scrollRect = CustomizeScroller.GetComponent<ScrollRect>();
            var selected = EventSystem.current.currentSelectedGameObject;


            float maxY = Mathf.NegativeInfinity;
            float minY = Mathf.Infinity;

            foreach(var go in scrollRect.content.GetComponentsInChildren<RectTransform>())
            {
                if (go.position.y < minY)
                    minY = go.position.y;
                if (go.position.y > maxY)
                    maxY = go.position.y;
            }

            scrollRect.verticalScrollbar.value = 1f - (maxY - selected.GetComponent<RectTransform>().position.y)/ (maxY - minY);

            if (controls.UI.Click.triggered)
                selected.GetComponent<Button>().onClick.Invoke();



        }

        #endregion




        float VolumePercentToDecibal(float percent)
        {
            if (percent == 0)
            {
                return -80f;
            }
            else
            {
                return Mathf.Log10(percent) * 20f;
            }
        }

        public void SetMasterVolume(float value)
        {
            ScusiAudio.SetFloat("MasterVolume", VolumePercentToDecibal(value));
            PlayerPrefs.SetFloat("MasterVolume", value);
            PlayerPrefs.Save();
        }
        public void SetAmbienceVolume(float value)
        {
            ScusiAudio.SetFloat("AmbienceVolume", VolumePercentToDecibal(value));
            PlayerPrefs.SetFloat("AmbienceVolume", value);
            PlayerPrefs.Save();
        }
        public void SetSoundEffectsVolume(float value)
        {
            ScusiAudio.SetFloat("SoundEffectsVolume", VolumePercentToDecibal(value));
            PlayerPrefs.SetFloat("SoundEffectsVolume", value);
            PlayerPrefs.Save();
        }
        public void SetMusicVolume(float value)
        {
            ScusiAudio.SetFloat("MusicVolume", VolumePercentToDecibal(value));
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save();
        }

        void SetAllVolumes()
        {
            SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1));
            MasterSlide.value = PlayerPrefs.GetFloat("MasterVolume", 1);
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1));
            MusicSlide.value = PlayerPrefs.GetFloat("MusicVolume", 1);
            SetSoundEffectsVolume(PlayerPrefs.GetFloat("SoundEffectsVolume", 1));
            EffectSlide.value = PlayerPrefs.GetFloat("SoundEffectsVolume", 1);
            SetAmbienceVolume(PlayerPrefs.GetFloat("AmbienceVolume", 1));
            AmbienceSlide.value = PlayerPrefs.GetFloat("AmbienceVolume", 1);
        }
    }




    
}