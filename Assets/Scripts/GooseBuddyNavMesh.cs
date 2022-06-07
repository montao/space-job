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

    public enum GooseState {
        ITEM_GET,
        CHASE_PLAYER,
        ASLEEP,
        DANCE,
    }

    public static GooseState RandomState() {
        int i = UnityEngine.Random.Range(0, Enum.GetValues(typeof(GooseState)).Length);
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

    private GooseState __set_via_properties_State = GooseState.ASLEEP;
    public GooseState State {
        get => __set_via_properties_State;
        set {
            var prev = __set_via_properties_State;
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
            }

            Debug.Log("Goose is now " + value);

            __set_via_properties_State = value;
        }
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
            State = RandomState();
        }
    }

    private IEnumerator GooseLoop() {
        while (true) {
            State = RandomState();
            yield return new WaitForSeconds(20f);
        }
    }

}
