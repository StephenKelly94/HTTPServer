/*
 * TCPEchoClient
 *
 * Author Michael Claudius, ZIBAT Computer Scienc
 * Version 1.0. 2014.02.10
 * Copyright 2014 by Michael Claudius
 * Revised 2014.09.01
 * All rights reserved
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TCPEchoClient
{
    class TCPEchoClient
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to start connecting to server");
            Console.ReadLine();
            //TcpClient clientSocket = new TcpClient("172.20.10.2", 65080);
            TcpClient clientSocket = new TcpClient("localhost", 65080);

            Stream ns = clientSocket.GetStream();  //provides a Stream
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing

            for (int i = 0; i < 5; i++)
            {
                //Console.WriteLine(i + ". Write a message to server:");
                //string message = Console.ReadLine();
                Random random = new Random(DateTime.Now.Millisecond);
                string message = "" + i + ".";
                for (int j = 0; j < 10; j++)
                {
                    int newChar = random.Next(97, 122);
                    message = message + (char)(newChar);
                }
                Console.WriteLine("I am sending: " + message);
                sw.WriteLine(message);
                string serverAnswer = sr.ReadLine();
                Console.WriteLine("Server replied: " + serverAnswer);
                Thread.Sleep(1000);
            }

            ns.Close();
            clientSocket.Close();
            Console.WriteLine("Client finished work.");
            Console.ReadLine();
        }

    }
}
