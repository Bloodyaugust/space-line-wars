using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransitionView : MonoBehaviour {
    public string MapPath;

    private Animation animation;
    private RawImage vignette;
    private RectTransform view;
    private UIController uiController;

    IEnumerator SceneSwitch() {
        AsyncOperation unload = SceneManager.UnloadSceneAsync(MapPath);

        yield return unload;

        AsyncOperation load = SceneManager.LoadSceneAsync(MapPath, LoadSceneMode.Additive);

        yield return load;

        animation["LevelTransitionView"].speed = 1;
    }

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
        animation["LevelTransitionView"].speed = 0;
    
        StartCoroutine("SceneSwitch");
    }

    void OnTransitionOutComplete() {
        Hide();
        uiController.SetValue("GameState", GameState.Playing);
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
