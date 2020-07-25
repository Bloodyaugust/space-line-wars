using System;
using UnityEngine;

public interface IMonoBehavior {
  GameObject gameObject { get; }
  Transform transform { get; }
}
