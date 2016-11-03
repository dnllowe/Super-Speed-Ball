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
    GameObject arenaClosed;

    /// <summary>
    /// The open, PONG mode arena
    /// </summary>
    GameObject arenaOpen;

    /// <summary>
    /// The bottom / left panel
    /// </summary>
    GameObject panelTop;

    /// <summary>
    /// The top / right panel (for PONG control mode)
    /// </summary>
    GameObject panelBottom;

    void Start() {
        arenaOpen = GameObject.FindGameObjectWithTag("arenaOpen");
        arenaClosed = oFunctions.FindInactiveChild("inactive", "arenaClosed");
        panelBottom = GameObject.FindGameObjectWithTag("panelBottom");
        panelTop = GameObject.FindGameObjectWithTag("panelTop");

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
