using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BreakerController : MonoBehaviour
{
    public BreakerPanelHelper breakerPanelHelper;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseEnter()
    {

    }

    public void OnMouseDown()
    {
        //if (!breakerPanelHelper.uiController.isUIClicked())
        //{
            breakerPanelHelper.gameObject.SetActive(true);
        //}
    }
}
