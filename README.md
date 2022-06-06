# space-Job

## Unity Setup

For setting up Unity on your Linux machine, please refer to
[this guide](https://github.com/GooseGirlGames/steves-job/blob/main/doc/UnitySetup.md#unity-setup-linux)
over in the Steve's Job repo.  The file also includes fixes for common
errors, including crashes and the dreaded double empty errors.

## Multiplayer

Multiplayer uses [Netcode for GameObjects](https://github.com/Unity-Technologies/com.unity.netcode.gameobjects),
which is included here as a git submodule.  After cloning this repo, run the following commands to clone the submodule:

```sh
git submodule init
git submodule update
```

## Lightmap and Shadowmap Baking

After initially cloning the repo, and anytime you change baked lights,
head to `Window->Rendering->Lighting`.  Set a good *Max Lightmap Size*
(32 for development, 1024 for release), and hit `Generate Lighting`.

## Interactables

See [misc/Interactables.md](misc/Interactables.md) for infomation on how
to create interactable objects.

## 3D Workflow

See [misc/3d\_workflow.md](misc/3d_workflow.md) for infomation on how to
export Blender models into Unity.

## PSX-style shader

Based on https://github.com/Kodrin/URP-PSX, released under the MIT License.
