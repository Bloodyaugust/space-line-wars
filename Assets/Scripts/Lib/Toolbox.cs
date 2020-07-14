using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Toolbox : Singleton<Toolbox> {
	protected Toolbox () {}

	void Awake () {
		
	}

	static public T RegisterComponent<T> () where T: Component {
		return Instance.GetOrAddComponent<T>();
	}
}

[System.Serializable]
public class IntEvent : UnityEvent<int> {}
