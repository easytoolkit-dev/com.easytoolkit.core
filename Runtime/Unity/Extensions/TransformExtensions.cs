using System;
using System.Collections.Generic;
using EasyToolkit.Core.Mathematics;
using UnityEngine;

namespace EasyToolkit.Core.Unity
{
    [Serializable]
    public class TransformRecorder
    {
        [SerializeField] private Vector3 _localPosition;
        [SerializeField] private Quaternion _localRotation;
        [SerializeField] private Vector3 _localScale;
        [SerializeField] private Transform _parent;

        public void Record(Transform target)
        {
            _localPosition = target.localPosition;
            _localRotation = target.localRotation;
            _localScale = target.localScale;
            _parent = target.parent;
        }

        public void Resume(Transform target)
        {
            target.SetParent(_parent);
            target.localPosition = _localPosition;
            target.localRotation = _localRotation;
            target.localScale = _localScale;
        }
    }

    public static class TransformExtensions
    {
        public static bool IsParentRecursive(this Transform transform, Transform parent)
        {
            if (transform == parent)
                return false;

            var p = transform.parent;
            while (p != parent)
            {
                if (p == null)
                    return false;
                p = p.parent;
            }

            return true;
        }

        public static T[] FindObjectsByTypeInParents<T>(this Transform transform, bool includeInactive = false, bool includeSelf = false)
        {
            var total = new List<T>();

            var p = includeSelf ? transform : transform.parent;
            while (p != null)
            {
                var c = p.GetComponent<T>();
                if (c != null)
                {
                    total.Add(c);
                }

                p = p.parent;
            }

            return total.ToArray();
        }

        public static TransformRecorder GetRecorder(this Transform transform)
        {
            var recorder = new TransformRecorder();
            recorder.Record(transform);
            return recorder;
        }

        public static float ScaleSquare(this Transform transform)
        {
            return transform.localScale.magnitude;
        }

        #region Fluent API - Position

        /// <summary>
        /// Sets the transform's position and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="position">The new position.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithPosition(this Transform transform, Vector3 position)
        {
            transform.position = position;
            return transform;
        }

        /// <summary>
        /// Sets the transform's position from a Vector2 and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="position">The new XY position.</param>
        /// <param name="z">The Z component to use.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithPosition(this Transform transform, Vector2 position, float z = 0f)
        {
            transform.position = position.ToVector3(z);
            return transform;
        }

        /// <summary>
        /// Sets the transform's position XY components and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="position">The new XY position (Z is preserved).</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithPositionXY(this Transform transform, Vector3 position)
        {
            transform.position = transform.position.WithX(position.x).WithY(position.y);
            return transform;
        }

        /// <summary>
        /// Sets the transform's position XY components from a Vector2 and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="position">The new XY position (Z is preserved).</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithPositionXY(this Transform transform, Vector2 position)
        {
            transform.position = transform.position.WithX(position.x).WithY(position.y);
            return transform;
        }

        /// <summary>
        /// Sets the transform's position X component and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="x">The new X value.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithPositionX(this Transform transform, float x)
        {
            transform.position = transform.position.WithX(x);
            return transform;
        }

        /// <summary>
        /// Sets the transform's position Y component and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="y">The new Y value.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithPositionY(this Transform transform, float y)
        {
            transform.position = transform.position.WithY(y);
            return transform;
        }

        /// <summary>
        /// Sets the transform's position Z component and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="z">The new Z value.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithPositionZ(this Transform transform, float z)
        {
            transform.position = transform.position.WithZ(z);
            return transform;
        }

        /// <summary>
        /// Sets the transform's local position and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="localPosition">The new local position.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithLocalPosition(this Transform transform, Vector3 localPosition)
        {
            transform.localPosition = localPosition;
            return transform;
        }

        /// <summary>
        /// Sets the transform's local position from a Vector2 and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="localPosition">The new local XY position.</param>
        /// <param name="z">The Z component to use.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithLocalPosition(this Transform transform, Vector2 localPosition, float z = 0f)
        {
            transform.localPosition = localPosition.ToVector3(z);
            return transform;
        }

        /// <summary>
        /// Sets the transform's local position X component and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="x">The new X value.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithLocalPositionX(this Transform transform, float x)
        {
            transform.localPosition = transform.localPosition.WithX(x);
            return transform;
        }

        /// <summary>
        /// Sets the transform's local position Y component and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="y">The new Y value.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithLocalPositionY(this Transform transform, float y)
        {
            transform.localPosition = transform.localPosition.WithY(y);
            return transform;
        }

        /// <summary>
        /// Sets the transform's local position Z component and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="z">The new Z value.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithLocalPositionZ(this Transform transform, float z)
        {
            transform.localPosition = transform.localPosition.WithZ(z);
            return transform;
        }

        #endregion

        #region Fluent API - Rotation

        /// <summary>
        /// Sets the transform's rotation and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="rotation">The new rotation.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithRotation(this Transform transform, Quaternion rotation)
        {
            transform.rotation = rotation;
            return transform;
        }

        /// <summary>
        /// Sets the transform's rotation from Euler angles and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="euler">The new Euler angles.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithRotationEuler(this Transform transform, Vector3 euler)
        {
            transform.rotation = Quaternion.Euler(euler);
            return transform;
        }

        /// <summary>
        /// Sets the transform's local rotation and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="localRotation">The new local rotation.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithLocalRotation(this Transform transform, Quaternion localRotation)
        {
            transform.localRotation = localRotation;
            return transform;
        }

