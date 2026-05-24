using System;
using EasyToolkit.Core.Foundation;
using UnityEditor;
using UnityEngine;

namespace EasyToolkit.Core.Editor
{
    public enum TriangleOrientations
    {
        Up,
        Down,
        Left,
        Right
    }

    public class EasyHandles
    {
        private const float CapsuleSizeEpsilon = 0.0001f;

        public static Rect GetWorldUnitsRectWithScaleFactor(Vector3 worldPosition, float sizeInWorldUnits)
        {
            float scaleFactor = HandleUtility.GetHandleSize(worldPosition);
            Vector3 screenPosition = HandleUtility.WorldToGUIPoint(worldPosition);
            float size = sizeInWorldUnits / scaleFactor * 100f;
            return new Rect(screenPosition.x - size / 2, screenPosition.y - size / 2, size, size);
        }

        public static void DrawTextureWithScaleFactor(Vector3 worldPosition, Texture image, float sizeInWorldUnits)
        {
            var rect = GetWorldUnitsRectWithScaleFactor(worldPosition, sizeInWorldUnits);

            Handles.BeginGUI();
            GUI.DrawTexture(rect, image);
            Handles.EndGUI();
        }

        public static void DrawRectangle(Rect rectangle)
        {
            Handles.DrawSolidRectangleWithOutline(rectangle, Color.white, Color.white);
        }

        public static void DrawRectangle2D(Vector2 position, Vector2 size)
        {
            DrawRectangle(position.ToVector3(), size.ToVector3());
        }

        public static void LabelWithScaleFactor(Vector3 worldPosition, string text, GUIStyle style)
        {
            float scaleFactor = HandleUtility.GetHandleSize(worldPosition);

            var style2 = new GUIStyle(style)
            {
                fontSize = Mathf.CeilToInt(style.fontSize / scaleFactor)
            };

            Handles.Label(worldPosition, text, style2);
        }

        public static void DrawLine2D(Vector2 p1, Vector2 p2)
        {
            Handles.DrawLine(p1.ToVector3(), p2.ToVector3());
        }

        /// <summary>
        /// Draws a wire capsule on the XY plane using the active handle color and matrix.
        /// </summary>
        /// <param name="center">The capsule center.</param>
        /// <param name="size">The capsule width and height.</param>
        /// <param name="direction">The direction of the capsule's long axis.</param>
        public static void DrawWireCapsule2D(Vector2 center, Vector2 size,
            CapsuleDirection2D direction = CapsuleDirection2D.Vertical)
        {
            DrawWireCapsule(center.ToVector3(), size, direction);
        }

        /// <summary>
        /// Draws a wire capsule on the XY plane using the active handle color and matrix.
        /// </summary>
        /// <param name="center">The capsule center.</param>
        /// <param name="size">The capsule width and height.</param>
        /// <param name="direction">The direction of the capsule's long axis.</param>
        public static void DrawWireCapsule(Vector3 center, Vector2 size,
            CapsuleDirection2D direction = CapsuleDirection2D.Vertical)
        {
            var capsule = Capsule2DData.Create(center, size, direction);
            if (!capsule.IsValid)
            {
                return;
            }

            if (direction == CapsuleDirection2D.Horizontal)
            {
                var upperOffset = Vector3.up * capsule.Radius;
                var lowerOffset = Vector3.down * capsule.Radius;

                Handles.DrawLine(capsule.StartCenter + upperOffset, capsule.EndCenter + upperOffset);
                Handles.DrawLine(capsule.StartCenter + lowerOffset, capsule.EndCenter + lowerOffset);
                Handles.DrawWireArc(capsule.StartCenter, Vector3.forward, Vector3.down, -180f, capsule.Radius);
                Handles.DrawWireArc(capsule.EndCenter, Vector3.forward, Vector3.up, -180f, capsule.Radius);
                return;
            }

            var leftOffset = Vector3.left * capsule.Radius;
            var rightOffset = Vector3.right * capsule.Radius;

            Handles.DrawLine(capsule.StartCenter + leftOffset, capsule.EndCenter + leftOffset);
            Handles.DrawLine(capsule.StartCenter + rightOffset, capsule.EndCenter + rightOffset);
            Handles.DrawWireArc(capsule.StartCenter, Vector3.forward, Vector3.left, 180f, capsule.Radius);
            Handles.DrawWireArc(capsule.EndCenter, Vector3.forward, Vector3.right, 180f, capsule.Radius);
        }

