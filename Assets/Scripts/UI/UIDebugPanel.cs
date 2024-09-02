using System;
using UnityEngine;

public class UIDebugPanel : UIPanel
{
    //TODO: A way to open and close the debug panel
    [SerializeField] private DebugToggle debugTogglePrefab;
    [SerializeField]   private GameObject toggleParent;
    private GameStateHandler gameStateHandler;

    private void Start()
    {
        gameStateHandler = GameStateHandler.Instance;
        // CreateDebugToggle("Use Voting", Settings.skipVote, gameStateHandler.SetSkipVote);
    }

    private void CreateDebugToggle(string label, bool initialValue, Action<bool> onToggleAction)
    {
        DebugToggle debugToggle = Instantiate(debugTogglePrefab, transform);
        debugToggle.transform.SetParent(toggleParent.transform);
        debugToggle.SetLabel(label);
        debugToggle.SetToggle(initialValue);
        debugToggle.OnToggle += (value) => onToggleAction(value);
    }
}

