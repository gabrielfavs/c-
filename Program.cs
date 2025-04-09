using System;

namespace SISTEMADEALUNO
{
    class Program
    {
        static void Main(string[] args)
        {
            // Solicita o nome do usuário
            Console.WriteLine("Digite o seu nome:");
            
            // Se o usuário não digitar nada, a variável nome será uma string vazia
            string nome = Console.ReadLine() ?? "";  // Usando operador de coalescência nula

            // Exibe uma mensagem de boas-vindas
            Console.WriteLine($"Seja muito bem-vindo!! {nome}");

            // Definir os anos de nascimento e atual
           
        }
    }
}