        /// <summary>
        /// Draws a solid capsule on the XY plane using the active handle color and matrix.
        /// </summary>
        /// <param name="center">The capsule center.</param>
        /// <param name="size">The capsule width and height.</param>
        /// <param name="direction">The direction of the capsule's long axis.</param>
        public static void DrawSolidCapsule2D(Vector2 center, Vector2 size,
            CapsuleDirection2D direction = CapsuleDirection2D.Vertical)
        {
            DrawSolidCapsule(center.ToVector3(), size, direction);
        }

        /// <summary>
        /// Draws a solid capsule on the XY plane using the active handle color and matrix.
        /// </summary>
        /// <param name="center">The capsule center.</param>
        /// <param name="size">The capsule width and height.</param>
        /// <param name="direction">The direction of the capsule's long axis.</param>
        public static void DrawSolidCapsule(Vector3 center, Vector2 size,
            CapsuleDirection2D direction = CapsuleDirection2D.Vertical)
        {
            var capsule = Capsule2DData.Create(center, size, direction);
            if (!capsule.IsValid)
            {
                return;
            }

            if (direction == CapsuleDirection2D.Horizontal)
            {
                DrawSolidCapsuleBody(
                    capsule.StartCenter + Vector3.up * capsule.Radius,
                    capsule.EndCenter + Vector3.up * capsule.Radius,
                    capsule.EndCenter + Vector3.down * capsule.Radius,
                    capsule.StartCenter + Vector3.down * capsule.Radius);
                Handles.DrawSolidArc(capsule.StartCenter, Vector3.forward, Vector3.down, -180f, capsule.Radius);
                Handles.DrawSolidArc(capsule.EndCenter, Vector3.forward, Vector3.up, -180f, capsule.Radius);
                return;
            }

            DrawSolidCapsuleBody(
                capsule.StartCenter + Vector3.left * capsule.Radius,
                capsule.StartCenter + Vector3.right * capsule.Radius,
                capsule.EndCenter + Vector3.right * capsule.Radius,
                capsule.EndCenter + Vector3.left * capsule.Radius);
            Handles.DrawSolidArc(capsule.StartCenter, Vector3.forward, Vector3.left, 180f, capsule.Radius);
            Handles.DrawSolidArc(capsule.EndCenter, Vector3.forward, Vector3.right, 180f, capsule.Radius);
        }

        public static void DrawRectangle(Vector3 position, Vector3 size)
        {
            Handles.DrawSolidRectangleWithOutline(new Rect(position, size), Color.white, Color.white);
        }

        public static void DrawTriangleInRect(Vector3 position, Vector3 size, TriangleOrientations orientation)
        {
            DrawTriangleInRect(new Rect(position, size), orientation);
        }

        public static void DrawTriangleInRect(Rect rect, TriangleOrientations orientation)
        {
            Vector3 v1;
            Vector3 v2;
            Vector3 v3;

            switch (orientation)
            {
                case TriangleOrientations.Up:
                    v1 = new Vector3(rect.xMin + rect.width / 2, rect.yMax);
                    v2 = new Vector3(rect.xMin, rect.yMin);
                    v3 = new Vector3(rect.xMax, rect.yMin);
                    break;
                case TriangleOrientations.Down:
                    v1 = new Vector3(rect.xMin + rect.width / 2, rect.yMin);
                    v2 = new Vector3(rect.xMin, rect.yMax);
                    v3 = new Vector3(rect.xMax, rect.yMax);
                    break;
                case TriangleOrientations.Left:
                    v1 = new Vector3(rect.xMin, rect.yMin + rect.height / 2);
                    v2 = new Vector3(rect.xMax, rect.yMin);
                    v3 = new Vector3(rect.xMax, rect.yMax);
                    break;
                case TriangleOrientations.Right:
                    v1 = new Vector3(rect.xMax, rect.yMin + rect.height / 2);
                    v2 = new Vector3(rect.xMin, rect.yMin);
                    v3 = new Vector3(rect.xMin, rect.yMax);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
            }

            Handles.DrawAAConvexPolygon(v1, v2, v3);
        }

        private static void DrawSolidCapsuleBody(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            if ((p1 - p2).sqrMagnitude <= CapsuleSizeEpsilon || (p2 - p3).sqrMagnitude <= CapsuleSizeEpsilon)
            {
                return;
            }

            Handles.DrawAAConvexPolygon(p1, p2, p3, p4);
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
