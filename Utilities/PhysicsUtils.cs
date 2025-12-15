using UnityEngine;

namespace VTLTools
{
    public static class PhysicsUtils
    {
        public static bool GetMousePositionOnPlane(Ray ray, Plane plane, out Vector3 hitPoint)
        {
            if (plane.Raycast(ray, out float enter))
            {
                hitPoint = ray.GetPoint(enter);
                return true;
            }
            else
            {
                hitPoint = Vector3.zero;
                return false;
            }
        }

        public static bool RaycastMouseToLayer(LayerMask mask, out RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hit, Mathf.Infinity, mask);
        }
    }
}