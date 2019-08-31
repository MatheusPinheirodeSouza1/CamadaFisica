using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Pratica
{
    class Client
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";
        string macOrigem = "64:1C:67:97:2B:D1";
        string macDestino = "64:1C:67:97:2B:D1";
        public void Enviar()
        {
            Random random = new Random(); 
            string texto = System.IO.File.ReadAllText("teste.txt");

            //Converte Head para byte.
            byte[] macOrigemByte = macOrigem.Split(':').Select(x => Convert.ToByte(x, 16)).ToArray();
            byte[] macDestinoByte = macDestino.Split(':').Select(x => Convert.ToByte(x, 16)).ToArray();
            byte[] tamanhoPayloadByte = BitConverter.GetBytes(texto.Length);
            //Concatena o Head.
            byte[] bytesToSend = Concat(Concat(macDestinoByte,macOrigemByte),tamanhoPayloadByte);

            //Tentar fazer a conexão
            TcpClient tcpClient = new TcpClient();
            while (true){
                try {
                    //Tenta estabelecer conexão.
                    tcpClient.Connect(SERVER_IP, PORT_NO);
                    NetworkStream nwStream = tcpClient.GetStream();

                    //Converte Payload para byte.
                    byte[] payloadByte = ASCIIEncoding.ASCII.GetBytes(texto);

                    //Concate o Head com o Payload
                    bytesToSend = Concat(bytesToSend,payloadByte);

                    Console.WriteLine("Sending : " + texto);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    //---read back the text---
                    byte[] bytesToRead = new byte[tcpClient.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, tcpClient.ReceiveBufferSize);
                    Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    Console.ReadLine();
                    tcpClient.Close();
                } catch(SocketException) {
                    var aleatorio = random.Next(0, 100);
                    Thread.Sleep(aleatorio);
                    Console.WriteLine("Erro, tempo de espera é de : " + aleatorio + "ms");
                }
            }
            
        }

        public byte[] Concat(byte[] a, byte[] b)
        {           
            byte[] output = new byte[a.Length + b.Length];
            for (int i = 0; i < a.Length; i++)
                output[i] = a[i];
            for (int j = 0; j < b.Length; j++)
                output[a.Length+j] = b[j];
            return output;           
        }
    }
}