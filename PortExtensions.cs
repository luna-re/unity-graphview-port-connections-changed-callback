
namespace UnityEditor.Experimental.GraphView
{
    using System;
    using System.Reflection;
    using PublishersFork;
    
    /// <summary>
    /// Extension methods for working with ports in Unity's GraphView.
    /// </summary>
    public static class PortExtensions
    {
        /// <summary>
        /// Registers a callback to be invoked when connections change on the port.
        /// </summary>
        /// <param name="port">The port to register the callback for.</param>
        /// <param name="callback">The callback to be invoked when connections change.</param>
        public static void RegisterConnectionsChangedCallback(this Port port, Action<Port> callback)
        {
            void RegisterToInternalEvent(string fieldName, Action<Port> callback)
            {
                FieldInfo baseField = WorkaroundUnityInternal.FindPrivateField(port, fieldName);
                Action<Port> baseFieldValue = (Action<Port>)baseField.GetValue(port);
                baseFieldValue += callback;
                baseField.SetValue(port, baseFieldValue);
            }

            RegisterToInternalEvent("OnConnect", callback);
            RegisterToInternalEvent("OnDisconnect", callback);
        }

        /// <summary>
        /// Unregisters a callback from connection changes on the port.
        /// </summary>
        /// <param name="port">The port to unregister the callback from.</param>
        /// <param name="callback">The callback to unregister.</param>
        public static void UnregisterConnectionsChangedCallback(this Port port, Action<Port> callback)
        {
            void UnregisterFromInternalEvent(string fieldName, Action<Port> callback)
            {
                FieldInfo baseField = WorkaroundUnityInternal.FindPrivateField(port, fieldName);
                Action<Port> baseFieldValue = (Action<Port>)baseField.GetValue(port);
                baseFieldValue -= callback;
                baseField.SetValue(port, baseFieldValue);
            }

            UnregisterFromInternalEvent("OnConnect", callback);
            UnregisterFromInternalEvent("OnDisconnect", callback);
        }
    }
}