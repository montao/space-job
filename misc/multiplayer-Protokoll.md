# some simple steps to work with multiplayer

## 1. set up Networkvariable

To have variable for evey player we need a Member ***Networkvariable*** with the datatype in our class. Also change Monobehaviour to ***Networkbehaviour***

## 2. create access to the variable

your need to overrite the *OnNetworkSpawn()* and *OnNetworkDespawn()* methodes with.

*m_LightStage.OnValueChanged += OnStateChange;* and<br>
*m_LightStage.OnValueChanged -= OnStateChange;*

This way we the Owner of the player can change the variable

## 3. Access Server-RPC

use the line

*[ServerRpc(RequireOwnership = false)]*