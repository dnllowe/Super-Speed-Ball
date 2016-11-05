using UnityEngine;

/// <summary>
/// Methods for modifying an object's material color
/// </summary>
public class ColorModifier : MonoBehaviour {

    /// <summary>
    /// Color of object (Vector3)
    /// </summary>
    Color color;

    /// <summary>
    /// Set alpha value for material color
    /// </summary>
    /// <param name="alphaInput">The new alpha value</param>
    public void SetAlpha(float alphaInput) {
        color.a = alphaInput;
        GetComponent<Renderer>().material.color = color;
    }

    /// <summary>
    /// Set RGB and alpha values for material color. If value is not passed, will use current color
    /// </summary>
    /// <param name="r">The new red value</param>
    /// <param name="g">The new green value</param>
    /// <param name="b">The new blue value</param>
    /// <param name="a">The new alpha value</param>
    public void SetRGBA(float r = -1.0f, float g = -1.0f, float b = -1.0f, float a = -1.0f) {

        if(r != -1.0f) {
            color.r = r;
        }

        if(g != -1.0f) {
            color.g = g;
        }

        if(b != -1.0f) {
            color.b = b;
        }

        if(a != -1.0f) {
            color.a = a;
        }

        GetComponent<Renderer>().material.color = color;
    }

    // Get color values at start
    void Start () {
        color = GetComponent<Renderer>().material.color;
    }
}
