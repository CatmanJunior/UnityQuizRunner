using UnityEngine;

public class DebugToggle : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI label;
    [SerializeField] UnityEngine.UI.Toggle toggle;

    public delegate void ToggleChanged(bool value);
    public event ToggleChanged OnToggle;

    private void Start()
    {
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool value)
    {
        OnToggle?.Invoke(value);
    }

    public void SetLabel(string text)
    {
        label.text = text;
    }
    public void SetToggle(bool value)
    {
        if (toggle != null)
        {
            toggle.isOn = value;
        }
    }
}