        /// <summary>
        /// Sets the transform's local rotation from Euler angles and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="euler">The new local Euler angles.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithLocalRotationEuler(this Transform transform, Vector3 euler)
        {
            transform.localRotation = Quaternion.Euler(euler);
            return transform;
        }

        /// <summary>
        /// Rotates the transform by the specified Euler angles and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="euler">The Euler angles to rotate by.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithRotated(this Transform transform, Vector3 euler)
        {
            transform.rotation = Quaternion.Euler(euler) * transform.rotation;
            return transform;
        }

        /// <summary>
        /// Rotates the transform locally by the specified Euler angles and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="euler">The local Euler angles to rotate by.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithRotatedLocal(this Transform transform, Vector3 euler)
        {
            transform.localRotation = Quaternion.Euler(euler) * transform.localRotation;
            return transform;
        }

        /// <summary>
        /// Rotates the transform around the Z axis (2D rotation) and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithRotatedZ(this Transform transform, float angle)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle) * transform.rotation;
            return transform;
        }

        /// <summary>
        /// Rotates the transform locally around the Z axis (2D rotation) and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="angle">The local rotation angle in degrees.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithRotatedLocalZ(this Transform transform, float angle)
        {
            transform.localRotation = Quaternion.Euler(0, 0, angle) * transform.localRotation;
            return transform;
        }

        #endregion

        #region Fluent API - Scale

        /// <summary>
        /// Sets the transform's local scale and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="scale">The new scale.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithScale(this Transform transform, Vector3 scale)
        {
            transform.localScale = scale;
            return transform;
        }

        /// <summary>
        /// Sets the transform's local scale uniformly and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="uniformScale">The uniform scale value for all axes.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithScale(this Transform transform, float uniformScale)
        {
            transform.localScale = Vector3.one * uniformScale;
            return transform;
        }

        /// <summary>
        /// Sets the transform's local scale from a Vector2 and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="scale">The new XY scale.</param>
        /// <param name="z">The Z scale to use.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithScale(this Transform transform, Vector2 scale, float z = 1f)
        {
            transform.localScale = scale.ToVector3(z);
            return transform;
        }

        /// <summary>
        /// Sets the transform's local scale X component and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="x">The new X scale value.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithScaleX(this Transform transform, float x)
        {
            transform.localScale = transform.localScale.WithX(x);
            return transform;
        }

        /// <summary>
        /// Sets the transform's local scale Y component and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="y">The new Y scale value.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithScaleY(this Transform transform, float y)
        {
            transform.localScale = transform.localScale.WithY(y);
            return transform;
        }

        /// <summary>
        /// Sets the transform's local scale Z component and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="z">The new Z scale value.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithScaleZ(this Transform transform, float z)
        {
            transform.localScale = transform.localScale.WithZ(z);
            return transform;
        }

        /// <summary>
        /// Sets the transform's scale magnitude while preserving direction and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="size">The new scale magnitude.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithScaleSquare(this Transform transform, float size)
        {
            transform.localScale = transform.localScale.normalized * size;
            return transform;
        }

        #endregion

        #region Fluent API - Hierarchy

        /// <summary>
        /// Sets the transform's parent and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="parent">The new parent transform.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithParent(this Transform transform, Transform parent)
        {
            transform.SetParent(parent);
            return transform;
        }

        /// <summary>
        /// Sets the transform's parent with world position settings and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="parent">The new parent transform.</param>
        /// <param name="worldPositionStays">Whether to maintain world position.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithParent(this Transform transform, Transform parent, bool worldPositionStays)
        {
            transform.SetParent(parent, worldPositionStays);
            return transform;
        }

        /// <summary>
        /// Sets the sibling index of the transform and returns the transform for chaining.
        /// </summary>
        /// <param name="transform">The transform to modify.</param>
        /// <param name="index">The new sibling index.</param>
        /// <returns>The transform for method chaining.</returns>
        public static Transform WithSiblingIndex(this Transform transform, int index)
        {
            transform.SetSiblingIndex(index);
            return transform;
        }

        #endregion


        public static string GetRelativePath(this Transform transform, Transform parent, bool includeParent = true)
        {
            if (transform == null)
                return string.Empty;
            var hierarchy = new Stack<string>();

            var p = transform;
            while (p != null && p != parent)
            {
                hierarchy.Push(p.gameObject.name);
                p = p.parent;
            }

            if (includeParent && parent != null)
            {
                hierarchy.Push(parent.gameObject.name);
            }

            var path = string.Join("/", hierarchy);

            if (parent == null)
                path = '/' + path;

            return path;
        }

        public static string GetAbsolutePath(this Transform transform, bool includeSceneName = true)
        {
            if (transform == null)
                return string.Empty;
            var path = GetRelativePath(transform, null);

            if (includeSceneName)
                path = '/' + transform.gameObject.scene.name + path;

            return path;
        }

        public static void DestroyChildren(this Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                UnityEngine.Object.Destroy(transform.GetChild(transform.childCount - i - 1).gameObject);
            }
        }

        /// <summary>
        /// Resets the transform's local properties to their default values.
        /// </summary>
        /// <param name="transform">The transform to reset.</param>
        /// <remarks>
        /// Sets localPosition to zero, localRotation to identity, and localScale to one.
        /// </remarks>
        public static void ResetLocal(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}
