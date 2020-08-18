using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour {
    private bool shown;
    private Button playButton;
    private RectTransform view;
    private UIController uiController;

    public void OnDiscordButtonClicked() {
        Application.OpenURL("https://discord.gg/9vYv6Ck");
    }

    public void OnItchButtonClicked() {
        Application.OpenURL("https://synsugarstudio.itch.io/space-line-wars/community");
    }

    public void OnTwitterButtonClicked() {
        Application.OpenURL("https://twitter.com/Bloodyaugust");
    }

    void Awake() {
        playButton = transform.Find("PlayButton").GetComponent<Button>();
        uiController = UIController.Instance;
        view = GetComponent<RectTransform>();

        playButton.onClick.AddListener(OnPlayButtonClicked);
        uiController.StoreUpdated += OnStoreUpdated;
    }

    void Hide() {
        view.DOAnchorPos(new Vector2(0, -800), 0.2f);
        shown = false;
    }

    void OnPlayButtonClicked() {
        uiController.ResetStore();
        uiController.SetValue("GameState", GameState.Loading);
    }

    void OnStoreUpdated(string storeKey) {
        if (storeKey == "GameState") {
            if (uiController.Store[storeKey] == GameState.Menu && shown == false) {
                Show();
            }

            if (uiController.Store[storeKey] != GameState.Menu && shown == true) {
                Hide();
            }
        }
    }

    void Show() {
        view.DOAnchorPos(new Vector2(0, 0), 0.2f);
        shown = true;
    }

    void Start() {
        shown = true;
    }
}
