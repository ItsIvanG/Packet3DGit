using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireEnd : MonoBehaviour /////// deprecate??
{
    public enum WireEndPort { A, B };
    public WireEndPort whatEnd;
    public CableHops cableHopClass;

    public void updateHop(PortProperties pp)
    {
        if (whatEnd == WireEndPort.A)
        {
            if(pp)
            cableHopClass.updateHopA(pp);
            else cableHopClass.updateHopA(null);
        }
        else
        {
            if (pp)
                cableHopClass.updateHopB(pp);
            else cableHopClass.updateHopB(null);
        }
    }

}
