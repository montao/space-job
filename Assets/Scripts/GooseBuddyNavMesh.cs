using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GooseBuddyNavMesh : MonoBehaviour {

    private NavMeshAgent m_GooseNavMesh;
    private Collider m_FollowingRange;
    private Transform m_Target;

    public Animator GooseAnimator;
    public Animator RadarAnimator;

    public LayerMask GroundLayer;
    public Transform ItemDepositTarget;  // create an empty child here

    public enum GooseState {
        ITEM_GET,
        ITEM_DEPOSIT,  // called when
        CHASE_PLAYER,
        ASLEEP,
        DANCE,
    }

    public static GooseState RandomState() {
        int i = UnityEngine.Random.Range(0, Enum.GetValues(typeof(GooseState)).Length);
        //Debug.Log("Goose " + i);
        return (GooseState)Enum.ToObject(typeof(GooseState), i);
    }
    public static Transform RandomTarget(GooseState state) {
        List<Transform> targets = new List<Transform>();
        if (state == GooseState.CHASE_PLAYER) {
            foreach (var pplayer in PlayerManager.Instance.Players) {
                Transform t = pplayer.Avatar.transform;
                targets.Add(t);
            }
        } else if (state == GooseState.ITEM_GET) {
            foreach (var item in GameObject.FindObjectsOfType<DroppableInteractable>()) {
                targets.Add(item.transform);
            }
        }

        int i = UnityEngine.Random.Range(0, targets.Count);
        if (i < 0) {
            return null;
        }
        return targets[i];
    }

    // only set this via the property, or the goose will die!
    private GooseState __State = GooseState.ASLEEP;
    public GooseState State {
        get => __State;
        set {
            var prev = __State;
            if (prev == value) return;

            bool awake = value != GooseState.ASLEEP;
            GooseAnimator.SetBool("GooseAwake", awake);
            RadarAnimator.SetBool("GooseAwake", awake);
            GooseAnimator.SetBool("Dance", value == GooseState.DANCE);

            if (value == GooseState.ASLEEP || value == GooseState.DANCE) {
                m_Target = null;
            } else if (value == GooseState.ITEM_GET || value == GooseState.CHASE_PLAYER) {
                m_GooseNavMesh.stoppingDistance = (value == GooseState.CHASE_PLAYER) ? 1f : 0f;
                m_Target = RandomTarget(value);
            } else if (value == GooseState.ITEM_DEPOSIT) {
                m_GooseNavMesh.stoppingDistance = 0f;
                ItemDepositTarget.position = RandomPointOnNavMesh(max_distance: 80);
                m_Target = ItemDepositTarget;
            }

            Debug.Log("Goose is now " + value);

            __State = value;
        }
    }

    private Vector3 RandomPointOnNavMesh(float max_distance) {
        if (max_distance < 0) {
            Debug.LogWarning("No target position found for the Goose :(");
            return transform.position;
        }

        Vector3 direction = UnityEngine.Random.insideUnitSphere * max_distance;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position + direction, out hit, max_distance, GroundLayer)) {
            return hit.position;
        }

        // Debug.Log("No Goose destination found, trying again with maxdist=" + (max_distance - 10) + "...");
        return RandomPointOnNavMesh(max_distance - 10);
    }

    void Start() {
        m_GooseNavMesh = GetComponent<NavMeshAgent>();
        m_FollowingRange = GetComponent<Collider>();
        StartCoroutine(GooseLoop());
    }

    // idle through ship
    // follow player
    // steal mission (priorieat)



    private void OnCollisionEnter(Collision other) {
        Debug.Log(m_Target.position + "; " + other.gameObject.name);
        if (other.gameObject.GetComponent<PlayerAvatar>() != null){
            m_Target = other.transform;
        }
    }

    void Update() {
        if (m_Target != null){
            m_GooseNavMesh.destination = m_Target.position;
        } else {
            m_GooseNavMesh.destination = transform.position;
        }
        GooseAnimator.SetFloat("Speed", m_GooseNavMesh.velocity.magnitude);
        if (State == GooseState.ITEM_GET && m_GooseNavMesh.remainingDistance < 0.05f) {
            State = GooseState.ITEM_DEPOSIT;
        }
    }

    private IEnumerator GooseLoop() {
        while (true) {
            State = RandomState();
            yield return new WaitForSeconds(5f);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        if (m_Target) {
            Gizmos.DrawCube(m_Target.position, Vector3.one);
        }
    }

}
