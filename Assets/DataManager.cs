using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    
    public static bool hasStart = false;
    public static bool hasEnd = false;
    public static bool instantFunctions = false;

    public void ToggleInstantFunctions()
    {
        if (instantFunctions == false)
            instantFunctions = true;
        else
            instantFunctions = false;
    }

}
