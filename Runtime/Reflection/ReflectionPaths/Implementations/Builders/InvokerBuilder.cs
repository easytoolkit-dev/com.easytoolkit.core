using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Builder for creating method invokers that call methods with optional parameters.
    /// </summary>
    public sealed class InvokerBuilder : ReflectionBuilderBase, IInvokerBuilder
    {
        /// <summary>
        /// Initializes a new instance of the InvokerBuilder.
        /// </summary>
        /// <param name="methodPath">The path to the method (e.g., "Method", "Child.Object.Method()").</param>
        public InvokerBuilder(string methodPath) : base(methodPath)
        {
        }

        /// <inheritdoc/>
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
                    return methodInvoker(current, args);
                };
            }
        }

        /// <inheritdoc/>
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
            return (target, args) =>
            {
                object current = target;
                for (int i = 0; i < pathSteps.Count; i++)
                {
                    current = pathSteps[i].CompiledGetter(current);
                }
                return methodInvoker.Invoke(current, args);
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
                var resolvedMethod = targetType.ResolveOverloadMethod(MemberPath, MemberAccessFlags.All, parameterTypes);
                return (resolvedMethod, new List<PathStep>());
            }

            // Complex path - parse the path and get the final method
            var pathSteps = MemberPathParser.Parse(targetType, MemberPath, isStatic, finalStepIsMethod: true);

            // The last step should be a method
            var lastStep = pathSteps[^1];
            if (lastStep.StepType != PathStepType.Member || lastStep.Member is not MethodInfo)
            {
                throw new ArgumentException($"The last element in path '{MemberPath}' is not a method.");
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
