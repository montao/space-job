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

## PSX-style shader

Based on https://github.com/Kodrin/URP-PSX, released under the MIT License.
