# Connections Changed Callback for GraphView Ports

Provides extension methods for Unity's [experimental GraphView package](https://docs.unity3d.com/2023.2/Documentation/ScriptReference/Experimental.GraphView.GraphView.html), allowing callbacks to subscribe to when the connections on a `Port` are changed.

## Requirements

- [adamgit's workaround for accessing internal Unity functionality](https://github.com/adamgit/PublishersFork/blob/main/EngineForks/WorkaroundUnityInternal.cs)


## Usage

Registered callbacks will be invoked when a connection is added or removed on the port.

```cs
// Register callback
port.RegisterConnectionChangedCallback(onPortChanged)

// Unregister callback
port.UnregisterConnectionsChangedCallback(onPortChanged)
```

```cs
// Example: Current port is an input port, and we
// want to operate on all connected output nodes
void onPortChanged(Port port)
{
    var connections = port.connections;
    var connectedNodes = connections
                .Select(edge => edge.output.node);
    
    // Handle event here
}
```

## Why?

[Unity's experimental GraphView package was originally built for Unity's Shader Graph and Visual Effect Graph](https://forum.unity.com/threads/graph-port-api-onconnect-disconnect-are-internal.1315425/#post-8321505). As a consequence, the public API is tailored to what was needed for these packages. Many useful parts of the API are marked with `internal` access.

This includes the internal `OnConnect` and `OnDisconnect` events on `GraphView.Port` 

Thanks to this [workaround by adamgit](https://github.com/adamgit/PublishersFork/blob/main/EngineForks/WorkaroundUnityInternal.cs), we can expose internal engine functionality. Now there can be public methods to register callbacks to when the graph is changed.

In many situations, this is more desirable than overriding `GraphView.OnGraphViewChanged` as it often makes sense to have the event handler for port connection changes on the port itself.
