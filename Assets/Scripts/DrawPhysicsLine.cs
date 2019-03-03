using UnityEngine;
using System.Collections;
public class DrawPhysicsLine : MonoBehaviour
{
    private LineRenderer line; // Reference to LineRenderer
    private Vector3 mousePos;
    private Vector3 touchPos;
    private Vector3 startPos;    // Start position of line
    private Vector3 endPos;    // End position of line
    private Touch myTouch;

    private void Update()
    {
        // ------------------ mouse input -----------------
        /*
        // On mouse down new line will be created 
        if (Input.GetMouseButtonDown(0))
        {
            //if (line == null)
            createLine();
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            line.SetPosition(0, mousePos);
            startPos = mousePos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (line)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                line.SetPosition(1, mousePos);
                endPos = mousePos;
                AddColliderToLine();
                line = null;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (line)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                line.SetPosition(1, mousePos);
            }
        }
        */
        // --------------- touch input ------------

        myTouch = Input.GetTouch(0);
        if (Input.touchCount > 0 && myTouch.phase == TouchPhase.Began)
        {
            createLine();
            touchPos = Camera.main.ScreenToWorldPoint(myTouch.position);
            touchPos.z = 0;
            line.SetPosition(0, touchPos);
            startPos = touchPos;
        }

        else if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            if (line)
            {
                touchPos = Camera.main.ScreenToWorldPoint(myTouch.position);
                touchPos.z = 0;
                line.SetPosition(1, touchPos);
                endPos = touchPos;
                AddColliderToLine();
                line = null;
            }
        }

        else if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
        {
            if (line)
            {
                touchPos = Camera.main.ScreenToWorldPoint(myTouch.position);
                touchPos.z = 0;
                line.SetPosition(1, touchPos);
            }
        }

    }
    // Following method creates line runtime using Line Renderer component
    private void createLine()
    {
        line = new GameObject("Line").AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Diffuse"));
        line.positionCount = 2;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.startColor = Color.black;
        line.endColor = Color.black;
        //line.SetVertexCount(2);
        //line.SetWidth(0.1f, 0.1f);
        //line.SetColors(Color.black, Color.black);
        line.useWorldSpace = true;
    }
    // Following method adds collider to created line
    private void AddColliderToLine()
    {
        BoxCollider col = new GameObject("Collider").AddComponent<BoxCollider>();
        col.transform.parent = line.transform; // Collider is added as child object of line
        float lineLength = Vector3.Distance(startPos, endPos); // length of line
        col.size = new Vector3(lineLength, 0.1f, 1f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
        Vector3 midPoint = (startPos + endPos) / 2;
        col.transform.position = midPoint; // setting position of collider object
        // Following lines calculate the angle between startPos and endPos
        float angle = (Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x));
        if ((startPos.y < endPos.y && startPos.x > endPos.x) || (endPos.y < startPos.y && endPos.x > startPos.x))
        {
            angle *= -1;
        }
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        col.transform.Rotate(0, 0, angle);
    }
}