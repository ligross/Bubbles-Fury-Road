using UnityEngine;
using System.Collections.Generic;

public class MoveCamera : MonoBehaviour
{
    public float speed = 0.1F;
    private bool isMovable;

    Dictionary<RelaxBubbleSize, float[]> sizeToBorders = new Dictionary<RelaxBubbleSize, float[]> {
        { RelaxBubbleSize.Big, new float[] { 1.5f, 4f } },
        { RelaxBubbleSize.Normal, new float[] { 1.8f, 5 } },
        { RelaxBubbleSize.Small, new float[] { 3f, 7 } },
        { RelaxBubbleSize.SuperSmall, new float[] { 3.3f, 7.8f } }
        };

    public void SetMovable(bool isMovable)
    {
        this.isMovable = isMovable;
    }

    void Update()
    {
        if (isMovable && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 minCoords = RelaxManager.Instance._gridManager.minCoords;
            Vector2 maxCoords = RelaxManager.Instance._gridManager.maxCoords;
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            if (transform.position.x + (-touchDeltaPosition.x * speed) < maxCoords.x - sizeToBorders[RelaxManager.Instance.currentBubbleSize][0] &&
                transform.position.x + (-touchDeltaPosition.x * speed) > minCoords.x + sizeToBorders[RelaxManager.Instance.currentBubbleSize][0] &&
                transform.position.y + (-touchDeltaPosition.y * speed) < maxCoords.y - sizeToBorders[RelaxManager.Instance.currentBubbleSize][1] &&
                transform.position.y + (-touchDeltaPosition.y * speed) > minCoords.y + sizeToBorders[RelaxManager.Instance.currentBubbleSize][1] )
            {
                transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
            }
        }
    }
}