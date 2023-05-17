using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;

class Program
{
    static string filePath = "clientes.csv";

    static void Main(string[] args)
    {
        CreateFileIfNotExists(filePath);

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Menu de Execução");
            Console.WriteLine("1 - Cadastrar");
            Console.WriteLine("2 - Editar");
            Console.WriteLine("3 - Excluir");
            Console.WriteLine("4 - Listar todos");
            Console.WriteLine("5 - Sair");

            Console.Write("Digite a opção desejada: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    CadastrarCliente();
                    break;
                case "2":
                    EditarCliente();
                    break;
                case "3":
                    ExcluirCliente();
                    break;
                case "4":
                    ListarTodosClientes();
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void CadastrarCliente()
    {
        Console.WriteLine("Cadastro de Cliente");

        int lastId = ObterUltimoId();

        int id = lastId + 1;
        Console.WriteLine("ID do Cliente: " + id);

        Console.Write("Nome: ");
        string nome = Console.ReadLine();

        Console.Write("Endereço: ");
        string endereco = Console.ReadLine();

        Console.Write("Telefone: ");
        string telefone = Console.ReadLine();

        string email = ObterEmailValido();

        Cliente cliente = new Cliente(id, nome, endereco, telefone, email);

        string csvLine = $"{id},{nome},{endereco},{telefone},{email}";

        AdicionarClienteAoArquivo(csvLine);

        Console.WriteLine("Cliente cadastrado com sucesso!");
    }

    static void EditarCliente()
    {
        Console.WriteLine("Edição de Cliente");

        Console.Write("Digite o ID do cliente a ser editado: ");
        int id = LerInteiroPositivo();

        List<string> lines = LerLinhasArquivo();

        bool clienteEncontrado = false;

        for (int i = 0; i < lines.Count; i++)
        {
            string[] fields = lines[i].Split(',');

            if (Convert.ToInt32(fields[0]) == id)
            {
                clienteEncontrado = true;

                Console.Write("Nome: ");
                string nome = Console.ReadLine();

                Console.Write("Endereço: ");
                string endereco = Console.ReadLine();

                Console.Write("Telefone: ");
                string telefone = Console.ReadLine();

                string email = ObterEmailValido();

                Cliente cliente = new Cliente(id, nome, endereco, telefone, email);

                string csvLine = $"{id},{nome},{endereco},{telefone},{email}";

                lines[i] = csvLine;

                EscreverLinhasNoArquivo(lines);

                Console.WriteLine("Cliente atualizado com sucesso!");
                break;
            }
        }

        if (!clienteEncontrado)
        {
            Console.WriteLine("Cliente não encontrado.");
        }
    }

    static void ExcluirCliente()
    {
        Console.WriteLine("Exclusão de Cliente");

        Console.Write("Digite o ID do cliente a ser excluído: ");
        int id = LerInteiroPositivo();

        List<string> lines = LerLinhasArquivo();

        bool clienteEncontrado = false;

        for (int i = 0; i < lines.Count; i++)
        {
            string[] fields = lines[i].Split(',');

            if (Convert.ToInt32(fields[0]) == id)
            {
                clienteEncontrado = true;
                lines.RemoveAt(i);
                EscreverLinhasNoArquivo(lines);
                Console.WriteLine("Cliente excluído com sucesso!");
                break;
            }
        }

        if (!clienteEncontrado)
        {
            Console.WriteLine("Cliente não encontrado.");
        }
    }

    static void ListarTodosClientes()
    {
        Console.WriteLine("Lista de Clientes");

        List<string> lines = LerLinhasArquivo();

        foreach (string line in lines)
        {
            string[] fields = line.Split(',');
            Console.WriteLine($"ID: {fields[0]}, Nome: {fields[1]}, Endereço: {fields[2]}, Telefone: {fields[3]}, E-mail: {fields[4]}");
        }
    }

    static void CreateFileIfNotExists(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }

    static int ObterUltimoId()
    {
        List<string> lines = LerLinhasArquivo();

        if (lines.Count > 0)
        {
            string lastLine = lines[lines.Count - 1];
            string[] fields = lastLine.Split(',');
            return Convert.ToInt32(fields[0]);
        }

        return 0;
    }

    static List<string> LerLinhasArquivo()
    {
        List<string> lines = new List<string>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }

        return lines;
    }

    static void AdicionarClienteAoArquivo(string csvLine)
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(csvLine);
        }
    }

    static void EscreverLinhasNoArquivo(List<string> lines)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }
        }
    }

    static string ObterEmailValido()
    {
        while (true)
        {
            Console.Write("E-mail: ");
            string email = Console.ReadLine();

            if (ValidarEmail(email))
            {
                return email;
            }

            Console.WriteLine("E-mail inválido. Por favor, informe um e-mail válido.");
        }
    }

    static bool ValidarEmail(string email)
    {
        try
        {
            MailAddress mailAddress = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    static int LerInteiroPositivo()
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int number) && number > 0)
            {
                return number;
            }

            Console.WriteLine("Valor inválido. Digite um número inteiro positivo.");
        }
    }
}

class Cliente
{
    public int ID { get; set; }
    public string Nome { get; set; }
    public string Endereco { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }

    public Cliente(int id, string nome, string endereco, string telefone, string email)
    {
        ID = id;
        Nome = nome;
        Endereco = endereco;
        Telefone = telefone;
        Email = email;
    }
}
