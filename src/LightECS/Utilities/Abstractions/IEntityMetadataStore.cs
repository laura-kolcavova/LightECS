﻿using LightECS.Utilities.Events;
using System.Diagnostics.CodeAnalysis;

namespace LightECS.Utilities.Abstractions;

internal interface IEntityMetadataStore
{
    public event EntityMetadataSetEventHandler? EntityMetadataSet;

    public event EntityMetadataRemovedEventHandler? EntityMetadataUnset;

    public EntityMetadata Get(
       Entity entity);

    public bool TryGet(
        Entity entity,
        [MaybeNullWhen(false)] out EntityMetadata entityMetadata);

    public void Set(
        Entity entity,
        Func<EntityMetadata> addEntityMetadataFactory,
        Func<EntityMetadata, EntityMetadata> updateEntityMetadataFactory);

    public void Remove(
        Entity entity);

    public bool Contains(
        Entity entity);
}
