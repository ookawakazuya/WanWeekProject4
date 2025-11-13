using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BoardFrameDrawer : MonoBehaviour
{
    [SerializeField] int width = 10;
    [SerializeField] int height = 20;
    [SerializeField] float cellSize = 1f;
    [SerializeField] Color lineColor = Color.cyan;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 5;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        DrawFrame();
    }

    void DrawFrame()
    {
        Vector3 bottomLeft = transform.position;
        Vector3[] points = new Vector3[5];
        points[0] = bottomLeft;
        points[1] = bottomLeft + Vector3.up * height * cellSize;
        points[2] = points[1] + Vector3.right * width * cellSize;
        points[3] = bottomLeft + Vector3.right * width * cellSize;
        points[4] = bottomLeft;

        lineRenderer.SetPositions(points);
    }
}
