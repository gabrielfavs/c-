using System;

namespace SISTEMADEALUNO
{
    public class Aluno
    {
        //PROPRIEDADE DA CLASSE
        public string Nome { get; set; } //Significa que essas variáveis podem ser lidas (get) e modificadas (set) fora da classe (ou seja, no Program)
        public double Nota1 { get; set; }
        public double Nota2 { get; set; }

        public Aluno(string nome, double nota1, double nota2) //Construtor da classe
        {
            Nome = nome;
            Nota1 = nota1;
            Nota2 = nota2;
        }

        public double CalcularMedia() //Método para calcular a média
        {
            return (Nota1 + Nota2) / 2;
        }

        public void ExibirResultado() //Método para exibir resultado
        {
            double media = CalcularMedia();
            Console.WriteLine($"\nAluno: {Nome}");
            Console.WriteLine($"Média: {media}");

            if (media >= 6.0)
            {
                Console.WriteLine("Status: Aprovado");
            }
            else
            {
                Console.WriteLine("Status: Reprovado");
            }
        }
    }
}