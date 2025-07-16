# LightECS

LightECS is a lightweight Entity Component System (ECS) library for .NET 8. It provides an API for managing entities and components.

> **Note:**  
> This project is a self-educational hobby project. It is not intended for production use, release, or to compete with or replicate features found in established ECS frameworks. Its purpose is to explore ECS concepts and C#/.NET design.

> **Limitation:**  
> The current implementation supports a maximum of **64 distinct component types** per entity context. This is a hard limit and cannot be exceeded.

Management of systems is intentionally not included in this library. The main reason is that designing a built-in system scheduler that is scalable, thread-safe, and flexible enough not to constrain users would significantly increase complexity. Therefore, system implementation and orchestration are left to the user.

## Features

- **Entity Management:** Create, remove, and query entities efficiently.
- **Component Stores:** Type-safe storage and retrieval of components with event support for added, updated, and removed components.
- **Events:** Subscribe to entity and component lifecycle events.
- **Queries:** Flexible querying of entities based on component composition.
- **Views:** Lazily-initialized, self-updating collections of entities matching a specific component composition. Automatically reflect changes as components are added or removed.
- **Context State:** Key-value storage for custom runtime data, enabling systems to share context-specific values.

## Basic Usage

```cs
using LightECS;

// Create an entity context
var context = new EntityContext();

// Create an entity
var entity = context.CreateEntity();

// Add a component
context.Set(entity, new Position { X = 10, Y = 20 });

// Retrieve a component
var position = context.Get<Position>(entity);

// Query entities with specific components
var query = context
  .UseQuery()
  .With<Position>();

foreach (var entityFromQuery in query.AsEnumerable())
{
  var pos = context.Get<Position>(entityFromQuery);
}

// Create a view from a specified query
var view = query.AsView();

foreach (var entityFromView in view.AsEnumerable())
{
  var pos = context.Get<Position>(entityFromView);
}

// Retrieve a component store
var positionStore = context.UseStore<Position>();

// Retrieve an entity store
var entityStore = context.UseEntityStore();

// Retrieve a context state
var state = context.State;

// Destroy an entity
context.DestroyEntity(entity);
```
