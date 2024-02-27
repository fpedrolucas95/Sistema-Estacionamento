using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SistemaEstacionamento
{
    class Program
    {
        static void Main(string[] args)
        {
            var estacionamento = new Estacionamento();
            while (true)
            {
                try
                {
                    Console.WriteLine("Escolha uma opção:");
                    Console.WriteLine("1 - Adicionar veículo");
                    Console.WriteLine("2 - Remover veículo");
                    Console.WriteLine("3 - Listar veículos");
                    Console.WriteLine("4 - Sair");
                    var opcao = Console.ReadLine();

                    switch (opcao)
                    {
                        case "1":
                            Console.WriteLine("Informe a placa do veículo para adicionar:");
                            var placa = Console.ReadLine()?.ToUpper().Trim();
                            if (ValidarPlaca(placa))
                            {
                                estacionamento.AdicionarVeiculo(placa);
                            }
                            else
                            {
                                Console.WriteLine("Placa inválida. Certifique-se de que a placa esteja no formato correto.");
                            }
                            break;
                        case "2":
                            Console.WriteLine("Informe a placa do veículo para remover:");
                            placa = Console.ReadLine()?.ToUpper().Trim();
                            if (ValidarPlaca(placa))
                            {
                                estacionamento.RemoverVeiculo(placa);
                            }
                            else
                            {
                                Console.WriteLine("Placa inválida. Certifique-se de que a placa esteja no formato correto.");
                            }
                            break;
                        case "3":
                            estacionamento.ListarVeiculos();
                            break;
                        case "4":
                            Console.WriteLine("Encerrando o sistema de estacionamento.");
                            return;
                        default:
                            Console.WriteLine("Opção inválida.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
        }

        static bool ValidarPlaca(string placa)
        {
            return Regex.IsMatch(placa, @"^[A-Z]{3}-?\d{4}$") || Regex.IsMatch(placa, @"^[A-Z]{3}\d[A-Z]\d{2}$");
        }
    }

    class Estacionamento
    {
        private Dictionary<string, DateTime> veiculos = new Dictionary<string, DateTime>();
        private const decimal valorHora = 5.00M;

        public void AdicionarVeiculo(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
            {
                throw new ArgumentException("Placa do veículo não pode ser vazia.");
            }

            if (!veiculos.ContainsKey(placa))
            {
                veiculos.Add(placa, DateTime.Now);
                Console.WriteLine($"Veículo {placa} adicionado em {DateTime.Now}.");
            }
            else
            {
                Console.WriteLine("Veículo já está no estacionamento.");
            }
        }

        public void RemoverVeiculo(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
            {
                throw new ArgumentException("Placa do veículo não pode ser vazia.");
            }

            if (veiculos.ContainsKey(placa))
            {
                DateTime entrada = veiculos[placa];
                DateTime saida = DateTime.Now;
                var totalHoras = Math.Ceiling((saida - entrada).TotalHours);
                var valorCobrado = (decimal)totalHoras * valorHora;
                veiculos.Remove(placa);

                Console.WriteLine($"Veículo {placa} removido. Entrada: {entrada}, Saída: {saida}, Total a pagar: R${valorCobrado:N2}");
            }
            else
            {
                Console.WriteLine("Veículo não encontrado.");
            }
        }

        public void ListarVeiculos()
        {
            if (veiculos.Count == 0)
            {
                Console.WriteLine("Não há veículos estacionados.");
            }
            else
            {
                Console.WriteLine("Veículos estacionados:");
                foreach (var veiculo in veiculos.Keys)
                {
                    Console.WriteLine($"{veiculo} - Estacionado desde {veiculos[veiculo]}");
                }
            }
        }
    }
}
