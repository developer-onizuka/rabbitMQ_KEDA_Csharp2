using System;
using System.Diagnostics;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;

public class pythonScript
{
    public string myPythonApp; //= "/home/site/wwwroot/faceDetect.py";
    public string inputFile;   //= "/home/site/wwwroot/image1.jpg";
    public string outputFile;  //= "/home/site/wwwroot/face1.jpg";

    public void faceDetectedMQ(EmployeeEntity entity)
    {
        var myProcess = new Process
        {
            StartInfo = new ProcessStartInfo("python3")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = myPythonApp + " " + this.inputFile + " " + this.outputFile
            }
        };

        myProcess.Start();
        StreamReader myStreamReader = myProcess.StandardOutput;
        myProcess.WaitForExit();
        myProcess.Close();

        using (FileStream fs = File.OpenRead(this.outputFile))
        {
            byte[] bs = new byte[fs.Length];
	    fs.Read(bs, 0, bs.Length);

	    entity.Face = bs;

	    RabbitmqDLX dl = new RabbitmqDLX();
	    dl.rabbitmq_ipaddr = System.Environment.GetEnvironmentVariable("RabbitMQ_IPaddress");
	    dl.rabbitmq_dlx = System.Environment.GetEnvironmentVariable("RabbitMQ_DLX");
	    dl.SendMessages(entity);
        }
    }
}
