using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerminalConsoleBehavior : MonoBehaviour
{
    //[SerializeField] private ConsoleCommand[] commands = new ConsoleCommand[0];
    [SerializeField] private List<ConsoleCommand> commands = new List<ConsoleCommand>();
    [Header("UI")]
    //[SerializeField] private GameObject uiCanvas = null;
    [SerializeField] public TMP_InputField inputField = null;
    [SerializeField] private TMP_InputField outputField = null;
    [SerializeField] private TextMeshProUGUI hostnamePrefix = null;
    [SerializeField] private GameObject cluesPanel = null; 
    [SerializeField] private GameObject cluePrefab = null;
    [Header("VARIABLES")]
    [SerializeField] public GameObject currentObj = null;
    [SerializeField] public TerminalPrivileges.privileges currentPrivilege = TerminalPrivileges.privileges.user;
    [SerializeField] public TerminalPrivileges.specificConfig currentConfigLevel = TerminalPrivileges.specificConfig.global;
    [SerializeField] public string localUsername;
    [SerializeField] public string localPassword;
    [SerializeField] public string enablePassword;
    [SerializeField] public string MOTD;
    public int enteringLocalWhat; //0: enter USERNAME, 1: enter PASSWORD, 2:no user and pass, just press RETURN
    public bool authenticatingEnable;
    public bool isCMD;
    public static TerminalConsoleBehavior instance;
    public List<string> currentClues;
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

       
        var loadedCommands = Resources.LoadAll<ConsoleCommand>("TerminalCommands");

        foreach(var command in loadedCommands)
        {
            commands.Add(command);
        }
    }

   
    public void OnTextChange(TMP_InputField InputText)
    {
        string typedCommand = InputText.text;
       
        if (typedCommand.EndsWith("\n"))
        {
            string typedCommandWOend = typedCommand.Remove(typedCommand.Length - 1);
            inputField.text = string.Empty;
            //IF LOGGED IN
            if (currentPrivilege > TerminalPrivileges.privileges.loggedOut)
            {
                ProcessComand(typedCommandWOend, true);
            }
            else //IF LOGGED OUT
            {
                if(enteringLocalWhat == 2)
                {
                    currentPrivilege = TerminalPrivileges.privileges.user;
                    ProcessComand("", false);
                }
                else if(enteringLocalWhat == 0)
                {
                    if (typedCommandWOend == localUsername)
                    {
                        enteringLocalWhat = 1;
                        ProcessComand(typedCommandWOend, false);
                    }
                    else
                    {
                        ProcessComand(typedCommandWOend, false);
                        printToTerminal("Invalid username.");
                        
                    }
                }
                else if (enteringLocalWhat == 1)
                {
                    if (!authenticatingEnable)//LOCAL PASSWORD CHECK
                    {
                        if (typedCommandWOend == localPassword)
                        {
                            currentPrivilege = TerminalPrivileges.privileges.user;
                            ProcessComand(typedCommandWOend, false);
                            enteringLocalWhat = 0;
                        }
                        else
                        {
                            ProcessComand(typedCommandWOend, false);
                            printToTerminal("Invalid password.");
                            enteringLocalWhat = 0;
                            TerminalCanvasScript.instance.updateHostnamePrefix();
                        }
                    }
                    else//ENABLE PASSWORD CHECK
                    {
                        if (typedCommandWOend == enablePassword)
                        {
                            currentPrivilege = TerminalPrivileges.privileges.privileged;
                            ProcessComand(typedCommandWOend, false);
                            authenticatingEnable = false;
                            enteringLocalWhat = 0;
                        }
                        else
                        {
                            ProcessComand(typedCommandWOend, false);
                            printToTerminal("Invalid password.");
                        }
                    }
                   
                }
            }
            
        }

        // CLUES BEHAVIOR

        while (cluesPanel.transform.childCount > 0)
        {
            DestroyImmediate(cluesPanel.transform.GetChild(0).gameObject);
        }
        currentClues.Clear();
        bool noClues = true;

        string[] typedCommandSplit = typedCommand.Split(" ");

        string typedCommandSplitLast = typedCommandSplit[typedCommandSplit.Length - 1];
        //Debug.Log("TYPED COMMAND LENGTH: "+typedCommandSplit.Length);

        if (inputField.text.Length > 0)
        {
            foreach (ConsoleCommand cc in commands)
            {
                if (typedCommandSplit.Length==1 && cc.CommandWord.Contains(typedCommandSplitLast) &&
                    typedCommandSplitLast != cc.CommandWord &&
                    ((instance.currentPrivilege == cc.CommandPrivilege && cc.specificConfig == TerminalPrivileges.specificConfig.global) ||
                cc.CommandPrivilege == TerminalPrivileges.privileges.all && currentPrivilege != TerminalPrivileges.privileges.cmd||
                (instance.currentPrivilege == cc.CommandPrivilege && cc.specificConfig == instance.currentConfigLevel)))

                {
                    cluesPanel.SetActive(true);
                    //Debug.Log("found " + cc.CommandWord);
                    GameObject prefab = Instantiate(cluePrefab, cluesPanel.transform);
                    prefab.GetComponent<cluesString>().setText(cc.CommandWord, typedCommandSplitLast, this);
                    noClues = false;
                }
                if (typedCommandSplit.Length > 1 && cc.CommandWord==typedCommandSplit[0])
                {
                    var SOargs = cc.CommandArgs;
                    foreach (string a in SOargs)
                    {
                        var argsSplit = a.Split(":");
                        if(typedCommandSplit.Length == argsSplit.Length + 1)
                        {
                            //Debug.Log("typedCommandSplitLast: " + typedCommandSplitLast);
                            if(typedCommandSplit.Length >2)
                            {
                                if (argsSplit[typedCommandSplit.Length - 2].Contains(typedCommandSplitLast) &&
                                    string.Join(" ", argsSplit).Contains(typedCommandSplit[typedCommandSplit.Length - 2]) &&
                                    typedCommandSplitLast != argsSplit[typedCommandSplit.Length - 2] &&
                                    !currentClues.Contains(argsSplit[typedCommandSplit.Length - 2]))
                                {
                                    cluesPanel.SetActive(true);
                                    GameObject prefab = Instantiate(cluePrefab, cluesPanel.transform);
                                    prefab.GetComponent<cluesString>().setText(argsSplit[typedCommandSplit.Length - 2], typedCommandSplitLast, this);
                                    prefab.GetComponent<cluesString>().args = a;
                                    prefab.GetComponent<cluesString>().argsBaseCommand = cc.CommandWord;
                                    currentClues.Add(argsSplit[typedCommandSplit.Length - 2]);
                                    noClues = false;
                                }
                            }
                            else
                            {
                                if (argsSplit[0].Contains(typedCommandSplitLast) && typedCommandSplitLast != argsSplit[0] &&
                                    !currentClues.Contains(argsSplit[0]))
                                {
                                    cluesPanel.SetActive(true);
                                    GameObject prefab = Instantiate(cluePrefab, cluesPanel.transform);
                                    prefab.GetComponent<cluesString>().setText(argsSplit[0], typedCommandSplitLast, this);
                                    prefab.GetComponent<cluesString>().args = argsSplit[0];
                                    prefab.GetComponent<cluesString>().argsBaseCommand = cc.CommandWord;
                                    currentClues.Add(argsSplit[0]);
                                    noClues = false;
                                }
                            }
                            
                        }
                        
                    }
                }
            }
        }
        else
        {
            noClues=true;
        }
        if (noClues)
        {
            cluesPanel.SetActive(false);
        }

    }
 
    public void ProcessComand(string commandInput,bool flagIfInvalid)
    {
        outputField.text += hostnamePrefix.text + commandInput + "\n";
        TerminalConsole.ProcessCommand(commandInput, flagIfInvalid);

        if (!isCMD)
        {
            TerminalCanvasScript.instance.updateHostnamePrefix();
        }
    }

    public string getPrivilegePrefix()
    {
        string prefixChar;
        prefixChar = TerminalPrivileges.priviledgePrefix[(int)instance.currentPrivilege];

        string configPrefix;

        configPrefix = TerminalPrivileges.configPrefix[(int)instance.currentConfigLevel];

        if (instance.currentPrivilege < TerminalPrivileges.privileges.config)
        {
            return prefixChar;
        }
        else if (instance.currentPrivilege == TerminalPrivileges.privileges.config)
        {
            return "(" + configPrefix + ")" + prefixChar;
        }
        else
        {
            return "";
        }

    }

    public static void printToTerminal(string stringToPrint)
    {
        instance.outputField.text += stringToPrint+"\n";
    }

    public void saveVarsToCisco()
    {
        if (!isCMD)
        {
            CiscoDevice ciscoDevice = currentObj.GetComponent<CiscoDevice>();
            ciscoDevice.currentPrivilege = currentPrivilege;
            ciscoDevice.currentConfigLevel = currentConfigLevel;
            ciscoDevice.localUsername = localUsername;
            ciscoDevice.localPassword = localPassword;
            ciscoDevice.enablePassword = enablePassword;
            ciscoDevice.MOTD = MOTD;
            ciscoDevice.enteringLocalWhat = enteringLocalWhat; //0: enter USERNAME, 1: enter PASSWORD, 2:no user and pass, just press RETURN
            ciscoDevice.authenticatingEnable = authenticatingEnable;
            ciscoDevice.terminalContent = outputField.text;
        }
        else
        {
            isCMD = false;
            PCBehavior pcBehavior = currentObj.GetComponent<PCBehavior>();
            pcBehavior.terminalContent = outputField.text;
        }
    }
    public void getVarsFromCisco()
    {

        if (!isCMD){
            CiscoDevice ciscoDevice = currentObj.GetComponent<CiscoDevice>();
            currentPrivilege = ciscoDevice.currentPrivilege;
            currentConfigLevel = ciscoDevice.currentConfigLevel;
            localUsername = ciscoDevice.localUsername;
            localPassword = ciscoDevice.localPassword;
            enablePassword = ciscoDevice.enablePassword;
            MOTD = ciscoDevice.MOTD;
            enteringLocalWhat = ciscoDevice.enteringLocalWhat; //0: enter USERNAME, 1: enter PASSWORD, 2:no user and pass, just press RETURN
            authenticatingEnable = ciscoDevice.authenticatingEnable;
            outputField.text = ciscoDevice.terminalContent;
        }
        else
        {
            PCBehavior pcBehavior = currentObj.GetComponent<PCBehavior>();
            outputField.text = pcBehavior.terminalContent;
        }
    }

    public void openCMDPrompt()
    {
        Debug.Log("opening cmd");
        currentObj = DesktopCanvasScript.instance.currentPC;
        TerminalCanvasScript.instance.gameObject.SetActive(true);
        TerminalCanvasScript.instance.hostnameLabel.text = "C:\\>";
        TerminalCanvasScript.instance.deviceLabel.text = DesktopCanvasScript.instance.currentPC.name;
        currentPrivilege = TerminalPrivileges.privileges.cmd;
        isCMD = true;
        getVarsFromCisco();
    }
}
