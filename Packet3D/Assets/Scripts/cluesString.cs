using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
public class cluesString : MonoBehaviour
{
    public TextMeshProUGUI clueText;
    private TerminalConsoleBehavior terminalConsoleBehavior;
    private string typed;
    private string filteredCommandWord;
    public string args;
    public string argsBaseCommand;
    private void Awake()
    {
        clueText.text = "";
    }
    public void setText(string filteredCommandWord,string typed, TerminalConsoleBehavior terminal)
    {
        //for (int i = 0; i < txt.Length; i++)
        //{
        //    bool isThere = false;
        //    for (int j = 0; j < typed.Length; j++)
        //    {
        //        if (txt[i] == typed[j])
        //        {
        //            clueText.text += "<#00ffff>" + txt[i] + "</color>";
        //            isThere = true;
        //        }
        //    }
        //    if (!isThere)
        //    {
        //        clueText.text += txt[i];
        //    }
        //}
        terminalConsoleBehavior = terminal;

        string output = Regex.Replace(filteredCommandWord, typed,
          match => "<#00ffff>" + match.Value + "</color>",
          RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        clueText.text = output;
        this.typed = typed;
        this.filteredCommandWord = filteredCommandWord;
    }

    public void clueClick()
    {
        if (args == "")
        {
            Debug.Log("autocorrecting " +filteredCommandWord);
            terminalConsoleBehavior.inputField.text = filteredCommandWord;

        }
        else
        {
            
            string[] argsSplit = args.Split(":");
            Debug.Log("autocorrecting args " + string.Join(" ", argsSplit));
            terminalConsoleBehavior.inputField.text = argsBaseCommand +" "+ string.Join(" ",argsSplit);
        }

        terminalConsoleBehavior.inputField.ActivateInputField();
        terminalConsoleBehavior.inputField.caretPosition = terminalConsoleBehavior.inputField.text.Length;
        NonNativeKeyboard.Instance.UpdateCaretPosition(terminalConsoleBehavior.inputField.text.Length);
    }
}
