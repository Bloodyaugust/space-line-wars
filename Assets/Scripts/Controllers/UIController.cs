using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Menu,
    Loading,
    Playing,
    Over
}

public class UIController : Singleton<UIController> {
	protected UIController () {}

    public event Action<string> StoreUpdated;

    public Dictionary<string, dynamic> Store;

	static public T RegisterComponent<T> () where T: Component {
		return Instance.GetOrAddComponent<T>();
	}

    public void ResetStore() {
        Store = new Dictionary<string, dynamic>() {
            {"CompletedResearch", new Dictionary<int, List<SOResearch>>() { {0, new List<SOResearch>()}, {1, new List<SOResearch>()} }},
            {"DestroyedBaseNodes", new bool[2]},
            {"GameState", GameState.Menu},
            {"ProductionNodes", new float[2]},
            {"ResourceRate", new float[2]},
            {"Selection", null},
            {"TooltipItem", null}
        };
    }

    public void SetValue(string key, dynamic value) {
        Store[key] = value;

        StoreUpdated?.Invoke(key);
    }

    public void UpdateValue(string key) {
        StoreUpdated?.Invoke(key);
    }

    void Awake() {
        SceneManager.LoadScene("Scenes/Basic1v1", LoadSceneMode.Additive);
        DontDestroyOnLoad(gameObject);
        ResetStore();
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonUp(1)) {
            SetValue("Selection", null);
        }
    }
}
