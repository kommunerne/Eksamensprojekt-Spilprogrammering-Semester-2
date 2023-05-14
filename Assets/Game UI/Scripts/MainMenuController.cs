using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Mirror;
using Mono.Data.Sqlite;

public class MainMenuController : MonoBehaviour
{

    private GunnerNetworkManager manager;
    
    #region Vairbales
    
    
    
    // Main Menu
    
        // Scene

        private Scene mainMenu;
    
    
        // UI Document

        private UIDocument uiDoc;
    
        // Buttons
        private Button _loadLoadGameMenuButton;
        private Button _loadNewGameMenuButton;
        private Button _loadSettingsMenuButton;
        private Button _exitMainMenu;
    
        // Visual Elements
        private VisualElement _mainMenu;
    
    // Load Game Menu
    
        // Buttons
        private Button _returnFromLoadGameMenu;
        private Button _loadGameButton;
   
        // Text Elements
        private TextField _loadUsername;
        private TextField _loadPinCode;
        // public string loadUsername;
        // public string loadPinCode;
        
        // Visual Elements
        private VisualElement _loadGameMenu;
        
        // Toggle

        private Toggle _loadHostToggle;
        
    // New Game Menu
    
        // Buttons
        private Button _returnFromNewGameMenu;
        private Button _createNewTank;
        
        // Text Elements
        private TextField _newUsername;
        private TextField _newPinCode;
        // public string newUsername;
        // public string newPinCode;
        
        // Toggles
        private Toggle _bigGunTankToggle;
        private Toggle _sniperTankToggle;
        private Toggle _gunnerTankToggle;
        private Toggle _machineTankToggle;
        private Toggle _newHostToggle;
        
        // Toggle to int converter

        public int _toggleToInt;
        
        // Visual Elements
        private VisualElement _newGameMenu;
        
    // Settings Menu
    
        // Buttons
        private Button _returnFromSettingsMenu;
        
        // Sliders
        private Slider _musicSlider;
        private Slider _effectsSlider;
        
        // Visual Elements
        private VisualElement _settingsMenu;
    
    // Database

        private DBScript _db;
    
    // NetworkManager Boolean to check if new character or old character is created

        public bool isNewPlayer;
        private bool _loadHostBool;
        private bool _newHostBool;
        
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        mainMenu = SceneManager.GetSceneByName("MainMenu");

        #region Instantiating
        // Components
        uiDoc = GetComponent<UIDocument>();
        var root = GetComponent<UIDocument>().rootVisualElement;
        manager = FindObjectOfType(typeof(GunnerNetworkManager)).GetComponent<GunnerNetworkManager>();
        _db = GetComponent<DBScript>();
        
        // Main Menu
        
            // Buttons
            _loadLoadGameMenuButton = root.Q<Button>("goToLoadGameMenu");
            _loadNewGameMenuButton = root.Q<Button>("goToNewGameMenu");
            _loadSettingsMenuButton = root.Q<Button>("goToSettingsMenu");
            _exitMainMenu = root.Q<Button>("exitMainMenu");
            
            // Visual Elements
            _mainMenu = root.Q<VisualElement>("MainMenu");

        // Load Game Menu
        
            // Buttons
            _loadGameButton = root.Q<Button>("loadGameButton");
            _returnFromLoadGameMenu = root.Q<Button>("returnFromLoadGameMenu");
        
            // Text Elements
            _loadUsername = root.Q<TextField>("loadUsername");
            _loadPinCode = root.Q<TextField>("loadPinCode");
    
            // Visual Elements
            _loadGameMenu = root.Q<VisualElement>("LoadGameMenu");

            // Toggle

            _loadHostToggle = root.Q<Toggle>("loadHostToggle");
            
        // New Game Menu
                    
            // Buttons
            _returnFromNewGameMenu = root.Q<Button>("returnFromNewGameMenu");
            _createNewTank = root.Q<Button>("createNewTank");
                
            // Toggles
            _bigGunTankToggle = root.Q<Toggle>("bigGunTankToggle");
            _sniperTankToggle = root.Q<Toggle>("sniperTankToggle");
            _gunnerTankToggle = root.Q<Toggle>("gunnerTankToggle");
            _machineTankToggle = root.Q<Toggle>("machineTankToggle");
            _newHostToggle = root.Q<Toggle>("newHostToggle");
            
            // Visual Elements
            _newGameMenu = root.Q<VisualElement>("NewGameMenu");
            
            // Text Fields

            _newUsername = root.Q<TextField>("newUsername");
            _newPinCode = root.Q<TextField>("newPinCode");
                        
        // Settings Menu
        
            // Buttons
            _returnFromSettingsMenu = root.Q<Button>("returnFromSettingsMenu");
                
            // Sliders
            _musicSlider = root.Q<Slider>("musicSlider");
            _effectsSlider = root.Q<Slider>("effectsSlider");
            
            // Visual Elements
            _settingsMenu = root.Q<VisualElement>("SettingsMenu");

            #endregion
    
        
        // On game start

        _mainMenu.style.display = DisplayStyle.Flex;
        _loadGameMenu.style.display = DisplayStyle.None;
        _newGameMenu.style.display = DisplayStyle.None;
        _settingsMenu.style.display = DisplayStyle.None;

