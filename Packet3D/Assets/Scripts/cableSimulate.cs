using UnityEngine;

public class cableSimulate : MonoBehaviour
{
    public float packetIconCoeff =0f;
    public GameObject packetIcon;
    public cableRender cableRender;
    private Camera cam;
    private Transform posA;
    private Transform posB;
    private LineRenderer cableLine;
    public Material oldCableMat;
    public bool DEBUGAnimatePacket;
    public Material simulateCableMat;
    private Vector2 matOffset;
    public bool forwards = true;
    private float simDirection;
    private void Awake()
    {
        cam = Camera.main;
        posA = cableRender.posA;
        posB = cableRender.posB;
        cableLine = cableRender.GetComponent<LineRenderer>();
        oldCableMat = cableLine.material;
    }
    private void Start()
    {
        packetIcon.SetActive(false);
    }
    private void Update()
    {
        Vector3 midpoint;
        if (forwards)
        {
            midpoint = new Vector3(posA.position.x + 
            (posB.position.x - posA.position.x) *  packetIconCoeff, 
            posA.position.y + (posB.position.y - posA.position.y) * packetIconCoeff, 
            posA.position.z + (posB.position.z - posA.position.z) * packetIconCoeff);
        }
        else
        {
            midpoint = new Vector3(posA.position.x +
            (posB.position.x - posA.position.x) * (1f-packetIconCoeff),
            posA.position.y + (posB.position.y - posA.position.y) * (1f - packetIconCoeff),
            posA.position.z + (posB.position.z - posA.position.z) * (1f - packetIconCoeff));
        }

       

        packetIcon.transform.position = midpoint;
        packetIcon.transform.rotation = cam.transform.rotation;
        //
        if (DEBUGAnimatePacket)
        {
            DEBUGAnimatePacket = !DEBUGAnimatePacket;
            animatePacket();
        }


    }

    private void FixedUpdate()
    {
        simDirection = forwards ? 1 : -1;

        if (packetIconCoeff > 0) { 


            matOffset.x -= 0.035f*simDirection;

            packetIcon.SetActive(true);
            packetIconCoeff += 0.05f * SimulationBehavior.instance.SimSpeed;

            cableLine.startWidth = 0.007f;
            cableLine.material = simulateCableMat;

            cableLine.material.SetTextureOffset("_MainTex",matOffset);

            if (packetIconCoeff >= 1)
            {
                packetIconCoeff = 0;
                packetIcon.SetActive(false);
                cableLine.startWidth = 0.005f;
                cableLine.material = oldCableMat;
            }
        }
    }

    public void animatePacket()
    {
        packetIconCoeff = 0.01f;
    }
}
