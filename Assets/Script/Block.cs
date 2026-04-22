using UnityEngine;

public enum BlockColor
{
    Red,
    Green,
    Blue,
    Yellow
}
 
public class Block : MonoBehaviour
{
    public BlockColor blockColor;

    public void CheckColor(BlockColor laserColor)
    {
        if (blockColor == laserColor)
        {
            Explode();
        }
    }

    void Explode()
    {
        Destroy(gameObject);
    }
}