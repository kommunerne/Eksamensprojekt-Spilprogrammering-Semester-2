using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerUIController : NetworkBehaviour
{
    // Stats HUD
    
        // Buttons
        private Button _hpUpgradeButton;
        private Button _dmgUpgradeButton;
        private Button _firerateUpgradeButton;
        private Button _movespeedUpgradeButton;
        private Button _hpregenUpgradeButton;
        private Button _hideStats;
        private Button _showStats;
    
        // Visual Elements
        private VisualElement _statsHidden;
        private VisualElement _statsShown;
        
        // Progress Bars
        private ProgressBar _hpProgressBar;
        private ProgressBar _dmgProgressBar;
        private ProgressBar _firerateProgressBar;
        private ProgressBar _movespeedProgressBar;
        private ProgressBar _hpregenProgressBar;
        
        // Labels
        private Label _currentLevel;
        private Label _statusPoints;

    // Death Screen
        
        // Buttons
        private Button _deathContinue;
    
        // VisualElements
        private VisualElement _deathScreen;
        
        // Labels
        private Label _playerScore;
        private Label _playerLevel;
        private Label _playerTimeAlive;
        
    // Player
    
        // Player Script
        public PlayerController player;
        
        // Visual Elements
        private VisualElement _healthBarContainer;
    
        // Progress Bars
        private ProgressBar _healthBar;
    
    // Main UI
    
        // Visual Elements
        private VisualElement _uiScreen;
        
        
    // Esc Screen
    
        // Buttons
        private Button _saveButton;
        private Button _closeButton;
        
        // Visual Elements
        private VisualElement _escScreen;
        
        // Labels

        private Label _playerName;

    // GameObjects and GameManager
    
        private GameObject _managerGameObject;
        private DBScript _db;
    
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _managerGameObject = GameObject.Find(nameof(GunnerNetworkManager));
        _db = _managerGameObject.GetComponent<DBScript>();
        
        if (!isLocalPlayer)
        {
             // Labels
            _currentLevel = root.Q<Label>("currentLevel");
            _statusPoints = root.Q<Label>("statusPoints");
        
            // Buttons
            _hpUpgradeButton = root.Q<Button>("hpButton");
            _dmgUpgradeButton = root.Q<Button>("dmgButton");
            _firerateUpgradeButton = root.Q<Button>("firerateButton");
            _movespeedUpgradeButton = root.Q<Button>("movespeedButton");
            _hpregenUpgradeButton = root.Q<Button>("hpregenButton");
            _hideStats = root.Q<Button>("hideStats");
            _showStats = root.Q<Button>("showStats");
        
            // Sliders
            _hpProgressBar = root.Q<ProgressBar>("hpSlider");
            _dmgProgressBar = root.Q<ProgressBar>("dmgSlider");
            _firerateProgressBar = root.Q<ProgressBar>("firerateSlider");
            _movespeedProgressBar = root.Q<ProgressBar>("movespeedSlider");
            _hpregenProgressBar = root.Q<ProgressBar>("hpregenSlider");

            // Visual Element
            _statsShown = root.Q<VisualElement>("PlayerStatsShown");
            _statsHidden = root.Q<VisualElement>("PlayerStatsHidden");
            
        // Death Screen
        
            // Buttons
            _deathContinue = root.Q<Button>("continueButton");
        
            // Labels
            _playerScore = root.Q<Label>("playerScore");
            _playerLevel = root.Q<Label>("playerLevel");
            _playerTimeAlive = root.Q<Label>("playerTimeAlive");
            
            // Visual Elements
            _deathScreen = root.Q<VisualElement>("DeathScreenContainer");
        
        // Player
            // Visual Elements
            _healthBarContainer = root.Q<VisualElement>("PlayerHealthbar");
            
            // Progress Bars
            _healthBar = root.Q<ProgressBar>("healthbar");
        
        // Main UI

            // Visual Elements
            _uiScreen = root.Q<VisualElement>("MainScreen");
            
        
        // Esc Screen
        
            // Buttons
            _saveButton = root.Q<Button>("saveButton");
            _closeButton = root.Q<Button>("closeButton");
            
            
            // Visual Elements
            _escScreen = root.Q<VisualElement>("EscScreenContainer");
            
            // Labels

            _playerName = root.Q<Label>("playerName");


            // On Start Values
        _playerName.text = player.playerName;
        
        _statsShown.style.display = DisplayStyle.None;
        _statsHidden.style.display = DisplayStyle.None;
        _uiScreen.style.display = DisplayStyle.None;
        _deathScreen.style.display = DisplayStyle.None;
        _escScreen.style.display = DisplayStyle.None;
        _healthBarContainer.style.display = DisplayStyle.None;
        return;
        }

        #region Instantiation

        // Stats HUD
        
            // Labels
            _currentLevel = root.Q<Label>("currentLevel");
            _statusPoints = root.Q<Label>("statusPoints");
        
            // Buttons
            _hpUpgradeButton = root.Q<Button>("hpButton");
            _dmgUpgradeButton = root.Q<Button>("dmgButton");
            _firerateUpgradeButton = root.Q<Button>("firerateButton");
            _movespeedUpgradeButton = root.Q<Button>("movespeedButton");
            _hpregenUpgradeButton = root.Q<Button>("hpregenButton");
            _hideStats = root.Q<Button>("hideStats");
            _showStats = root.Q<Button>("showStats");
        
            // Sliders
            _hpProgressBar = root.Q<ProgressBar>("hpSlider");
            _dmgProgressBar = root.Q<ProgressBar>("dmgSlider");
            _firerateProgressBar = root.Q<ProgressBar>("firerateSlider");
            _movespeedProgressBar = root.Q<ProgressBar>("movespeedSlider");
            _hpregenProgressBar = root.Q<ProgressBar>("hpregenSlider");

            // Visual Element
            _statsShown = root.Q<VisualElement>("PlayerStatsShown");
            _statsHidden = root.Q<VisualElement>("PlayerStatsHidden");
            
        // Death Screen
        
            // Buttons
            _deathContinue = root.Q<Button>("continueButton");
        
            // Labels
            _playerScore = root.Q<Label>("playerScore");
            _playerLevel = root.Q<Label>("playerLevel");
            _playerTimeAlive = root.Q<Label>("playerTimeAlive");
            
            // Visual Elements
            _deathScreen = root.Q<VisualElement>("DeathScreenContainer");
        
        // Player
            // Visual Elements
            _healthBarContainer = root.Q<VisualElement>("PlayerHealthbar");
            
            // Progress Bars
            _healthBar = root.Q<ProgressBar>("healthbar");
        
        // Main UI

            // Visual Elements
            _uiScreen = root.Q<VisualElement>("MainScreen");
            
        
        // Esc Screen
        
            // Buttons
            _saveButton = root.Q<Button>("saveButton");
            _closeButton = root.Q<Button>("closeButton");
            
            
            // Visual Elements
            _escScreen = root.Q<VisualElement>("EscScreenContainer");
            
            // Labels

            _playerName = root.Q<Label>("playerName");

            #endregion
        
        
        // On Start Values
        _playerName.text = player.playerName;
        
        _statsShown.style.display = DisplayStyle.Flex;
        _statsHidden.style.display = DisplayStyle.None;
        _uiScreen.style.display = DisplayStyle.Flex;
        _deathScreen.style.display = DisplayStyle.None;
        _escScreen.style.display = DisplayStyle.None;

        _hpProgressBar.value = 0f;
        _dmgProgressBar.value = 0f;
        _firerateProgressBar.value = 0f;
        _movespeedProgressBar.value = 0f;
        _hpregenProgressBar.value = 0f;
        _healthBarContainer.style.display = DisplayStyle.None;

        // On Buttons Clicked
            
            // Stats HUD
            _hideStats.clicked += HideStats;
            _showStats.clicked += ShowStats;
            _hpUpgradeButton.clicked += HpUpgrade;
            _dmgUpgradeButton.clicked += DmgUpgrade;
            _firerateUpgradeButton.clicked += FirerateUpgrade;
            _movespeedUpgradeButton.clicked += MovespeedUpgrade;
            _hpregenUpgradeButton.clicked += HpregenUpgrade;
            
            // Death Screen
            _deathContinue.clicked += ContinueToMainMenu;

            // Esc Screen
            _saveButton.clicked += SaveGame;
            _closeButton.clicked += CloseEscScreen;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;
        
        UpdatePlayerHealth();
        
        if(Input.GetKeyDown(KeyCode.Escape)&& isLocalPlayer)
            ShowEscScreen();
        
        _currentLevel.text = player.level.ToString();
        _statusPoints.text = player.statPoints.ToString();
        if (player.statPoints == 0)
        {
            _statsShown.style.display = DisplayStyle.None;
        }

        if (player.playerGotHit)
        {
            _healthBarContainer.style.display = DisplayStyle.Flex;
        }
        else
        {
            HideHealth();
        }
    }

    // Methods
    
    #region Stats HUD
    
    void HpUpgrade()
    {
        if (_hpProgressBar.value < 10 && player.statPoints>0)
        {
            Debug.Log(player.statPoints);
            Debug.Log(player.statPoints.ToString());
            _hpProgressBar.value += 1f;
            player.CmdUpgradeHp();
        }
    }
    // [ClientRpc]
    void DmgUpgrade()
    {
        if (_dmgProgressBar.value < 10 && player.statPoints>0)
        {
            _dmgProgressBar.value += 1f;
            player.CmdUpgradeDmg();
        }
    }
    // [ClientRpc]
    void FirerateUpgrade()
    {
        if (_firerateProgressBar.value < 10 && player.statPoints>0)
        {
            _firerateProgressBar.value += 1f;
            player.CmdUpgradeFireRate();
        }
    }
    // [ClientRpc]
    void MovespeedUpgrade()
    {
        if (_movespeedProgressBar.value < 10 && player.statPoints>0)
        {
            _movespeedProgressBar.value += 1f;
            player.CmdUpgradeMoveSpeed();
        }
    }
    // [ClientRpc]
    void HpregenUpgrade()
    {
        if (_hpregenProgressBar.value < 10 && player.statPoints>0)
        {
            _hpregenProgressBar.value += 1f;
            player.CmdUpgradeHpRegen();
        }
    }
    // [ClientRpc]
    void HideStats()
    {
        _statsShown.style.display = DisplayStyle.None;
        _statsHidden.style.display = DisplayStyle.Flex;
    }
    // [ClientRpc]
    void ShowStats()
    {
        _statsShown.style.display = DisplayStyle.Flex;
        _statsHidden.style.display = DisplayStyle.None;
    }
    

    #endregion
    
    #region Player
    [Client]
    void HideHealth()
    {
        _healthBarContainer.style.display = DisplayStyle.None;
    }
    [Client]
    void UpdatePlayerHealth()
    {
        _healthBar.highValue = player.maxHp;
        _healthBar.value = player.currentHp;

    }
    #endregion

    #region Death Screen
    [Client]
    public void DeathScreen()
    {
        if(isLocalPlayer)
        {
            _uiScreen.style.display = DisplayStyle.None;
            _deathScreen.style.display = DisplayStyle.Flex;
        }
    }
    
    void ContinueToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    #endregion

    #region Esc Screen

    void SaveGame()
    {
        if(isLocalPlayer)
        { 
            Player playerToSave = new Player(player.playerName, player.pinCode, player.prefabNr, player.level, player.exp,
            player.score);
            _db.UpdatePlayer(player.playerName,player.pinCode,playerToSave);
            if(isClient)
                NetworkManager.singleton.StopClient();
        }
    }

    // [ClientRpc]
    void ShowEscScreen()
    {
        _escScreen.style.display = DisplayStyle.Flex;
    }
    // [ClientRpc]
    void CloseEscScreen()
    {
        _escScreen.style.display = DisplayStyle.None;
    }
    #endregion
    
}
