using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    #region Vairbales
    // Load Menu Buttons
    private Button _loadLoadGameMenuButton;
    private Button _loadSettingsMenuButton;
    private Button _loadNewGameMenuButton;
    
    // Return Buttons
    private Button _returnFromLoadGameMenu;
    private Button _returnFromSettingsMenu;
    private Button _returnFromNewGameMenu;
    private Button _exitMainMenu;
    
    // Visual Elements
    private VisualElement _mainMenu;
    private VisualElement _loadGameMenu;
    private VisualElement _newGameMenu;
    private VisualElement _settingsMenu;

    // Text Elements

    private TextField _pinCode;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Load Menu Buttons
        _loadLoadGameMenuButton = root.Q<Button>("goToLoadGameMenu");
        _loadSettingsMenuButton = root.Q<Button>("goToSettingsMenu");
        _loadNewGameMenuButton = root.Q<Button>("goToNewGameMenu");
        // Return Buttons
        _returnFromLoadGameMenu = root.Q<Button>("returnFromLoadGameMenu");
        _returnFromSettingsMenu = root.Q<Button>("returnFromSettingsMenu");
        
        // Visual Elements
        _mainMenu = root.Q<VisualElement>("MainMenu");
        _loadGameMenu = root.Q<VisualElement>("LoadGameMenu");
        _settingsMenu = root.Q<VisualElement>("SettingsMenu");
        
        _loadGameMenu.style.display = DisplayStyle.None;
        _settingsMenu.style.display = DisplayStyle.None;
        
        // Text Field
        
        // Methods called on button click
        // ------------------------------
        // Load Menu Buttons

        _loadLoadGameMenuButton.clicked += LoadLoadGameMenu;
        _loadSettingsMenuButton.clicked += LoadSettingsMenu;
        _loadNewGameMenuButton.clicked += LoadGame;
        
        // Return Menu Button

        _returnFromLoadGameMenu.clicked += UnloadLoadGameMenu;
        _returnFromSettingsMenu.clicked += UnloadSettingsMenu;

    }

    // LoadGame Menu Methods
    private void LoadLoadGameMenu()
    {
        _loadGameMenu.style.display = DisplayStyle.Flex;
    }
    private void UnloadLoadGameMenu()
    {
        _loadGameMenu.style.display = DisplayStyle.None;
    }

    // Settings Menu Methods
    private void LoadSettingsMenu()
    {
        _settingsMenu.style.display = DisplayStyle.Flex;
    }
    private void UnloadSettingsMenu()
    {
        _settingsMenu.style.display = DisplayStyle.None;
    }

    void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
        _mainMenu.style.display = DisplayStyle.None;
        
    }
    
}
