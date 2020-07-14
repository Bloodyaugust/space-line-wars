using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIController : Singleton<UIController> {
	protected UIController () {}

    public event Action<string> StoreUpdated;

    public Dictionary<string, dynamic> Store = new Dictionary<string, dynamic>() {
        {"ProductionNodes", new float[2]},
        {"ResourceRate", new float[2]}
    };

	static public T RegisterComponent<T> () where T: Component {
		return Instance.GetOrAddComponent<T>();
	}

    public void SetValue(string key, dynamic value) {
        Store[key] = value;

        StoreUpdated?.Invoke(key);
    }

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
