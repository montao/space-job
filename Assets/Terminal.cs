using TMPro;

public class Terminal : Interactable<bool> {
    public TMP_Text Text;

    public override void OnStateChange(bool previous, bool current) {
    }

    protected override void Interaction() {
    }

    public void DisplayError(string err) {
        Text.text = err;
    }
}
