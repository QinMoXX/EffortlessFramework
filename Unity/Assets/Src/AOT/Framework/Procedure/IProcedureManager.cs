using System;

namespace AOT.Framework.Procedure
{
    public interface IProcedureManager
    {
        ProcedureBase CurrentProcedure { get; }
        
        void StartProcedure<T>() where T : ProcedureBase;
        
        void StartProcedure(Type procedureType);
        
        ProcedureBase GetProcedure<T>() where T : ProcedureBase;
        
        ProcedureBase GetProcedure(Type procedureType);
        
        bool HasProcedure<T>() where T : ProcedureBase;
        
        bool HasProcedure(Type procedureType);
    }
}