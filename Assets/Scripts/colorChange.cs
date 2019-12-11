using UnityEngine;

public class colorChange : MonoBehaviour
{
    [SerializeField] Color color;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    // Start is called before the first frame update
    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();

        // Get the current value of the material properties in the renderer.
        _renderer.GetPropertyBlock(_propBlock);
        // Assign our new value.
        _propBlock.SetColor("_Color", color);
        // Apply the edited values to the renderer.
        _renderer.SetPropertyBlock(_propBlock);

        // Sets color of object
        // material.SetColor("_Color", color);
    }

    // Returns color
    public Color ReturnColor()
    {
        return color;
    }
}
