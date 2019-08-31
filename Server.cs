using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Pratica
{
    class Server
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";

        public void Receber()
        {
            //Escuta a porta
            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localAdd, PORT_NO);
            Console.WriteLine("Listening...");
            listener.Start();

            //Aceita conexão do cliente
            TcpClient client = listener.AcceptTcpClient();

            //---get the incoming data through a network stream---
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];

            //Lê o que foi recebido
            int bufferSize = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
            
            //Converte os 12 primeiros Bytes para hexa
            string head = BitConverter.ToString(buffer.Take(12).ToArray()).Replace("-",":");

            //Converte de 12-14 Bytes para int
            string head = BitConverter.ToString(buffer.Take(12).ToArray()).Replace("-",":");

            //converter para texto

            Console.WriteLine("Reading : " + head);

            //---write back the text to the client---
            Console.WriteLine("Sending back : " + head);
            nwStream.Write(buffer, 0, headByte);
            client.Close();
            listener.Stop();
            Console.ReadLine();
        }
    }
}
