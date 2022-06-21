using UnityEngine;

public class RevivePodAnimation : MonoBehaviour {
    private Animator m_Animator;
    private enum Parameters {
        INSERT_FLOPPY,
        SAFETY_IS_OPEN,
        DOOR_IS_OPEN,
    }

    void Awake() {
        m_Animator = GetComponent<Animator>();
    }

    public void TriggerFloppyInsert() {
        m_Animator.SetTrigger(Parameters.INSERT_FLOPPY.ToString().ToLower());
    }

    public void SetSafetyOpen(bool is_open) {
        m_Animator.SetBool(Parameters.SAFETY_IS_OPEN.ToString().ToLower(), is_open);
    }

    public void SetDoorOpen(bool is_open) {
        m_Animator.SetBool(Parameters.DOOR_IS_OPEN.ToString().ToLower(), is_open);
    }
}
