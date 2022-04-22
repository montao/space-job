# some simple steps to work with multiplayer

## 1. set up Networkvariable

To have variable for evey player we need a Member ***Networkvariable*** with the datatype in our class. Also change Monobehaviour to ***Networkbehaviour***

## 2. create access to the variable

your need to overrite the *OnNetworkSpawn()* and *OnNetworkDespawn()* methodes with.

*m_LightStage.OnValueChanged += OnStateChange;* and<br>
*m_LightStage.OnValueChanged -= OnStateChange;*

This way we the Owner of the player can change the variable. *OnStateChange* will be called when the value of the variable changes.

## 3. Access Server-RPC

use the line

*[ServerRpc(RequireOwnership = false)]*

Before activly manipulating The Value of the state. This way all players are alowed to interakt with it. Afterwords just write the rest of your class how you like.

## 4. Create a Prefab

jeah, just create a gameobject with the script an everything and make a prefab out of that.

## 5. Add to NetworkManager

Go in the networkmanager and add the prefab