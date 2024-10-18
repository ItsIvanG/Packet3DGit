using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New CMD_Ping Command", menuName = "Terminal/CMD_Ping Command")]
public class CMD_Ping : ConsoleCommand
{
    public List<GameObject> hops;
    public override bool Process(string[] args)
    {
        TerminalConsoleBehavior.printToTerminal("Pinging " + args[0] + " with 32 bytes of data:");
        //TODO: ACTUAL PING!
        bool found = false;

        hops.Clear();
        hops.Add(DesktopCanvasScript.instance.currentPC);
        SimulationBehavior.instance.hopsFound.Clear();
        if (args[0] != "localhost")
        {
            SimulationBehavior.instance.recursiveTest(DesktopCanvasScript.instance.currentPC, this);

            foreach (var hop in hops)
            {
                var ports = hop.GetComponentsInChildren<PortProperties>();
                foreach (var port in ports)
                {
                    if (port.address == args[0])
                    {

                        //TerminalConsoleBehavior.printToTerminal("Reply from " + args[0] + ": bytes=32 time<1ms TTL=128");

                        TerminalConsoleBehavior.instance.StartCoroutine(TerminalConsoleBehavior.instance.RepeatPrint(
                            "Reply from " + args[0] + ": bytes=32 time<1ms TTL=128",
                            0.5f,
                            4
                            ));

                        DesktopCanvasScript.instance.currentPC.GetComponent<PCBehavior>().successPing = args[0];
                        found = true;
                    }
                }
            }
            if (!found)
            {
                TerminalConsoleBehavior.printToTerminal("Request timed out.");
            }
        }
        else
        {
            TerminalConsoleBehavior.instance.StartCoroutine(TerminalConsoleBehavior.instance.RepeatPrint(
                          "Reply from " + DesktopCanvasScript.instance.currentPC.GetComponentInChildren<PortProperties>().address + ": bytes=32 time<1ms TTL=128",
                          0.5f,
                          4
                          ));
        }

        return true;
    }

}
