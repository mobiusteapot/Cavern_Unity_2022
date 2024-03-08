using UnityEngine;
using UnityEngine.InputSystem;

public class BirdBoxManager : MonoBehaviour {
    public GameObject birdBox;
    public GameObject audioListener;
    public bool attachListenerToBirdBox = false;
    public RotateObjectBetweenPoints birdBoxRotater;
    public BirdBoxMode bbMode = BirdBoxMode.Rotate;
    public BirdBoxFrame[] birdBoxFrames;
    private bool savedPosition = false;
    private Vector3 birdBoxInitPos;
    private Vector3 audioListenerInitPos;
    public InputActionReference SetModeToRotate;
    private bool attachChord = false;
    public InputActionReference AttachToListenerChord;
    
    public BirdBoxFrame GetBirdBoxFrame(BirdBoxMode mode){
        // Return the frame that matches the mode
        foreach (BirdBoxFrame frame in birdBoxFrames){
            if (frame.Mode == mode){
                return frame;

            }
        }
        return null;
    }
    public void SetBirdBoxMode(BirdBoxMode mode)
    {
        Debug.Log("MoveBoxToFrame_2");
        bbMode = mode;
        UpdateBirdBox();
    }
    public void SetListenerAndBBMode(BirdBoxMode mode)
    {
        attachListenerToBirdBox = attachChord;
        if (mode == BirdBoxMode.Rotate) {
            attachListenerToBirdBox = false;
        }
        bbMode = mode;
        UpdateBirdBox();
        audioListener.transform.position = attachListenerToBirdBox ? birdBox.transform.position : audioListenerInitPos;
    }
    private void OnValidate() {
        if (birdBox == null) return;
        if (birdBoxRotater == null) return;
        
        birdBoxRotater.ObjectToRotate = birdBox;
        UpdateBirdBox();

    }
    private void Awake()
    {
        birdBoxInitPos = birdBox.transform.localPosition;
        audioListenerInitPos = audioListener.transform.position;
    }
    private void OnEnable()
    {
        SetModeToRotate.action.Enable();
        SetModeToRotate.action.performed += context => SetListenerAndBBMode(BirdBoxMode.Rotate);
        AttachToListenerChord.action.Enable();
        AttachToListenerChord.action.performed += context => attachChord = true;
        AttachToListenerChord.action.canceled += context => attachChord = false;


        foreach (BirdBoxFrame frame in birdBoxFrames)
        {
            frame.UpdateBirdBoxMode += SetListenerAndBBMode;
        }
    }
    private void OnDisable()
    {
        SetModeToRotate.action.Disable();
        SetModeToRotate.action.performed -= context => SetListenerAndBBMode(BirdBoxMode.Rotate);
        AttachToListenerChord.action.Disable();
        AttachToListenerChord.action.performed -= context => attachChord = true;
        AttachToListenerChord.action.canceled -= context => attachChord = false;

        foreach (BirdBoxFrame frame in birdBoxFrames)
        {
            frame.UpdateBirdBoxMode -= SetListenerAndBBMode;
        }
    }
    private void FixedUpdate(){
        UpdateBirdBox();
    }
    private void OnGUI()
    {
        var style = new GUIStyle();
        style.fontSize = 96;
        // Todo: stereo friendly debug text function
        string debugText = "BirdBoxMode: " + bbMode.ToString() + "\n" + "Attached to listener: " + attachListenerToBirdBox;
        GUI.Label(new Rect(10, 10, 200, 10), debugText, style);
        GUI.Label(new Rect(10, 10 + 1080, 200, 10), debugText, style);

    }

    public void UpdateBirdBox(){
        if (bbMode == BirdBoxMode.Rotate){
            birdBoxRotater.enabled = true;
            birdBoxRotater.ObjectToRotate = birdBox;
            if (savedPosition) {
                birdBox.transform.localPosition = birdBoxInitPos;
                savedPosition = false;
            }

            return;
        } else {
            birdBoxRotater.enabled = false;
            birdBoxRotater.StopRotation();
            savedPosition = true;
        }
        var frame = GetBirdBoxFrame(bbMode);
        if (frame != null){
            birdBox.transform.position = frame.transform.position;
            birdBox.transform.rotation = frame.transform.rotation;
        } else {
            Debug.LogError("No frame found for mode: " + bbMode);
        }
    }
}
