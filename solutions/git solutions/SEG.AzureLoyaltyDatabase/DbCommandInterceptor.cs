//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Common;
//using System.Data.Entity.Infrastructure.Interception;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SEG.AzureLoyaltyDatabase
//{
//   public  class IsolationLevelInterceptor: DbCommandInterceptor
//    {
//        private IsolationLevel _isolationLevel;

//        public IsolationLevelInterceptor(IsolationLevel level)
//        {
//            _isolationLevel = level;
//        }

        



//        //[ThreadStatic]
//        //private DbCommand _command;

//        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
//        {
//            SetTransaction(command);
//            base.ReaderExecuting(command, interceptionContext);
           
//        }
     
       
//        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
//        {
//            SetTransaction(command);
//            base.ScalarExecuting(command, interceptionContext);
//        }

//        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
//        {
//            SetTransaction(command);
//            base.NonQueryExecuting(command, interceptionContext);
//        }




//        private void SetTransaction(DbCommand command)
//        {
//            if (command != null)
//            {
//                if (command.Transaction == null)
//                {
//                    var t = command.Connection.BeginTransaction(_isolationLevel);
//                    command.Transaction = t;
//                    //_command = command;
//                }
//            }
//        }
//    }
//}
