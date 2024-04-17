using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string promt { get; }
    public bool Interact(Interactor interactor);
}
