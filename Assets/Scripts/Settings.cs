using UnityEngine;
 
public class Settings : MonoBehaviour {
    public int targetFrameRate = 30;

    void Awake() {
        Application.targetFrameRate = targetFrameRate;
    }
}
