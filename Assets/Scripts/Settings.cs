using UnityEngine;
 
public class Settings : MonoBehaviour {
    public int targetFrameRate = 60;

    void Awake() {
        Application.targetFrameRate = targetFrameRate;
    }
}
