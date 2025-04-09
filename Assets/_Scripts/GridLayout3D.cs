using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Places child objects in a simple grid layout in 3D space.
/// Attach this script to a parent GameObject that contains child objects.
/// </summary>
public class GridLayout3D : MonoBehaviour
{
    [Tooltip("How many columns the grid should have.")]
    public int columns = 3;
    
    [Tooltip("Width spacing between each column.")]
    public float cellWidth = 2.0f;
    
    [Tooltip("Height spacing between each row.")]
    public float cellHeight = 2.0f;
    
    [Tooltip("Optionally offset the first cell in the X axis.")]
    public float offsetX = 0.0f;
    
    [Tooltip("Optionally offset the first cell in the Y axis (vertical).")]
    public float offsetY = 0.0f;
    
    [Tooltip("Optionally offset the first cell in the Z axis (depth).")]
    public float offsetZ = 0.0f;

    // You can update in real-time if desired, or move this to Start() if you only
    // need the grid arranged once. If you do it in Update, it'll keep re-arranging
    // if children are added/removed or if parameters change during play.
    private void Update()
    {
        LayoutChildren();
    }
    
    [Button]
    private void LayoutChildren()
    {
        int childIndex = 0;
        
        foreach (Transform child in transform)
        {
            // Calculate the row and column for each child
            int row = childIndex / columns;
            int col = childIndex % columns;
            
            // Calculate positions based on row/column
            float xPos = offsetX + (col * cellWidth);
            float yPos = offsetY; // If you want to stack vertically, you can adapt similarly to xPos
            float zPos = offsetZ + (row * cellHeight); // Using z for "rows", but you can adjust orientation
            
            // Apply local position relative to the parent
            child.localPosition = new Vector3(xPos, yPos, zPos);
            
            childIndex++;
        }
    }
}