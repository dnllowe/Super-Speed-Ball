using UnityEngine;

public class oFunctions : MonoBehaviour {

    /// <summary>
    /// Used to assigned currentCamera to static object in Unity Inspector
    /// </summary>
    public Camera currentCameraInput;

    /// <summary>
    /// Current camera for game
    /// </summary>
    public static Camera currentCamera;

    /// <summary>
    /// Converts a touch to game world coordinates at z-depth of target
    /// </summary>
    /// <param name="touch">The touch to convert</param>
    /// <param name="target">The z-depth for where the touch should intersect in 3D game space</param>
    public static Vector3 ConvertTouchToGameCoordinates(Touch touch, GameObject target) {

        // Convert touch location to ray
        var ray = currentCamera.ScreenPointToRay(new Vector3(touch.position.x,
            touch.position.y, 0));

        // Convert ray to vector at plane that intersects object
        var destination = ray.GetPoint(
            Vector3.Distance(currentCamera.transform.position, target.transform.position));

        return destination;
    }

    /// <summary>
    /// Converts a touch to game world coordinates at z-depth of target
    /// </summary>
    /// <param name="mouse">The mouse position to convert</param>
    /// <param name="target">The z-depth for where the touch should intersect in 3D game space</param>
    public static Vector3 ConvertMouseToGameCoordinates(Vector3 mouse, GameObject target) {
        // Convert touch location to ray
        var ray = currentCamera.ScreenPointToRay(new Vector3(mouse.x, mouse.y, 0));

        // Convert ray to vector at plane that intersects object
        var destination = ray.GetPoint(
            Vector3.Distance(currentCamera.transform.position, target.transform.position));

        return destination;
    }
	// Use this for initialization
	void Start () {
        currentCamera = currentCameraInput;
	}
}
