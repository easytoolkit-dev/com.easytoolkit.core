using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyToolKit.Core
{
    /// <summary>
    /// Builder for creating method invokers that call methods with optional parameters.
    /// </summary>
    public sealed class InvokerBuilder : MemberPathBuilderBase, IInvokerBuilder
    {
        /// <summary>
        /// Initializes a new instance of the InvokerBuilder.
        /// </summary>
        /// <param name="methodPath">The path to the method (e.g., "Method", "Child.Object.Method()").</param>
        public InvokerBuilder(string methodPath) : base(methodPath)
        {
        }

        /// <inheritdoc/>
        public string MethodPath => MemberPath;

        /// <inheritdoc/>
        public StaticFuncInvoker BuildStaticFunc(Type targetType, params Type[] parameterTypes)
        {
            var (method, pathSteps) = ResolveMethodInfo(targetType, isStatic: true, parameterTypes ?? Type.EmptyTypes);

            if (pathSteps.Count == 0)
            {
                // Simple static method
                return args => method.Invoke(null, args);
            }

            // Path-based static method
            return args =>
            {
                object target = null;
                for (int i = 0; i < pathSteps.Count; i++)
                {
                    target = ExecuteStep(pathSteps[i], target);
                }
                return method.Invoke(target, args);
            };
        }

        /// <inheritdoc/>
        public InstanceFuncInvoker BuildInstanceFunc(Type targetType, params Type[] parameterTypes)
        {
            var (method, pathSteps) = ResolveMethodInfo(targetType, isStatic: false, parameterTypes ?? Type.EmptyTypes);

            if (pathSteps.Count == 0)
            {
                // Simple instance method
                return (ref object target, object[] args) => method.Invoke(target, args);
            }

            // Path-based instance method
            return (ref object target, object[] args) =>
            {
                object current = target;
                for (int i = 0; i < pathSteps.Count; i++)
                {
                    current = ExecuteStep(pathSteps[i], current);
                }
                return method.Invoke(current, args);
            };
        }

        /// <summary>
        /// Resolves the method info and path steps for the given target type and parameters.
        /// </summary>
        private (MethodInfo method, List<PathStep> pathSteps) ResolveMethodInfo(Type targetType, bool isStatic, Type[] parameterTypes)
        {
            // Check if the path contains dots (is a complex path)
            if (!MemberPath.Contains("."))
            {
                // Simple method name - use direct method resolution
                MethodInfo resolvedMethod = targetType.ResolveOverloadMethod(MemberPath, BindingFlagsHelper.All, parameterTypes);
                return (resolvedMethod, new List<PathStep>());
            }

            // Complex path - parse the path and get the final method
            List<PathStep> pathSteps = MemberPathParser.Parse(targetType, MemberPath, isStatic);

            // The last step should be a method
            PathStep lastStep = pathSteps[^1];
            if (lastStep.StepType != PathStepType.Member || lastStep.Member is not MethodInfo)
            {
                throw new ArgumentException($"The last element in path '{MemberPath}' is not a method.");
            }

            MethodInfo method = (MethodInfo)lastStep.Member;

            // For parameterized methods, we need to find the correct overload
            if (parameterTypes.Length > 0)
            {
                ParameterInfo[] methodParams = method.GetParameters();
                if (methodParams.Length != parameterTypes.Length)
                {
                    throw new ArgumentException($"Method '{method.Name}' has {methodParams.Length} parameters, but {parameterTypes.Length} parameter types were provided.");
                }

                // Verify parameter types match
                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    if (!methodParams[i].ParameterType.IsAssignableFrom(parameterTypes[i]))
                    {
                        throw new ArgumentException($"Parameter type mismatch at index {i}: expected {methodParams[i].ParameterType.Name}, got {parameterTypes[i].Name}.");
                    }
                }
            }
            else if (method.GetParameters().Length > 0)
            {
                throw new ArgumentException($"Method '{method.Name}' requires parameters, but no parameter types were provided. Use the parameter overload of Build methods.");
            }

            // Remove the last step (the method itself) from the path steps
            pathSteps.RemoveAt(pathSteps.Count - 1);

            return (method, pathSteps);
        }
    }
}
