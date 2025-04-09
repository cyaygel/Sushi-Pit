using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int _focusedID;
    private PlateController _focusedPlate;
    private GameObject _focusedObject;
    private bool _isDragging;
    private Vector3 _offset;
    [SerializeField] private LayerMask _draggableSurfaceLayerMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000 ,~_draggableSurfaceLayerMask))
            {
                if (hit.collider.CompareTag("Item"))
                {
                    _focusedObject = hit.collider.gameObject;
                    _isDragging = true;
                    _offset = _focusedObject.transform.position - hit.point;
                    if (_focusedObject.GetComponent<ItemController>())
                    {
                        _focusedID = _focusedObject.GetComponent<ItemController>().ID;
                    }
                }
            }
        }

        if (_isDragging && _focusedObject != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit2, 100f, _draggableSurfaceLayerMask))
            {
                _focusedObject.transform.position = hit2.point + _offset;
                if (hit2.collider.CompareTag("Plate"))
                {
                    _focusedObject.GetComponent<MeshRenderer>().enabled = false;
                    _focusedPlate = hit2.collider.gameObject.GetComponent<PlateController>();
                    _focusedPlate.SetSelectedEffect(true);
                }
                else
                {
                    _focusedObject.GetComponent<MeshRenderer>().enabled = true;
                    if (_focusedPlate != null) _focusedPlate.SetSelectedEffect(false);
                    _focusedPlate = null;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_focusedPlate != null)
            {
                Destroy(_focusedObject);
                _focusedPlate.TryAddItem(_focusedID);
                _focusedPlate.SetSelectedEffect(false);
            }
            _isDragging = false;
            _focusedObject = null;
            _focusedPlate = null;
            _focusedID = -1;
            
        }
    }
}