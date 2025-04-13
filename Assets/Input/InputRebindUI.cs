using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputRebindUI : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    [Header("Bindings")]
    [SerializeField] private TextMeshProUGUI jumpBindingLabel;
    [SerializeField] private Button jumpRebindButton;

    [SerializeField] private TextMeshProUGUI attackBindingLabel;
    [SerializeField] private Button attackRebindButton;

    [SerializeField] private TextMeshProUGUI moveUpBindingLabel;
    [SerializeField] private Button moveUpRebindButton;

    [SerializeField] private TextMeshProUGUI moveDownBindingLabel;
    [SerializeField] private Button moveDownRebindButton;

    [SerializeField] private TextMeshProUGUI moveLeftBindingLabel;
    [SerializeField] private Button moveLeftRebindButton;

    [SerializeField] private TextMeshProUGUI moveRightBindingLabel;
    [SerializeField] private Button moveRightRebindButton;

    [SerializeField] private Button resetRebingsButton;

    private void Start()
    {
        UpdateAllBindingLabels();

        jumpRebindButton.onClick.AddListener(() => Rebind("Jump", jumpBindingLabel));
        attackRebindButton.onClick.AddListener(() => Rebind("Attack", attackBindingLabel));
        moveUpRebindButton.onClick.AddListener(() => Rebind("Move", moveUpBindingLabel, "up"));
        moveDownRebindButton.onClick.AddListener(() => Rebind("Move", moveDownBindingLabel, "down"));
        moveLeftRebindButton.onClick.AddListener(() => Rebind("Move", moveLeftBindingLabel, "left"));
        moveRightRebindButton.onClick.AddListener(() => Rebind("Move", moveRightBindingLabel, "right"));

        resetRebingsButton.onClick.AddListener(() =>
        {
            ResetBindingsToDefault();
        });
    }

    private void Rebind(string action, TextMeshProUGUI label, string bindingName = "")
    {
        inputReader.StartRebind(action, bindingName, newBinding =>
        {
            label.text = inputReader.GetBindingDisplayName(action, bindingName);
        });
    }

    private void UpdateAllBindingLabels()
    {
        jumpBindingLabel.text = inputReader.GetBindingDisplayName("Jump");
        attackBindingLabel.text = inputReader.GetBindingDisplayName("Attack");
        moveUpBindingLabel.text = inputReader.GetBindingDisplayName("Move", "up");
        moveDownBindingLabel.text = inputReader.GetBindingDisplayName("Move", "down");
        moveLeftBindingLabel.text = inputReader.GetBindingDisplayName("Move", "left");
        moveRightBindingLabel.text = inputReader.GetBindingDisplayName("Move", "right");
    }

    private void ResetBindingsToDefault()
    {
        inputReader.ResetBindings();
        UpdateAllBindingLabels();
    }
}
