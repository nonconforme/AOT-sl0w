//Fixed With [DOGE]DEN aottg Sources fixer
//Doge Guardians FTW
//DEN is OP as fuck.
//Farewell Cowboy

using System;
using UnityEngine;

public class BTN_Server_List_PgDn : MonoBehaviour
{
    private void OnClick()
    {
        GameObject.Find("PanelMultiROOM").GetComponent<PanelMultiJoin>().pageDown();
    }
}

