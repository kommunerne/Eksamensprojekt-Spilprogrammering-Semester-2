using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Mirror;
using Mono.Data.Sqlite;

public class MainMenuController : NetworkBehaviour
{
    #region Vairbales
    
    
    
    // Main Menu
    
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
        private TextElement _loadUsername;
        private TextElement _loadPinCode;
        
        // Visual Elements
        private VisualElement _loadGameMenu;
        
    // New Game Menu
    
        // Buttons
        private Button _returnFromNewGameMenu;
        private Button _createNewTank;
        
        // Text Elements
        private TextElement _newUsername;
        private TextElement _newPinCode;
        
        // Toggles
        private Toggle _bigGunTankToggle;
        private Toggle _sniperTankToggle;
        private Toggle _gunnerTankToggle;
        private Toggle _machineTankToggle;
        
        // Toggle to int converter

        private int _toggleToInt;
        
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
    
    // NetworkManager Message
        
        
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        #region Instantiating
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
            _loadUsername = root.Q<TextElement>("loadUsername");
            _loadPinCode = root.Q<TextElement>("loadPinCode");
    
            // Visual Elements
            _loadGameMenu = root.Q<VisualElement>("LoadGameMenu");

        // New Game Menu
                    
            // Buttons
            _returnFromNewGameMenu = root.Q<Button>("returnFromNewGameMenu");
            _createNewTank = root.Q<Button>("createNewTank");
                
            // Toggles
            _bigGunTankToggle = root.Q<Toggle>("bigGunTankToggle");
            _sniperTankToggle = root.Q<Toggle>("sniperTankToggle");
            _gunnerTankToggle = root.Q<Toggle>("gunnerTankToggle");
            _machineTankToggle = root.Q<Toggle>("machineTankToggle");
    
            // Visual Elements
            _newGameMenu = root.Q<VisualElement>("NewGameMenu");
                        
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
            SceneManager.LoadScene("GameScene");
        }
        private void UnloadLoadGameMenu()
        {
            _loadGameMenu.style.display = DisplayStyle.None;
        }

    // New Game Menu Methods

    private void LoadNewGame()
    {
        _db.CreatePlayer(_newUsername.text);
        ToggleToInt();
        SceneManager.LoadScene("GameScene");
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

    
}
