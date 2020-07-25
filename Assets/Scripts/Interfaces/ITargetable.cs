using System;

public interface ITargetable : IMonoBehavior {
    event Action Died;
    
    int Team { get; set; }
}
