using UnityEngine;

public class BirdBoxManager : MonoBehaviour {
    public GameObject birdBox;
    public RotateObjectBetweenPoints birdBoxRotater;
    public BirdBoxMode bbMode = BirdBoxMode.Rotate;
    public BirdBoxFrame[] birdBoxFrames;
    private bool savedPosition = false;
    private Vector3 birdBoxInitPos;
    public BirdBoxFrame GetBirdBoxFrame(BirdBoxMode mode){
        // Return the frame that matches the mode
        foreach (BirdBoxFrame frame in birdBoxFrames){
            if (frame.mode == mode){
                return frame;
            }
        }
        return null;
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
    }
    private void FixedUpdate(){
        UpdateBirdBox();
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
