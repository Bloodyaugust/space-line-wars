using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsView : MonoBehaviour {
    private bool shown;
    private Button mainMenuButton;
    private RectTransform view;
    private TextMeshProUGUI playerResultText;
    private TextMeshProUGUI playerResultsBreakdown;
    private UIController uiController;

    void Awake() {
        mainMenuButton = transform.Find("MainMenuButton").GetComponent<Button>();
        playerResultText = transform.Find("ResultText").GetComponent<TextMeshProUGUI>();
        playerResultsBreakdown = transform.Find("ResultsBreakdown").GetComponent<TextMeshProUGUI>();
        uiController = UIController.Instance;
        view = GetComponent<RectTransform>();

        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        uiController.StoreUpdated += OnStoreUpdated;
    }

    void Hide() {
        view.DOAnchorPos(new Vector2(0, -800), 0.2f);
        shown = false;
    }

    void OnMainMenuButtonClicked() {
        uiController.SetValue("GameState", GameState.Menu);
    }

    void OnStoreUpdated(string storeKey) {
        if (storeKey == "GameState") {
            if (uiController.Store[storeKey] == GameState.Over && shown == false) {
                Show();
            }

            if (uiController.Store[storeKey] != GameState.Over && shown == true) {
                Hide();
            }
        }
    }

    void Show() {
        view.DOAnchorPos(new Vector2(0, 0), 0.2f);
        shown = true;

        if (uiController.Store["DestroyedBaseNodes"][0]) {
            playerResultText.text = "You lost!";
        } else {
            playerResultText.text = "You won!";
        }

        playerResultsBreakdown.text = $@"
        - Kills: {uiController.Store["Kills"][0]}
        - Resources Gained: {uiController.Store["ResourcesGained"][0].ToString("N0")}
        - Techs Researched: {uiController.Store["TechResearched"][0]}";
    }

    void Start() {
        Hide();
    }
}
