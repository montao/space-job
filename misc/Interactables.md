# A Rough Guide To Interactables

## Droppable Interactables

An interactable that can be dropped consists of three different
GameObjects that reside on different layers.

Here is the structure for the Cup object, including layers and the
essential scripts on each of the three objects:

```
Cup (layer: IgnorePlayerCollision)
│ * Rigidbody w/ appropriate colliders, i.e. non-trigger colliders
│   matching the physical shape
│ * NetworkTransform w/ NetworkObject
│ * FixMouseOverNotPassingToChildren (to let mouseovers pass on to the
│   child with the Interactable script)
└── Interaction (layer: Default)
    │ * Mesh Renderer
    │ * Script inheriting from DroppableInteractable
    │ * Trigger Collider for mouse over (usually slightly larger than
    │   object)
    └── InteractionRange (layer: IgnoreRaycast)
          * Trigger Collider the player must be inside of to interact
```

When changing the layers take care *not* to change the layers of
children!

A note on scale:  Any scaling should be applied to the root object (i.e.
the one on the IgnorePlayerCollision layer), the other two object should
have a scale of 1.  Otherwise, the item will not be correctly scaled in
the player's hand/on their back.

The three-object thing is a bit awkward, but we do need three different
layers:  The Rigidbody must be on the IgnorePlayerCollision layer,
otherwise the player would bump into the object or be able to use it a
ramp etc. The layer of the GameObject with the Interactable script gets
changed from `Default` to `Highlight` in order to apply the highlight
shader. The InteractionRange must be on IgnoreRaycast in order to not
swallow the mouseover.
