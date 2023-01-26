using UnityEngine;

namespace Utils
{
    public static class UnityUtils
    {
        public static void CheckForTeleport(this MonoBehaviour monoBehaviour, Camera camera, Rigidbody2D rigidbody)
        {
            Vector3 pos = camera.WorldToViewportPoint(monoBehaviour.transform.position);
            if (pos.x < 0.0f && IsMovingInDirection(monoBehaviour, Vector3.left, rigidbody.velocity))
            {
                pos = new Vector3(1.0f, pos.y, pos.z);
            }
            else if (pos.x >= 1.0f && IsMovingInDirection(monoBehaviour, Vector3.right, rigidbody.velocity))
            {
                pos = new Vector3(0.0f, pos.y, pos.z);
            }

            if (pos.y < 0.0f && IsMovingInDirection(monoBehaviour, Vector3.down, rigidbody.velocity))
            {
                pos = new Vector3(pos.x, 1.0f, pos.z);
            }
            else if (pos.y >= 1.0f && IsMovingInDirection(monoBehaviour, Vector3.up, rigidbody.velocity))
            {
                pos = new Vector3(pos.x, 0.0f, pos.z);
            }

            monoBehaviour.transform.position = camera.ViewportToWorldPoint(pos);
        }

        public static bool IsMovingInDirection(this MonoBehaviour monoBehaviour, Vector3 dir, Vector3 velocity)
        {
            return Vector3.Dot(dir, velocity) > 0;
        }
    }
}