using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TerminalConsoleBehavior : MonoBehaviour
{
   [SerializeField] private ConsoleCommand[] commands = new ConsoleCommand[0];
    [Header("UI")]
    [SerializeField] private GameObject uiCanvas = null;
    [SerializeField] private TMP_InputField inputField = null;
    [SerializeField] private TMP_InputField outputField = null;
    [SerializeField] private TextMeshProUGUI hostnamePrefix = null;

    private static TerminalConsoleBehavior instance;

    private TerminalConsole terminalConsole;

    private TerminalConsole TerminalConsole
    {
        get
        {
            if (terminalConsole != null)
            {
                return terminalConsole;
            }

            return terminalConsole = new TerminalConsole(commands);

        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ProcessComand()
    {
        TerminalConsole.ProcessCommand(inputField.text);
        outputField.text += hostnamePrefix.text + " " + inputField.text + "\n";
        inputField.text = string.Empty;
    }
}
