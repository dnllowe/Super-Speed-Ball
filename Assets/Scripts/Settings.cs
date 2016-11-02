using UnityEngine;
 
public class Settings : MonoBehaviour {

    /// <summary>
    /// Enumerator for game control scheme
    /// </summary>
    public enum CONTROL_MODE { PONG, BRICK };

    /// <summary>
    /// enumerator for game control scheme
    /// </summary>
    public static CONTROL_MODE controlMode = CONTROL_MODE.PONG;

    /// <summary>
    /// The closed, BRICK breaker mode arena
    /// </summary>
    public GameObject arenaClosed;

    /// <summary>
    /// The open, PONG mode arena
    /// </summary>
    public GameObject arenaOpen;

    /// <summary>
    /// The bottom / left panel
    /// </summary>
    public GameObject panelTop;

    /// <summary>
    /// The top / right panel (for PONG control mode)
    /// </summary>
    public GameObject panelBottom;

    void Start() {
        switch(controlMode) {
            case CONTROL_MODE.PONG:
                arenaClosed.SetActive(false);
                arenaOpen.SetActive(true);
                panelTop.SetActive(true);
                break;
            case CONTROL_MODE.BRICK:
                arenaClosed.SetActive(true);
                arenaOpen.SetActive(false);
                panelTop.SetActive(false);
                break;
        }
    }
}
