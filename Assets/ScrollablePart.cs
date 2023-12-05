using UnityEngine;

public class ScrollablePart: MonoBehaviour
{
    protected Vector3 bounds;

    public void Init()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (col == null)
        {
            return;
        }

        bounds = col.bounds.size;
    }

    public float GetLength()
    {
        var l = bounds != null ? bounds.x : 15f;
        return l;
    }

    public void Move(float speed)
    {
        transform.position += new Vector3(0f, speed, 0f);
        Vector3 newPos = transform.position + new Vector3(speed, 0f, 0f);
    }

}