using System;

namespace EasyToolKit.Core
{
    #region Accessor Delegates (Non-Generic)

    /// <summary>
    /// Delegate for getting values from static members (fields or properties).
    /// </summary>
    /// <returns>The value retrieved from the static member.</returns>
    public delegate object StaticGetter();

    /// <summary>
    /// Delegate for getting values from instance members (fields or properties).
    /// </summary>
    /// <param name="instance">The instance to get the value from. Passed by reference to support value types.</param>
    /// <returns>The value retrieved from the instance member.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This ensures that modifications made to struct fields during chained member access are preserved.
    /// For reference types, this has no semantic difference but maintains consistency.
    /// </remarks>
    public delegate object InstanceGetter(ref object instance);

    /// <summary>
    /// Delegate for setting values to static members (fields or properties).
    /// </summary>
    /// <param name="value">The value to set.</param>
    public delegate void StaticSetter(object value);

    /// <summary>
    /// Delegate for setting values to instance members (fields or properties).
    /// </summary>
    /// <param name="instance">The instance containing the member to set. Passed by reference to support value types.</param>
    /// <param name="value">The value to set.</param>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// When setting properties on a struct instance, modifications would be lost without <c>ref</c> as the setter
    /// would operate on a copy. Using <c>ref</c> ensures the original instance is modified.
    /// </remarks>
    public delegate void InstanceSetter(ref object instance, object value);

    #endregion

    #region Accessor Delegates (Generic)

    /// <summary>
    /// Generic delegate for getting values from static members (fields or properties).
    /// </summary>
    /// <typeparam name="TValue">The type of value to retrieve.</typeparam>
    /// <returns>The value retrieved from the static member.</returns>
    public delegate TValue StaticGetter<TValue>();

    /// <summary>
    /// Generic delegate for getting values from instance members (fields or properties).
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TValue">The type of value to retrieve.</typeparam>
    /// <param name="instance">The instance to get the value from. Passed by reference to support value types.</param>
    /// <returns>The value retrieved from the instance member.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This ensures that:
    /// <list type="bullet">
    /// <item><description>No performance overhead from copying large structs</description></item>
    /// <item><description>Modifications to struct fields during chained member access are preserved</description></item>
    /// <item><description>The delegate signature works consistently for both reference and value types</description></item>
    /// </list>
    /// </remarks>
    public delegate TValue TypedInstanceGetter<TInstance, TValue>(ref TInstance instance);

    /// <summary>
    /// Generic delegate for setting values to static members (fields or properties).
    /// </summary>
    /// <typeparam name="TValue">The type of value to set.</typeparam>
    /// <param name="value">The value to set.</param>
    public delegate void StaticSetter<TValue>(TValue value);

    /// <summary>
    /// Generic delegate for setting values to instance members (fields or properties).
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TValue">The type of value to set.</typeparam>
    /// <param name="instance">The instance containing the member to set. Passed by reference to support value types.</param>
    /// <param name="value">The value to set.</param>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to ensure modifications are applied to the original instance.
    /// This is critical for value types (structs) because:
    /// <list type="bullet">
    /// <item><description>Property setters on structs would otherwise operate on a copy, losing the modification</description></item>
    /// <item><description>Without <c>ref</c>, changes to struct fields would be discarded after the setter returns</description></item>
    /// <item><description>Using <c>ref</c> ensures the setter modifies the actual struct storage location</description></item>
    /// </list>
    /// </remarks>
    public delegate void TypedInstanceSetter<TInstance, TValue>(ref TInstance instance, TValue value);

    #endregion

    #region Accessor Delegates (Generic - Object Instance)

    /// <summary>
    /// Generic delegate for getting values from instance members (fields or properties) with object instance.
    /// </summary>
    /// <typeparam name="TValue">The type of value to retrieve.</typeparam>
    /// <param name="instance">The instance to get the value from. Passed by reference to support value types.</param>
    /// <returns>The value retrieved from the instance member.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This provides a generic API for value types while keeping the instance as object.
    /// </remarks>
    public delegate TValue InstanceGetter<TValue>(ref object instance);

    /// <summary>
    /// Generic delegate for setting values to instance members (fields or properties) with object instance.
    /// </summary>
    /// <typeparam name="TValue">The type of value to set.</typeparam>
    /// <param name="instance">The instance containing the member to set. Passed by reference to support value types.</param>
    /// <param name="value">The value to set.</param>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to ensure modifications are applied to the original instance.
    /// This provides a generic API for value types while keeping the instance as object.
    /// </remarks>
    public delegate void InstanceSetter<TValue>(ref object instance, TValue value);

    #endregion

    #region Invoker Delegates (Non-Generic)

    /// <summary>
    /// Delegate for invoking static methods with variable parameters.
    /// </summary>
    /// <param name="args">The method arguments.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate object StaticFuncInvoker(params object[] args);

    /// <summary>
    /// Delegate for invoking instance methods with variable parameters.
    /// </summary>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="args">The method arguments.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This ensures that any modifications made to the instance by the method are preserved,
    /// especially important when calling mutating methods on struct instances.
    /// </remarks>
    public delegate object InstanceFuncInvoker(ref object instance, params object[] args);

    #endregion

    #region Invoker Delegates (Generic - Static Methods)

    /// <summary>
    /// Generic delegate for invoking parameterless static methods.
    /// </summary>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <returns>The return value of the method invocation.</returns>
    public delegate TResult StaticFuncInvoker<TResult>();

    /// <summary>
    /// Generic delegate for invoking static methods with one parameter.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="arg">The first parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate TResult StaticFuncInvoker<T, TResult>(T arg);

