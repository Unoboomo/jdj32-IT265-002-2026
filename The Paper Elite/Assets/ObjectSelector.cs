using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ObjectSelector : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Awake()
    {
        var c = GetComponent<Camera>();
        if (c != null && _camera != null)
        {
            _camera = c;
        }

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("user clicked left mouse button");
            Vector3 mouse = Vector3.zero;
            mouse.x = Input.mousePosition.x;
            mouse.y = Input.mousePosition.y;
            Ray ray = _camera.ScreenPointToRay(mouse);
            RaycastHit hitInfo;
            Vector3 origin = this.transform.position;
            Vector3 direction = this.transform.forward;
            Debug.DrawLine(ray.origin, ray.direction * 100, Color.blue, 3);

            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Debug.DrawRay(ray.origin, hitInfo.point, Color.green, 3);
            }
        }
    }
}