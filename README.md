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

## Why use this method?

These extension methods allow port connections to be handled by the port itself or by their containing nodes, rather than by the graph view. This streamlines the logic for creating connection handlers depending on the use case, and helps modularity and clear code organization.

The existing `Port` API is lacking a way to hook events to port connections, and developers are expected to do this in the `GraphView`. Depending on the use case, this can be long-winded and split up logic in an unintuitive way. Ports actually already have `OnConnect` and `OnDisconnect` events, but are not publically accessible due to being marked `internal`.

Due to the GraphView package being [originally built for Unity's Shader Graph and Visual Effect Graph](https://forum.unity.com/threads/graph-port-api-onconnect-disconnect-are-internal.1315425/#post-8321505), the publically accessible API is tailored to what was needed for these packages. As a result, many useful parts of the API are marked `internal` and therefore inaccessible.

Thanks to this [workaround by adamgit](https://github.com/adamgit/PublishersFork/blob/main/EngineForks/WorkaroundUnityInternal.cs), we can expose internal engine functionality and hook callbacks to these internal events.

## Preexisting method (Without using this repo)

This section is for those who simply wish to create a port connection handler using the pre-existing GraphView API and have no interest in using the code in this repo:

1. In your custom graph view class inheriting from `GraphView`, override `OnGraphViewChanged`. This will give access to pending operations on the graph view.
2. Use `GraphViewChange.edgesToCreate` to reference newly created edges
3. Use `GraphViewChange.elementsToRemove` to get all `GraphElements` about to be removed, then filter them by type `Edge` to get all edges about to be deleted.
4. Write your custom logic