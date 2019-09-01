using System;

namespace Pratica{
  class Program{
    static void Main(string[] args){
      bool outLoop = false;
      while(!outLoop){
        Console.WriteLine("Escolha uma opção: \n\n1) Servidor \n2) Cliente");
        Console.Write("Opção: ");
        int option;
        if(int.TryParse(Console.ReadLine(), out option)){
            switch(option){
            case 1:
              var server = new Server();
              server.receive();
              outLoop = true;
              break;
            case 2:
              var client = new Client();
              client.send();
              outLoop = true;
              break;
            default:
              Console.WriteLine("Opção inválida! Gentileza escolher novamente.\n");
              break;
          }
        } else {
          Console.WriteLine("Opção inválida! Gentileza escolher novamente.\n");
        }
      }
    }
  }
}