        // Methods called on button click
        
            // Main Menu
            
            _loadLoadGameMenuButton.clicked += LoadLoadGameMenu;
            _loadSettingsMenuButton.clicked += LoadSettingsMenu;
            _loadNewGameMenuButton.clicked += LoadNewGameMenu;
            _exitMainMenu.clicked += ExitGame;
        
            // Load Game Menu

            _loadGameButton.clicked += LoadSavedGame;
            _returnFromLoadGameMenu.clicked += UnloadLoadGameMenu;
            
            // New Game Menu

            _createNewTank.clicked += LoadNewGame;
            _returnFromNewGameMenu.clicked += UnloadNewGameMenu;
            
            // Settings Menu
            
            _returnFromSettingsMenu.clicked += UnloadSettingsMenu;

    }
    void Update()
    {
        TogglesController();
        GetToggle();

        if (mainMenu == SceneManager.GetActiveScene())
        {
            uiDoc.enabled = true;
        }
        else
        {
            uiDoc.enabled = false;
        }
        _newHostBool = _newHostToggle.value;
        _loadHostBool = _loadHostToggle.value;
    }

    void FixedUpdate()
    {
        
    }
    // Main Menu Methods
    
        private void LoadLoadGameMenu()
        {
            _loadGameMenu.style.display = DisplayStyle.Flex;
        }
        private void LoadSettingsMenu()
        {
            _settingsMenu.style.display = DisplayStyle.Flex;
        }
        private void LoadNewGameMenu()
        {
            _newGameMenu.style.display = DisplayStyle.Flex;
        }
        private void ExitGame()
        {
            
        }
    
    // LoadGame Menu Methods
    
        private void LoadSavedGame()
        {
            Player loadedPlayer = _db.GetPlayer(_loadUsername.value, _loadPinCode.value);
            if (loadedPlayer != null)
            {
                isNewPlayer = false;
                if (_loadHostBool)
                {
                    manager.StartHost();
                }
                else
                {
                    manager.networkAddress = "localhost";
                    manager.StartClient();
                }
                uiDoc.enabled = false;
            }
        }
        private void UnloadLoadGameMenu()
        {
            _loadGameMenu.style.display = DisplayStyle.None;
        }

    // New Game Menu Methods

    private void LoadNewGame()
    {
        ToggleToInt();
        isNewPlayer = true;
        Player newPlayer = new Player(_newUsername.value, _newPinCode.value, _toggleToInt, 0, 0, 0);
        
        _db.CreatePlayer(newPlayer);
        ToggleToInt();
        if (_newHostBool)
        {
            manager.StartHost();
        }
        else
        {
            manager.networkAddress = "localhost";
            manager.StartClient();
        }
        //uiDoc.enabled = false;
    }
    private void UnloadNewGameMenu()
    {
        _newGameMenu.style.display = DisplayStyle.None;
    }
    private void TogglesController()
    {
        switch (GetToggle())
        {
            case "bigGunTank":
                _sniperTankToggle.value = false;
                _gunnerTankToggle.value = false;
                _machineTankToggle.value = false;
                break;
            case "sniperTank":
                _bigGunTankToggle.value = false;
                _gunnerTankToggle.value = false;
                _machineTankToggle.value = false;
                break;
            case "gunnerTank":
                _bigGunTankToggle.value = false;
                _sniperTankToggle.value = false;
                _machineTankToggle.value = false;
                break;
            case "machineTank":
                _bigGunTankToggle.value = false;
                _sniperTankToggle.value = false;
                _gunnerTankToggle.value = false;
                break;
            case "noneSelected":
                break;
            default:
                break;
        }
    }
    private string GetToggle()
    {
        if (_bigGunTankToggle.value)
        {
            return "bigGunTank";
        } 
        else if (_sniperTankToggle.value)
        {
            return "sniperTank";
        } 
        else if (_gunnerTankToggle.value)
        {
            return "gunnerTank";
        } 
        else if (_machineTankToggle.value)
        {
            return "machineTank";
        } 
        else { return "noneSelected"; }
    }
    void ToggleToInt()
    {
        if (_bigGunTankToggle.value)
        {
            _toggleToInt = 1;
        }
        else if (_gunnerTankToggle.value)
        {
            _toggleToInt = 2;
        }
        else if (_sniperTankToggle.value)
        {
            _toggleToInt = 3;
        } 
        else if (_machineTankToggle.value)
        {
            _toggleToInt = 4;
        }
        else
        {
            _toggleToInt = 0;
        }
    }
        
    // Settings Menu Methods
    
        private void UnloadSettingsMenu()
        {
            _settingsMenu.style.display = DisplayStyle.None;
        }
        private void MusicSlider(){}
        private void EffectsSlider(){}

    // Send Player Info Methods

    public string GetNewPlayerName()
    {
        return _newUsername.value;
    }
    public string GetNewPinCode()
    {
        return _newPinCode.value;
    }
    public string GetLoadPlayerName()
    {
        return _loadUsername.value;
    }
    public string GetLoadPinCode()
    {
        return _loadPinCode.value;
    }
    
    
    // Network Manager HUD

}
