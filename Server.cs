using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Pratica{
  class Server{
    const int PORT_NO = 5000;
    const string SERVER_IP = "127.0.1.1";
    public void receive(){
      IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());  
      IPAddress ipAddress = ipHostInfo.AddressList[0];
      Console.WriteLine("Servidor ativo:");
      Console.WriteLine("\tIP: {0}\n\tPorta: {1}", ipAddress, PORT_NO);
      while(true){
        //Escuta a porta
        TcpListener listener = new TcpListener(ipAddress, PORT_NO);
        listener.Start();
        //Aceita conexão do cliente
        TcpClient client = listener.AcceptTcpClient();
        client.ReceiveBufferSize = 1024;
        Console.WriteLine("Conexão aceita: \n\tIP: {0}", ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
        //Recebe os dados do cliente via stream
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];
        //Lê o que foi recebido
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
        byte[] bytesReceive = buffer.Take(bytesRead).ToArray();
        //mac origem: 0-5 para hexa
        string macOrigem = BitConverter.ToString(bytesReceive.Take(6).ToArray()).Replace("-",":");
        //mac destino: 6-11 para hexa
        string macDestino = BitConverter.ToString(bytesReceive.Skip(6).Take(6).ToArray()).Replace("-",":");
        //Converte de 12-13 Bytes para int
        int payloadSize = BitConverter.ToInt16(bytesReceive.Skip(12).Take(2).ToArray());
        //Converte 14-sizeReceive para string
        string payload = ASCIIEncoding.ASCII.GetString((bytesReceive.Skip(14).Take(bytesRead-14).ToArray()));
        //Exibe PDU
        Console.WriteLine("\tMAC Origem: " + macOrigem);
        Console.WriteLine("\tMAC Destino: " + macDestino);
        Console.WriteLine("\tTamanho do Payload: {0}", payloadSize);
        Console.WriteLine("\tPayload: {0}", payload);
        //Encerra conexao
        client.Close();
        listener.Stop();
      }
    }
  }
}
