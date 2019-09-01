using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Pratica{
  class Client{
    const int PORT_NO = 5000;
    const int COLISION_PERCENTAGE = 10;
    const string SERVER_IP = "127.0.1.1";
    const string FILE_PATH = "teste.txt";
    string macOrigem = "41:7f:33:0e:65:b2";
    string macDestino = "41:7f:83:e8:5e:ff";
    public void send(){
      Random random = new Random(); 
      //Tentar fazer a conexão
      TcpClient tcpClient = new TcpClient();
      tcpClient.SendBufferSize = 1024;
      bool outLoop = false;
      while (!outLoop){
        try {
          //verifica colisao
          var colision = random.Next(0, 100);
          if(COLISION_PERCENTAGE <= colision){
            string content = System.IO.File.ReadAllText(FILE_PATH);
            //Converte Head para byte.
            byte[] macOrigemByte = macOrigem.Split(':').Select(x => Convert.ToByte(x, 16)).ToArray();
            byte[] macDestinoByte = macDestino.Split(':').Select(x => Convert.ToByte(x, 16)).ToArray();
            byte[] payloadSizeByte = BitConverter.GetBytes(Convert.ToInt16(content.Length));
            //Concatena o Head.
            byte[] bytesToSend = Concat(Concat(macOrigemByte,macDestinoByte),payloadSizeByte);
            //Tenta estabelecer conexão.
            tcpClient.Connect(SERVER_IP, PORT_NO);
            NetworkStream nwStream = tcpClient.GetStream();
            Console.WriteLine("\n\nConexão estabelecida:");
            //Converte Payload para byte.
            byte[] payloadByte = ASCIIEncoding.ASCII.GetBytes(content);
            //Concate o Head com o Payload
            bytesToSend = Concat(bytesToSend,payloadByte);
            var pduBits = string.Concat(bytesToSend.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
            //Exibe PDU
            Console.WriteLine("\tMAC Origem: {0}", macOrigem);
            Console.WriteLine("\tMAC Destino: {0}", macDestino);
            Console.WriteLine("\tTamanho do payload: {0}", content.Length);
            Console.WriteLine("\tPayload: " + content);
            Console.WriteLine("\tPDU completa em bits: {0}", pduBits);
            //Faz o envio dos bits
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            //Encerra a conexao
            tcpClient.Close();
            Console.WriteLine("\nConexão encerrada.");
            outLoop = true;
          } else {
            var sleepTime = random.Next(0, 100);
            Thread.Sleep(sleepTime);
            Console.WriteLine("Colisão detectada! Será enviado novamente em {0} ms.", sleepTime);
          }
        } catch(SocketException) {
          var sleepTime = random.Next(0, 100);
          Thread.Sleep(sleepTime);
          Console.WriteLine("Erro, tempo de espera é de : " + sleepTime + "ms");
        }
      } 
    }
    public byte[] Concat(byte[] a, byte[] b){           
      byte[] output = new byte[a.Length + b.Length];
      for (int i = 0; i < a.Length; i++)
        output[i] = a[i];
      for (int j = 0; j < b.Length; j++)
        output[a.Length+j] = b[j];
      return output;           
    }
  }
}