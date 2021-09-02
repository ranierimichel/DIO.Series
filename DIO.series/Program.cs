using System;

namespace DIO.series
{
  class Program
  {
    static SerieRepositorio repositorio = new SerieRepositorio();
    static void Main(string[] args)
    {
      Console.WriteLine("DIO Séries a seu dispor!!!");
      string opcaoUsuario = ObterOpcaoUsuario();
      SelecionarOpcaoUsuario(opcaoUsuario);
      Console.WriteLine("Obrigado por utilizar nossos serviços.");
      Console.ReadLine();
    }
    private static void InserirSerie()
    {
      Console.WriteLine("Inserir nova série");

      Serie novaSerie = NovaSerie(null);

      repositorio.Insere(novaSerie);
    }
    private static void ExcluirSerie()
    {
      if (repositorio.ProximoId() <= 0)
      {
        Warning("Nenhuma série cadastrada!");
        return;
      }

      Console.Write("Digite o id da série: ");
      int indiceSerie = int.Parse(Console.ReadLine());

      if (repositorio.ProximoId() <= indiceSerie)
      {
        Warning("Indice inválido!");
        return;
      }

      string confirmacao;
      do
      {
        Console.WriteLine("Confirma exclusão? S / N");
        confirmacao = Console.ReadLine().ToUpper();
      } while (!confirmacao.Equals("S") && !confirmacao.Equals("N"));

      if (confirmacao == "S")
      {
        Warning($"Indice {indiceSerie} excluído!");
        repositorio.Exclui(indiceSerie);
      }
      else
        Warning($"Exclusão cancelada!");
    }
    private static void VisualizarSerie()
    {
      if (repositorio.ProximoId() <= 0)
      {
        Warning("Nenhuma série cadastrada!");
        return;
      }

      Console.Write("Digite o id da série: ");
      int indiceSerie = int.Parse(Console.ReadLine());

      if (repositorio.ProximoId() <= indiceSerie)
      {
        Warning("Indice inexistente!");
        return;
      }

      var serie = repositorio.RetornaPorId(indiceSerie);
      Console.WriteLine();
      Console.WriteLine(serie);
    }
    private static void AtualizarSerie()
    {
      if (repositorio.ProximoId() < 1)
      {
        Console.WriteLine("Não há série para atualizar!");
        Console.ReadLine();
        return;
      }

      Console.WriteLine("Atualizar série");
      Console.Write("Digite o id da série: ");
      int indiceSerie = int.Parse(Console.ReadLine());

      if (repositorio.ProximoId() <= indiceSerie)
      {
        Warning("Indice inexistente!");
        return;
      }

      if (repositorio.RetornaPorId(indiceSerie).retornaExcluido())
      {
        Warning("Indice excluído não pode ser atualizado!");
        return;
      }

      Serie atualizaSerie = NovaSerie(indiceSerie);

      repositorio.Atualiza(indiceSerie, atualizaSerie);
    }
    private static void ListarSeries()
    {
      Console.WriteLine("Listar séries");
      var lista = repositorio.Lista();

      if (lista.Count == 0)
      {
        Warning("Nenhuma série cadastrada!");
        return;
      }

      foreach (var serie in lista)
      {
        Console.WriteLine($"#ID {serie.retornaId()}: - {serie.retornaTitulo()} {(serie.retornaExcluido() ? "- Excluído" : "")}");
      }
    }
    // Informar id da serie caso atualizando, informar null caso nova serie!
    private static Serie NovaSerie(int? id)
    {
      int idSerie;

      if (id == null)
        idSerie = repositorio.ProximoId();
      else
        idSerie = (int)id;

      bool valido;
      int quantidadeGeneros;
      int indice;
      do
      {
        quantidadeGeneros = 0;
        foreach (int i in Enum.GetValues(typeof(Genero)))
        {
          Console.WriteLine($"{i}-{Enum.GetName(typeof(Genero), i)}");
          quantidadeGeneros++;
        }
        Console.Write("Digite o indice do gênero entre as opções acima: ");
        string selecao = Console.ReadLine();

        valido = int.TryParse(selecao, out indice);

      } while (!valido || indice > quantidadeGeneros);


      Console.Write("Digite o Título da Série: ");
      string entradaTitulo = Console.ReadLine();

      int ano;
      do
      {
        Console.Write("Digite o Ano de Início da Série: ");
        string entradaAno = Console.ReadLine();

        valido = int.TryParse(entradaAno, out ano);
      } while (!valido || ano > 9999 || ano < 999);


      Console.Write("Digite a Descrição da Série: ");
      string entradaDescricao = Console.ReadLine();

      Serie novaSerie = new Serie(id: idSerie,
                                  genero: (Genero)indice,
                                  titulo: entradaTitulo,
                                  ano: ano,
                                  descricao: entradaDescricao);

      return novaSerie;
    }

    private static string ObterOpcaoUsuario()
    {
      Console.WriteLine();
      Console.WriteLine("Informe a opção desejada:");

      Console.WriteLine("1- Listar séries");
      Console.WriteLine("2- Inserir nova série");
      Console.WriteLine("3- Atualizar série");
      Console.WriteLine("4- Excluir série");
      Console.WriteLine("5- Visualizar série");
      Console.WriteLine("C- Limpar tela");
      Console.WriteLine("X- Sair");
      Console.WriteLine();

      string opcaoUsuario = Console.ReadLine().ToUpper();
      Console.WriteLine();
      return opcaoUsuario;
    }

    private static void SelecionarOpcaoUsuario(string opcaoUsuario)
    {
      while (opcaoUsuario.ToUpper() != "X")
      {
        switch (opcaoUsuario)
        {
          case "1":
            ListarSeries();
            break;
          case "2":
            InserirSerie();
            break;
          case "3":
            AtualizarSerie();
            break;
          case "4":
            ExcluirSerie();
            break;
          case "5":
            VisualizarSerie();
            break;
          case "C":
            Console.Clear();
            break;
          default:
            Console.WriteLine("Indice inválido!");
            break;
        }
        opcaoUsuario = ObterOpcaoUsuario();
      }
    }

    private static void Warning(string text)
    {
      Console.WriteLine(text);
      Console.ReadLine();
    }

  }
}
