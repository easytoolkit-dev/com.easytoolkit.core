using UnityEngine;

namespace EasyToolkit.Core.Unity
{
    public static class GizmosUtility
    {
        private const int DefaultCapsuleSegmentCount = 16;
        private const float CapsuleSizeEpsilon = 0.0001f;

        public static void DrawMeshRect(Vector2 center, Vector2 size, Vector2 density)
        {
            var halfSize = size / 2;

            var p1 = new Vector2(center.x - halfSize.x, center.y - halfSize.y);
            var p2 = new Vector2(center.x + halfSize.x, center.y - halfSize.y);
            var p3 = new Vector2(center.x + halfSize.x, center.y + halfSize.y);
            var p4 = new Vector2(center.x - halfSize.x, center.y + halfSize.y);

            if (density.x > 0.01f)
            {
                Gizmos.DrawLine(p2, p3);
                Gizmos.DrawLine(p4, p1);
                int gridX = Mathf.FloorToInt(size.x / density.x);
                var stepX = size.x / gridX;
                for (int i = 1; i < gridX; i++)
                {
                    var x = p1.x + i * stepX;
                    Gizmos.DrawLine(new Vector3(x, p1.y, 0), new Vector3(x, p4.y, 0));
                }
            }

            if (density.y > 0.01f)
            {
                Gizmos.DrawLine(p1, p2);
                Gizmos.DrawLine(p3, p4);
                int gridY = Mathf.FloorToInt(size.y / density.y);

                var stepY = size.y / gridY;
                for (int i = 1; i < gridY; i++)
                {
                    var y = p1.y + i * stepY;
                    Gizmos.DrawLine(new Vector3(p1.x, y, 0), new Vector3(p2.x, y, 0));
                }
            }
        }

        /// <summary>
        /// Draws a wire capsule on the XY plane using the active gizmo color and matrix.
        /// </summary>
        /// <param name="center">The capsule center.</param>
        /// <param name="size">The capsule width and height.</param>
        /// <param name="direction">The direction of the capsule's long axis.</param>
        /// <param name="segmentCount">The number of segments used for each semicircle.</param>
        public static void DrawWireCapsule2D(Vector2 center, Vector2 size,
            CapsuleDirection2D direction = CapsuleDirection2D.Vertical,
            int segmentCount = DefaultCapsuleSegmentCount)
        {
            DrawWireCapsule(center, size, direction, segmentCount);
        }

        /// <summary>
        /// Draws a wire capsule on the XY plane using the active gizmo color and matrix.
        /// </summary>
        /// <param name="center">The capsule center.</param>
        /// <param name="size">The capsule width and height.</param>
        /// <param name="direction">The direction of the capsule's long axis.</param>
        /// <param name="segmentCount">The number of segments used for each semicircle.</param>
        public static void DrawWireCapsule(Vector3 center, Vector2 size,
            CapsuleDirection2D direction = CapsuleDirection2D.Vertical,
            int segmentCount = DefaultCapsuleSegmentCount)
        {
            var capsule = Capsule2DData.Create(center, size, direction);
            if (!capsule.IsValid)
            {
                return;
            }

            segmentCount = Mathf.Max(1, segmentCount);

            if (direction == CapsuleDirection2D.Horizontal)
            {
                var upperOffset = Vector3.up * capsule.Radius;
                var lowerOffset = Vector3.down * capsule.Radius;

                Gizmos.DrawLine(capsule.StartCenter + upperOffset, capsule.EndCenter + upperOffset);
                Gizmos.DrawLine(capsule.StartCenter + lowerOffset, capsule.EndCenter + lowerOffset);
                DrawWireArc(capsule.StartCenter, 270f, 90f, capsule.Radius, segmentCount);
                DrawWireArc(capsule.EndCenter, 90f, -90f, capsule.Radius, segmentCount);
                return;
            }

            var leftOffset = Vector3.left * capsule.Radius;
            var rightOffset = Vector3.right * capsule.Radius;

            Gizmos.DrawLine(capsule.StartCenter + leftOffset, capsule.EndCenter + leftOffset);
            Gizmos.DrawLine(capsule.StartCenter + rightOffset, capsule.EndCenter + rightOffset);
            DrawWireArc(capsule.StartCenter, 180f, 360f, capsule.Radius, segmentCount);
            DrawWireArc(capsule.EndCenter, 0f, 180f, capsule.Radius, segmentCount);
        }

        public static void DrawCube(Transform transform, Vector3 offset, Vector3 cubeSize, bool wireOnly)
        {
            Matrix4x4 rotationMatrix = transform.localToWorldMatrix;
            Gizmos.matrix = rotationMatrix;
            if (wireOnly)
            {
                Gizmos.DrawWireCube(offset, cubeSize);
            }
            else
            {
                Gizmos.DrawCube(offset, cubeSize);
            }
        }

        public static void DrawMeshRect(Vector2 center, Vector2 size)
        {
            DrawMeshRect(center, size, new Vector2(0.1f, 0.1f));
        }

        private static void DrawWireArc(Vector3 center, float startAngle, float endAngle, float radius, int segmentCount)
        {
            var previousPoint = GetPointOnCircle(center, radius, startAngle);
            for (var i = 1; i <= segmentCount; i++)
            {
                var angle = Mathf.Lerp(startAngle, endAngle, i / (float)segmentCount);
                var nextPoint = GetPointOnCircle(center, radius, angle);
                Gizmos.DrawLine(previousPoint, nextPoint);
                previousPoint = nextPoint;
            }
        }

        private static Vector3 GetPointOnCircle(Vector3 center, float radius, float angle)
        {
            var radians = angle * Mathf.Deg2Rad;
            return center + new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * radius;
        }

        private readonly struct Capsule2DData
        {
            private Capsule2DData(Vector3 startCenter, Vector3 endCenter, float radius)
            {
                StartCenter = startCenter;
                EndCenter = endCenter;
                Radius = radius;
            }

            public Vector3 StartCenter { get; }

            public Vector3 EndCenter { get; }

            public float Radius { get; }

            public bool IsValid => Radius > CapsuleSizeEpsilon;

            public static Capsule2DData Create(Vector3 center, Vector2 size, CapsuleDirection2D direction)
            {
                var width = Mathf.Abs(size.x);
                var height = Mathf.Abs(size.y);
                var radius = Mathf.Min(width, height) * 0.5f;

                if (direction == CapsuleDirection2D.Horizontal)
                {
                    var halfStraightLength = Mathf.Max(0f, width * 0.5f - radius);
                    var axisOffset = Vector3.right * halfStraightLength;
                    return new Capsule2DData(center - axisOffset, center + axisOffset, radius);
                }

                var verticalHalfStraightLength = Mathf.Max(0f, height * 0.5f - radius);
                var verticalAxisOffset = Vector3.up * verticalHalfStraightLength;
                return new Capsule2DData(center - verticalAxisOffset, center + verticalAxisOffset, radius);
            }
        }
    }
}
