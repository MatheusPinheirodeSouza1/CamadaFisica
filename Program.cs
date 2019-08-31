using System;

namespace Pratica
{
    class Program
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";

        static void Main(string[] args)
        {
            while(true){
                Console.WriteLine("1- Para receber \n2 - para enviar");
                var opcao = int.Parse(Console.ReadLine());
                if (opcao == 1){
                    var server = new Server();
                    server.Receber();
                    break;
                } else if (opcao == 2){
                    var client = new Client();
                    client.Enviar();
                    break;
                }
            }
        }
    }
}