    /// <summary>
    /// Generic delegate for invoking static methods with two parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate TResult StaticFuncInvoker<T1, T2, TResult>(T1 arg1, T2 arg2);

    /// <summary>
    /// Generic delegate for invoking static methods with three parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate TResult StaticFuncInvoker<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3);

    /// <summary>
    /// Generic delegate for invoking static methods with four parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <param name="arg4">The fourth parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate TResult StaticFuncInvoker<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    /// <summary>
    /// Generic delegate for invoking static methods with five parameters.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <param name="arg4">The fourth parameter.</param>
    /// <param name="arg5">The fifth parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    public delegate TResult StaticFuncInvoker<T1, T2, T3, T4, T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    #endregion

    #region Invoker Delegates (Generic - Instance Methods with Object Instance)

    /// <summary>
    /// Generic delegate for invoking parameterless instance methods with object instance.
    /// </summary>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This provides a generic API for return types while keeping the instance as object.
    /// </remarks>
    public delegate TResult InstanceFuncInvoker<TResult>(ref object instance);

    /// <summary>
    /// Generic delegate for invoking instance methods with one parameter and object instance.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg">The first parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This provides a generic API for parameter and return types while keeping the instance as object.
    /// </remarks>
    public delegate TResult InstanceFuncInvoker<T, TResult>(ref object instance, T arg);

    /// <summary>
    /// Generic delegate for invoking instance methods with two parameters and object instance.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This provides a generic API for parameter and return types while keeping the instance as object.
    /// </remarks>
    public delegate TResult InstanceFuncInvoker<T1, T2, TResult>(ref object instance, T1 arg1, T2 arg2);

    /// <summary>
    /// Generic delegate for invoking instance methods with three parameters and object instance.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This provides a generic API for parameter and return types while keeping the instance as object.
    /// </remarks>
    public delegate TResult InstanceFuncInvoker<T1, T2, T3, TResult>(ref object instance, T1 arg1, T2 arg2, T3 arg3);

    /// <summary>
    /// Generic delegate for invoking instance methods with four parameters and object instance.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <param name="arg4">The fourth parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This provides a generic API for parameter and return types while keeping the instance as object.
    /// </remarks>
    public delegate TResult InstanceFuncInvoker<T1, T2, T3, T4, TResult>(ref object instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    /// <summary>
    /// Generic delegate for invoking instance methods with five parameters and object instance.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <param name="arg4">The fourth parameter.</param>
    /// <param name="arg5">The fifth parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This provides a generic API for parameter and return types while keeping the instance as object.
    /// </remarks>
    public delegate TResult InstanceFuncInvoker<T1, T2, T3, T4, T5, TResult>(ref object instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    #endregion

    #region Invoker Delegates (Generic - Instance Methods)

    /// <summary>
    /// Generic delegate for invoking parameterless instance methods.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This ensures that any modifications made to the struct instance by the method are preserved.
    /// For reference types, this maintains consistency without semantic difference.
    /// </remarks>
    public delegate TResult TypedInstanceFuncInvoker<TInstance, TResult>(ref TInstance instance);

    /// <summary>
    /// Generic delegate for invoking instance methods with one parameter.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg">The first parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This ensures that any mutating operations performed on struct instances are applied to the original,
    /// preventing loss of changes that would occur if operating on a copy.
    /// </remarks>
    public delegate TResult TypedInstanceFuncInvoker<TInstance, T, TResult>(ref TInstance instance, T arg);

    /// <summary>
    /// Generic delegate for invoking instance methods with two parameters.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This ensures that modifications to struct instances are preserved, avoiding the common pitfall where
    /// struct mutations are lost because they were performed on a copy of the value.
    /// </remarks>
    public delegate TResult TypedInstanceFuncInvoker<TInstance, T1, T2, TResult>(ref TInstance instance, T1 arg1, T2 arg2);

    /// <summary>
    /// Generic delegate for invoking instance methods with three parameters.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This is essential for preserving state changes on struct instances, ensuring that mutations are not lost
    /// due to the method operating on a copy rather than the original value.
    /// </remarks>
    public delegate TResult TypedInstanceFuncInvoker<TInstance, T1, T2, T3, TResult>(ref TInstance instance, T1 arg1, T2 arg2, T3 arg3);

    /// <summary>
    /// Generic delegate for invoking instance methods with four parameters.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <param name="arg4">The fourth parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This prevents performance degradation from copying large structs and ensures that any modifications
    /// made to the instance during method invocation are applied to the original storage location.
    /// </remarks>
    public delegate TResult TypedInstanceFuncInvoker<TInstance, T1, T2, T3, T4, TResult>(ref TInstance instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    /// <summary>
    /// Generic delegate for invoking instance methods with five parameters.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="TResult">The return type of the method.</typeparam>
    /// <param name="instance">The instance to invoke the method on. Passed by reference to support value types.</param>
    /// <param name="arg1">The first parameter.</param>
    /// <param name="arg2">The second parameter.</param>
    /// <param name="arg3">The third parameter.</param>
    /// <param name="arg4">The fourth parameter.</param>
    /// <param name="arg5">The fifth parameter.</param>
    /// <returns>The return value of the method invocation.</returns>
    /// <remarks>
    /// The instance parameter is passed by reference (<c>ref</c>) to avoid unnecessary copying of value types (structs).
    /// This ensures that state mutations on struct instances are preserved and prevents unintended behavior
    /// where changes would be lost if the method operated on a copy of the struct.
    /// </remarks>
    public delegate TResult TypedInstanceFuncInvoker<TInstance, T1, T2, T3, T4, T5, TResult>(ref TInstance instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    #endregion
}
