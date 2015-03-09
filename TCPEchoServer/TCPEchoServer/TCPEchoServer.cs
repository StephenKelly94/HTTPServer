/*
 * TCPEchoServer
 *
 * Author Michael Claudius, ZIBAT Computer Science
 * Version 1.0. 2014.02.10
 * Copyright 2014 by Michael Claudius
 * Revised 2014.09.01
 * All rights reserved
 */


namespace TCPEchoServer
{
    class TcpEchoServer
    {

        public static void Main(string[] args)
        {
            ServiceStart serviceStart = new ServiceStart();
            serviceStart.Run();
        }
    }
    
}
