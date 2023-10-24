using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public TextMeshProUGUI subtitleField;
    public LayoutElement layoutElement;
    public int characterWraplimit;

    public RectTransform rectTransform;

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
        Vector2 position;
        position.x = Input.mousePosition.x+15;
        position.y = Input.mousePosition.y-5;

        transform.position = position;

    }
}
