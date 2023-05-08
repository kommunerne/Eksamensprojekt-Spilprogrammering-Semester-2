using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerUIController : MonoBehaviour
{
    private Button _hpUpgradeButton;
    private Button _dmgUpgradeButton;
    private Button _firerateUpgradeButton;
    private Button _movespeedUpgradeButton;
    private Button _hpregenUpgradeButton;
    private Button hideStats;
    private Button showStats;

    private VisualElement statsHidden;
    private VisualElement statsShown;
    
    
    private ProgressBar hpProgressBar;
    private ProgressBar dmgProgressBar;
    private ProgressBar firerateProgressBar;
    private ProgressBar movespeedProgressBar;
    private ProgressBar hpregenProgressBar;

    private Label currentLevel;
    private Label statusPoints;
    
    public MiniMapTestPlayer player;
    
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Labels
        currentLevel = root.Q<Label>("currentLevel");
        statusPoints = root.Q<Label>("statusPoints");
        
        // Buttons
        _hpUpgradeButton = root.Q<Button>("hpButton");
        _dmgUpgradeButton = root.Q<Button>("dmgButton");
        _firerateUpgradeButton = root.Q<Button>("firerateButton");
        _movespeedUpgradeButton = root.Q<Button>("movespeedButton");
        _hpregenUpgradeButton = root.Q<Button>("hpregenButton");
        hideStats = root.Q<Button>("hideStats");
        showStats = root.Q<Button>("showStats");
        

        // Sliders

        hpProgressBar = root.Q<ProgressBar>("hpSlider");
        dmgProgressBar = root.Q<ProgressBar>("dmgSlider");
        firerateProgressBar = root.Q<ProgressBar>("firerateSlider");
        movespeedProgressBar = root.Q<ProgressBar>("movespeedSlider");
        hpregenProgressBar = root.Q<ProgressBar>("hpregenSlider");

        // Visual Element

        statsShown = root.Q<VisualElement>("PlayerStatsShown");
        statsHidden = root.Q<VisualElement>("PlayerStatsHidden");


        statsShown.style.display = DisplayStyle.Flex;
        statsHidden.style.display = DisplayStyle.None;
        

        hpProgressBar.value = 0f;
        dmgProgressBar.value = 0f;
        firerateProgressBar.value = 0f;
        movespeedProgressBar.value = 0f;
        hpregenProgressBar.value = 0f;

        hideStats.clicked += HideStats;
        showStats.clicked += ShowStats;
        _hpUpgradeButton.clicked += HpUpgrade;
        _dmgUpgradeButton.clicked += DmgUpgrade;
        _firerateUpgradeButton.clicked += FirerateUpgrade;
        _movespeedUpgradeButton.clicked += MovespeedUpgrade;
        _hpregenUpgradeButton.clicked += HpregenUpgrade;

    }

    // Update is called once per frame
    void Update()
    {
        currentLevel.text = player.level.ToString();
        statusPoints.text = player.statPoints.ToString();
        if (player.statPoints == 0)
        {
            statsShown.style.display = DisplayStyle.None;
        }
        
    }

    void HpUpgrade()
    {
        if (hpProgressBar.value < 10 && player.statPoints>0)
        {
            Debug.Log(player.statPoints);
            Debug.Log(player.statPoints.ToString());
            hpProgressBar.value += 1f;
            player.HpUpgrade();
        }
    }
    void DmgUpgrade()
    {
        if (dmgProgressBar.value < 10 && player.statPoints>0)
        {
            dmgProgressBar.value += 1f;
            player.DmgUpgrade();
        }
    }
    void FirerateUpgrade()
    {
        if (firerateProgressBar.value < 10 && player.statPoints>0)
        {
            firerateProgressBar.value += 1f;
            player.FirerateUpgrade();
        }
    }
    void MovespeedUpgrade()
    {
        if (movespeedProgressBar.value < 10 && player.statPoints>0)
        {
            movespeedProgressBar.value += 1f;
            player.MovespeedUpgrade();
        }
    }
    void HpregenUpgrade()
    {
        if (hpregenProgressBar.value < 10 && player.statPoints>0)
        {
            hpregenProgressBar.value += 1f;
            player.HpregenUpgrade();
        }
    }

    void HideStats()
    {
        statsShown.style.display = DisplayStyle.None;
        statsHidden.style.display = DisplayStyle.Flex;
    }

    void ShowStats()
    {
        statsShown.style.display = DisplayStyle.Flex;
        statsHidden.style.display = DisplayStyle.None;
    }
}
