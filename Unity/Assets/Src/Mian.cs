using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Src;
using Src.AOT.Framework.Fsm;
using Src.AOT.Framework.Procedure;
using Src.AOT.Framework.Resource;
using UnityEngine;

public class Mian : MonoBehaviour
{
    private void Awake()
    {
        GameEntry.CreatModule<FsmManager>();
        GameEntry.CreatModule<ProcedureManager>();
        GameEntry.CreatModule<ResourceManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEntry.GetModule<ProcedureManager>().Initialize(typeof(VersionCheckProcedure)
            ,typeof(DownloadProcedure)
            ,typeof(EnterGameProcedure));
        GameEntry.GetModule<ProcedureManager>().StartProcedure<VersionCheckProcedure>();
    }

    // Update is called once per frame
    void Update()
    {
        GameEntry.Update(Time.deltaTime, Time.deltaTime);
    }
}
