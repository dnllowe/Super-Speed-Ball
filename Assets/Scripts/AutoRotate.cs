using UnityEngine;
using System.Collections;

/// <summary>
/// Automatically rotates object in scene. Edit rotation amount in Unity Inspector
/// </summary>
public class AutoRotate : MonoBehaviour {
   
    /// <summary>
    /// How much to rotate the object each frame
    /// </summary>
    public Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);

	// Update is called once per frame
	void FixedUpdate () {
        transform.Rotate(rotation * Time.deltaTime, Space.World);
	}
}
