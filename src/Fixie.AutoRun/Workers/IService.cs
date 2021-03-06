﻿using System.ServiceModel;

namespace Fixie.AutoRun.Workers
{
   [ServiceContract]
   public interface IService
   {
      [OperationContract]
      void TestCompleted(TestResult testResult);

      [OperationContract]
      void Error(string type, string message, string stackTrace);
   }
}