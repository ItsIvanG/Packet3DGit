using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public TextMeshProUGUI subtitleField;
    public LayoutElement layoutElement;
    public int characterWraplimit;

    public RectTransform rectTransform;
    public GameObject rightControl;
    Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetText( string header, string content, string sub)
    {
        contentField.text = content;
        headerField.text = header;
        subtitleField.text = sub;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        //width limit
        layoutElement.enabled = (headerLength > characterWraplimit || contentLength > characterWraplimit) ? true : false;
    }

    private void Update()
    {

        RaycastHit hit;
        bool RightRay = Physics.Raycast(rightControl.transform.position, rightControl.transform.forward, out hit, 100);

        if (RightRay)
        {
            transform.position = hit.point;


            transform.LookAt(new Vector3(cam.position.x, transform.position.y, cam.position.z));
            transform.forward *= -1;
        }

        //Vector2 position;
        //position.x = Input.mousePosition.x+15;
        //position.y = Input.mousePosition.y-5;

        //transform.position = position;

    }
}
