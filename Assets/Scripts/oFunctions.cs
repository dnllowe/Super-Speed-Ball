using UnityEngine;
using System.Text;

public class oFunctions : MonoBehaviour {

    /// <summary>
    /// Current camera for game
    /// </summary>
    public static Camera overheadCamera;

    /// <summary>
    /// Converts a touch to game world coordinates at z-depth of target
    /// </summary>
    /// <param name="touch">The touch to convert</param>
    /// <param name="target">The z-depth for where the touch should intersect in 3D game space</param>
    public static Vector3 ConvertTouchToGameCoordinates(Touch touch, GameObject target) {

        // Convert touch location to ray
        var ray = overheadCamera.ScreenPointToRay(new Vector3(touch.position.x,
            touch.position.y, 0));

        // Convert ray to vector at plane that intersects object
        var destination = ray.GetPoint(
            Vector3.Distance(overheadCamera.transform.position, target.transform.position));

        return destination;
    }

    /// <summary>
    /// Converts a touch to game world coordinates at z-depth of target
    /// </summary>
    /// <param name="mouse">The mouse position to convert</param>
    /// <param name="target">The z-depth for where the touch should intersect in 3D game space</param>
    public static Vector3 ConvertMouseToGameCoordinates(Vector3 mouse, GameObject target) {
        // Convert touch location to ray
        var ray = overheadCamera.ScreenPointToRay(new Vector3(mouse.x, mouse.y, 0));

        // Convert ray to vector at plane that intersects object
        var destination = ray.GetPoint(
            Vector3.Distance(overheadCamera.transform.position, target.transform.position));

        return destination;
    }

    /// <summary>
    /// Converts a screen coordinate to game world coordinates at z-depth of target
    /// </summary>
    /// <param name="x">The x screen coordinate to convert</param>
    /// /// <param name="y">The y screen coordinate to convert</param>
    /// <param name="target">The z-depth for where the touch should intersect in 3D game space</param>
    public static Vector3 ConvertScreenToGameCoordinates(float x, float y, GameObject target) {
        
        // Convert touch location to ray
        var ray = overheadCamera.ScreenPointToRay(new Vector3(x, y, 0));

        // Convert ray to vector at plane that intersects object
        var destination = ray.GetPoint(
            Vector3.Distance(overheadCamera.transform.position, target.transform.position));

        return destination;
    }

    /// <summary>
    /// Converts a screen coordinate to game world coordinates at z-depth of target
    /// </summary>
    /// <param name="screenCoordinates">The screen coordinates to convert</param>
    /// <param name="target">The z-depth for where the touch should intersect in 3D game space</param>
    public static Vector3 ConvertScreenToGameCoordinates(Vector3 screenCoordinates, GameObject target) {

        // Convert touch location to ray
        var ray = overheadCamera.ScreenPointToRay(screenCoordinates);

        // Convert ray to vector at plane that intersects object
        var destination = ray.GetPoint(
            Vector3.Distance(overheadCamera.transform.position, target.transform.position));

        return destination;
    }
    /// <summary>
    /// Finds an inactive game object using active parent (inactive objects are not return with GameObject.Find functions)
    /// </summary>
    /// <param name="parentTag">The active parent tag</param>
    /// <param name="childTag">The tag for the inactive child</param>
    public static GameObject FindInactiveChild(string parentTag, string childTag) {
        GameObject parentObject = GameObject.FindGameObjectWithTag(parentTag);
        GameObject childObject = null;

        for (int iii = 0; iii < parentObject.transform.childCount; iii++) {
            var transform = parentObject.transform.GetChild(iii);

            if(transform.gameObject.CompareTag(childTag)) {
                childObject = transform.gameObject;
                break;
            }
        }
        return childObject;
    }

    // Use this for initialization
    void Start() {
        var cameras = Camera.allCameras;

        for (int iii = 0; iii < cameras.Length; iii++) {
            if (cameras[iii].CompareTag("overhead")) {
                overheadCamera = cameras[iii];
                break;
            }
        }
    }
}
