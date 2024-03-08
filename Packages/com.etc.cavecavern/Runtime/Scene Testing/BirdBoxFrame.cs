using System;
using UnityEngine;
using UnityEngine.InputSystem;
// Todo: Make input system ref optional
[Serializable]
public class BirdBoxFrame : MonoBehaviour {
    public BirdBoxMode Mode;
    public InputActionReference MoveBoxToFrame;
    // Event for when the input action is performed
    public event Action<BirdBoxMode> UpdateBirdBoxMode;
    private void OnEnable()
    {
        MoveBoxToFrame.action.Enable();
        MoveBoxToFrame.action.performed += context => UpdateBirdBoxMode?.Invoke(Mode);

    }
    private void OnDisable()
    {
        MoveBoxToFrame.action.Disable();
        MoveBoxToFrame.action.performed -= context => UpdateBirdBoxMode?.Invoke(Mode);
    }
}
