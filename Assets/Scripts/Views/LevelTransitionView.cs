using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransitionView : MonoBehaviour {
    private Animation animation;
    private RawImage vignette;
    private RectTransform view;
    private UIController uiController;

    void Awake() {
        animation = GetComponent<Animation>();
        uiController = UIController.Instance;
        view = GetComponent<RectTransform>();
        vignette = GetComponent<RawImage>();

        uiController.StoreUpdated += OnStoreUpdated;
    }

    void Hide() {
        vignette.enabled = false;
    }

    void OnTransitionInComplete() {
        uiController.SetValue("GameState", GameState.Playing);
    }

    void OnTransitionOutComplete() {
        Hide();
    }

    void OnStoreUpdated(string storeKey) {
        if (storeKey == "GameState") {
            if (uiController.Store[storeKey] == GameState.Loading) {
                Show();
            }
        }
    }

    void Show() {
        vignette.enabled = true;
        animation.Play();
    }
}
