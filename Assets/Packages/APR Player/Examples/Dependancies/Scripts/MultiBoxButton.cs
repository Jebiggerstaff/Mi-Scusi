using UnityEngine;


    //-------------------------------------------------------------
    //--APR Player
    //--MultiBoxButton
    //
    //--Unity Asset Store - Version 1.0
    //
    //--By The Famous Mouse
    //
    //--Twitter @FamousMouse_Dev
    //--Youtube TheFamouseMouse
    //-------------------------------------------------------------


public class MultiBoxButton : MonoBehaviour
{
    public MultiBoxDoorButton controller;
    
    private bool pressed = false;

    public MeshRenderer mesh;

    public Material nonPress;
    public Material press;

    private void Update()
    {
        if (pressed)
            mesh.material = press;
        else
            mesh.material = nonPress;
    }

    void OnTriggerStay(Collider col)
    {
        if(!pressed)
        {
            pressed = true;
            controller.pressedButtons += 1;
        }
    }
    
    void OnTriggerExit(Collider col)
    {
        if(pressed)
        {
            pressed = false;
            controller.pressedButtons -= 1;
        }
    }
}
