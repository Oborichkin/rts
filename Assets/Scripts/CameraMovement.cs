using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Tutorials used:
    //  - [Game Dev Guide] Building a Camera Controller for a Strategy Game (https://www.youtube.com/watch?v=rnqF6S7PfFA)
    //  - [Brackeys] How to make RTS Camera Movement in Unity (https://www.youtube.com/watch?v=cfjLQrMGEb4)

    public float movementSpeed = 1f;
    public float movementTime = 500f;
    public float rotationAmount = 0.3f;
    public float zoomSpeed = 10f;
    public float mouseZoomSpeed = 10f;
    public float minZoom = 500f;
    public float maxZoom = 2000f;
    public float borderThickness = 10f;
    Vector3 zoomAmount;
    Terrain terrain;
    Vector3 newPosition;
    Quaternion newRotation;
    Vector3 newZoom;
    Transform cameraTransform;
    Vector3 mapMinBounds;
    Vector3 mapMaxBounds;

    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;

    void Start()
    {
        terrain = GameObject.FindWithTag("Terrain").GetComponent<Terrain>();
        newPosition = transform.position;
        newRotation = transform.rotation;
        cameraTransform = Camera.main.transform;
        newZoom = cameraTransform.localPosition;
        zoomAmount = new Vector3(0, 1, -1);
        mapMinBounds = new Vector3(terrain.transform.position.x, 0, terrain.transform.position.z);
        mapMaxBounds = new Vector3(terrain.terrainData.size.x, 0, terrain.terrainData.size.z);
    }

    void LateUpdate()
    {
        HandleKeyboardInput();
        HandleEdgeScreenPanning();
        HandleRotation();
        HandleZoom();
        HandleZommWithScrollwheel();
        HandleMouseDragging();

        ClampPosition(ref newPosition);
        ClampZoom(ref newZoom);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    void ClampPosition(ref Vector3 newPosition)
    {
        if (newPosition.x > mapMaxBounds.x)
            newPosition.x = mapMaxBounds.x;
        if (newPosition.x < mapMinBounds.x)
            newPosition.x = mapMinBounds.x;
        if (newPosition.z > mapMaxBounds.z)
            newPosition.z = mapMaxBounds.z;
        if (newPosition.z < mapMinBounds.z)
            newPosition.z = mapMinBounds.z;
    }

    void ClampZoom(ref Vector3 newZoom)
    {
        if (newZoom.y < minZoom)
        {
            newZoom.y = minZoom;
            newZoom.z = -minZoom;
        }
        if (newZoom.y > maxZoom)
        {
            newZoom.y = maxZoom;
            newZoom.z = -maxZoom;
        }
    }

    void HandleMouseDragging()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
    }

    void HandleZoom()
    {
        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount * zoomSpeed;
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount * zoomSpeed;
        }
    }

    void HandleZommWithScrollwheel()
    {
        newZoom -= zoomAmount * Input.mouseScrollDelta.y * mouseZoomSpeed;
    }

    void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
    }

    void HandleKeyboardInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }
    }

    void HandleEdgeScreenPanning()
    {
        if (Input.mousePosition.y > Screen.height - borderThickness)
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.mousePosition.y < borderThickness)
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.mousePosition.x > Screen.width - borderThickness)
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.mousePosition.x < borderThickness)
        {
            newPosition += (transform.right * -movementSpeed);
        }
    }
}
