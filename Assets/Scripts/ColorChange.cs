using UnityEngine;

public class ColorChange : MonoBehaviour
{
    [SerializeField] Color color;

    private Renderer render;
    private MaterialPropertyBlock propertyBlock;

    // Start is called before the first frame update
    void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        render = GetComponent<Renderer>();

        // Get the current value of the material properties in the renderer.
        render.GetPropertyBlock(propertyBlock);
        // Assign our new value.
        propertyBlock.SetColor("_Color", color);
        // Apply the edited values to the renderer.
        render.SetPropertyBlock(propertyBlock);

        // Sets color of object
        // material.SetColor("_Color", color);
    }

    // Returns color
    public Color ReturnColor()
    {
        return color;
    }
}
