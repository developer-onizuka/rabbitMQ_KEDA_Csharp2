using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace rabbitmq_to_faceDetectedMQ
{
    public class rabbitmq_to_faceDetecedMQ
    {
        [FunctionName("rabbitmq_to_faceDetectedMQ")]
        public void Run(
          [RabbitMQTrigger("employee-queue-tmp", ConnectionStringSetting = "RabbitMQConnection")] EmployeeEntity emp,
          ILogger log)
        {
	    byte[] bs = emp.Image;
	    string input = "/home/site/wwwroot/image" + emp.EmployeeID + ".jpg";
	    string output = "/home/site/wwwroot/face" + emp.EmployeeID + ".jpg";
	    using (var fs = new FileStream(input, FileMode.Create))
	    {
	        fs.Write(bs, 0, bs.Length);
	    }

	    pythonScript py = new pythonScript(); 
	    py.myPythonApp = "/home/site/wwwroot/faceDetect.py"; 
	    py.inputFile = input;
	    py.outputFile = output;
	    py.faceDetectedMQ(emp);

	    File.Delete(@input);
	    File.Delete(@output);
	}
    }
}
