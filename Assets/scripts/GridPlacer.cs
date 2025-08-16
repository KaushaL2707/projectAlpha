using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  // new input system

public class GridPlacer : MonoBehaviour
{
    [Header("Placement")]
    public GameObject turretPrefab;
    public Material validMaterial;   // green preview
    public Material invalidMaterial; // red preview
    public float cellSize = 1f;
    public Vector3 gridOrigin = Vector3.zero;
    public float yPlacement = 0f;

    [Header("Raycast")]
    public LayerMask groundMask;
    public float rayMaxDistance = 200f;

    private Camera _cam;
    private readonly HashSet<Vector2Int> _occupied = new();

    private GameObject _previewObj;
    private Renderer _previewRenderer;
    private Vector2Int _currentCell;

    void Awake()
    {
        _cam = Camera.main;

        // create preview ghost (start hidden)
        _previewObj = Instantiate(turretPrefab);
        _previewRenderer = _previewObj.GetComponentInChildren<Renderer>();
        _previewObj.SetActive(false); // hidden until pointing at ground

        // Disable colliders & rigidbodies
        foreach (var col in _previewObj.GetComponentsInChildren<Collider>())
            col.enabled = false;

        foreach (var rb in _previewObj.GetComponentsInChildren<Rigidbody>())
            Destroy(rb);
    }

    void Update()
    {
        if (_cam == null) return;

        // Touch input (phone)
        if (Touchscreen.current != null)
        {
            var t = Touchscreen.current.primaryTouch;
            Vector2 pos = t.position.ReadValue();

            // Update ghost preview
            UpdatePreview(pos);

            if (t.press.wasReleasedThisFrame) // finger lifted
                TryPlace();
        }
        // Mouse input (editor testing)
        else if (Mouse.current != null)
        {
            Vector2 pos = Mouse.current.position.ReadValue();

            UpdatePreview(pos);

            if (Mouse.current.leftButton.wasReleasedThisFrame)
                TryPlace();
        }
    }

    void UpdatePreview(Vector2 screenPos)
    {
        Ray ray = _cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, rayMaxDistance, groundMask))
        {
            _previewObj.SetActive(true);

            // Get snapped cell
            _currentCell = WorldToCell(hit.point);
            Vector3 worldPos = CellToWorld(_currentCell);
            _previewObj.transform.position = worldPos;

            // Set material color (valid = green, invalid = red)
            if (_occupied.Contains(_currentCell))
                _previewRenderer.material = invalidMaterial;
            else
                _previewRenderer.material = validMaterial;
        }
        else
        {
            _previewObj.SetActive(false); // hide if not pointing at ground
        }
    }

    void TryPlace()
    {
        if (!_previewObj.activeSelf) return; // not over ground

        // Only place if valid
        if (_occupied.Contains(_currentCell))
            return;

        Vector3 worldPos = CellToWorld(_currentCell);
        Instantiate(turretPrefab, worldPos, Quaternion.identity);
        _occupied.Add(_currentCell);
    }

    Vector2Int WorldToCell(Vector3 world)
    {
        Vector3 local = world - gridOrigin;
        int cx = Mathf.RoundToInt(local.x / cellSize);
        int cz = Mathf.RoundToInt(local.z / cellSize);
        return new Vector2Int(cx, cz);
    }

    Vector3 CellToWorld(Vector2Int cell)
    {
        float x = cell.x * cellSize;
        float z = cell.y * cellSize;
        return new Vector3(x, yPlacement, z) + gridOrigin;
    }
}
