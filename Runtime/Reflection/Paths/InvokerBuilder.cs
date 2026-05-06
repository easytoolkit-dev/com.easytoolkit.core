using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Builder for creating method invokers that call methods with optional parameters.
    /// </summary>
    public sealed class InvokerBuilder
    {
        /// <summary>
        /// Gets the method path this builder operates on.
        /// </summary>
        private readonly string _methodPath;

        /// <summary>
        /// Initializes a new instance of the InvokerBuilder.
        /// </summary>
        /// <param name="methodPath">The path to the method (e.g., "Method", "Child.Object.Method()").</param>
        public InvokerBuilder(string methodPath)
        {
            _methodPath = methodPath;
        }

        /// <summary>
        /// Builds a delegate for invoking a static method with parameters.
        /// </summary>
        /// <param name="targetType">The type containing the static method.</param>
        /// <param name="parameterTypes">The parameter types for overload resolution. Can be null or empty for parameterless methods.</param>
        /// <returns>A delegate that invokes the static method with parameters and returns the result.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public StaticInvoker BuildStaticFunc(Type targetType, params Type[] parameterTypes)
        {
            var (method, pathSteps) = ResolveMethodInfo(targetType, isStatic: true, parameterTypes ?? Type.EmptyTypes);

            if (pathSteps.Count == 0)
            {
                // Simple static method
                return ReflectionCompiler.CreateStaticMethodInvoker(method);
            }

            // Path-based method: navigate through static members to reach the target
            // Note: The path can include static fields/properties, but the final method can be either
            // static or instance on the final object
            if (method.IsStatic)
            {
                // Final method is static - create static invoker
                var methodInvoker = ReflectionCompiler.CreateStaticMethodInvoker(method);
                return args =>
                {
                    object current = null;
                    for (int i = 0; i < pathSteps.Count; i++)
                    {
                        current = pathSteps[i].CompiledGetter(current);
                    }
                    return methodInvoker(args);
                };
            }
            else
            {
                // Final method is instance - create instance invoker
                var methodInvoker = ReflectionCompiler.CreateInstanceMethodInvoker(method);
                return args =>
                {
                    object current = null;
                    for (int i = 0; i < pathSteps.Count; i++)
                    {
                        current = pathSteps[i].CompiledGetter(current);
                    }
                    return methodInvoker(ref current, args);
                };
            }
        }

        /// <summary>
        /// Builds a delegate for invoking an instance method with parameters.
        /// </summary>
        /// <param name="targetType">The type containing the instance method.</param>
        /// <param name="parameterTypes">The parameter types for overload resolution. Can be null or empty for parameterless methods.</param>
        /// <returns>A delegate that invokes the instance method with parameters and returns the result.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        public InstanceInvoker BuildInstanceFunc(Type targetType, params Type[] parameterTypes)
        {
            var (method, pathSteps) = ResolveMethodInfo(targetType, isStatic: false, parameterTypes ?? Type.EmptyTypes);

            if (pathSteps.Count == 0)
            {
                // Simple instance method
                return ReflectionCompiler.CreateInstanceMethodInvoker(method);
            }

            // Path-based instance method
            var methodInvoker = ReflectionCompiler.CreateInstanceMethodInvoker(method);
            return delegate(ref object target, object[] args)
            {
                object current = target;
                for (int i = 0; i < pathSteps.Count; i++)
                {
                    current = pathSteps[i].CompiledGetter(current);
                }
                return methodInvoker.Invoke(ref current, args);
            };
        }

        /// <summary>
        /// Resolves the method info and path steps for the given target type and parameters.
        /// </summary>
        private (MethodInfo method, List<PathStep> pathSteps) ResolveMethodInfo(Type targetType, bool isStatic, Type[] parameterTypes)
        {
            // Check if the path contains dots (is a complex path)
            if (!_methodPath.Contains("."))
            {
                // Simple method name - use direct method resolution
                var resolvedMethod = targetType.FindMethod(_methodPath, MemberAccessFlags.All, parameterTypes);
                return (resolvedMethod, new List<PathStep>());
            }

            // Complex path - parse the path and get the final method
            var pathSteps = MemberPathParser.Parse(targetType, _methodPath, isStatic, finalStepIsMethod: true);

            // The last step should be a method
            var lastStep = pathSteps[^1];
            if (lastStep.StepType != PathStepType.Member || lastStep.Member is not MethodInfo)
            {
                throw new ArgumentException($"The last element in path '{_methodPath}' is not a method.");
            }

            var method = (MethodInfo)lastStep.Member;

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
