using UnityEngine;

public class BirdBoxManager : MonoBehaviour {
    public GameObject birdBox;
    public BirdBoxRotater birdBoxRotater;
    public BirdBoxMode bbMode = BirdBoxMode.Rotate;
    public BirdBoxFrame[] birdBoxFrames;
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
        if (birdBoxRotater != null)
            birdBoxRotater.ObjectToRotate = birdBox;
        UpdateBirdBox();

    }
    private void FixedUpdate(){
        UpdateBirdBox();
    }

    public void UpdateBirdBox(){
        if(bbMode == BirdBoxMode.Rotate){
            birdBoxRotater.enabled = true;
            birdBoxRotater.ObjectToRotate = birdBox;
            return;
        } else {
            birdBoxRotater.enabled = false;
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
