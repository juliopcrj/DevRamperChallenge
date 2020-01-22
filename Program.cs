using System;
using System.IO;

namespace DevRamperChallenge
{
	class Program
	{

		/*
		* Conta a quantidade de linhas de código
		* em um arquivo, removendo as linhas de 
		* comentários.
		*/
		static int CountSLOC(StreamReader f){
			int crude_count = 0;
			string line;
			bool is_line_of_code = false;
			bool is_multiple_comment = false;

			while((line = f.ReadLine()) != null){
				Console.WriteLine(line.Length);
				// se a linha for vazia, pula
				if(line.Length == 0)
					is_line_of_code = false;

				// se a linha começar com comentário
				// de uma linha, pula
				else if(line.Trim().StartsWith("//")){
					is_line_of_code = false;
				}
				/*
				 * Caso a linha comece com comentário
				 * de mais de linha, analisa se tem  
				 * código fora de comentário nessas  
				 * linhas
				 */
				else if(line.Trim().StartsWith("/*") || is_multiple_comment ){
					if(!line.Contains("*/"))
						is_multiple_comment = true;
					else
						is_multiple_comment = false;

					while(!is_multiple_comment){
						int index = line.IndexOf("*/");
						try{
							line = line.Substring(index + 2);

							if(line.Trim().Length == 0){
								break;
							}

							if(line.Trim().StartsWith("//")){
								break;
							}

							if(line.Trim().StartsWith("/*")){
								if(!line.Contains("*/")){
									is_multiple_comment = true;
								}
							}else{
								is_line_of_code = true;
							}
						}catch{
							//fim da string
						}
					}
				}
				// Por fim, se a linha começa comum caractere
				// não nulo e não comentário, é uma linha de
				// código.
				else
					is_line_of_code = true;

				if(!is_line_of_code)
					continue;
				crude_count += 1;
				is_line_of_code = false;
			}
			return crude_count;
		}

		/*
		 * Uso do programa:
		 * dotnet run nome_de_algum_arquivo
		 */
		static int Main(string[] args)
		{
			if(args.Length == 0)
			{
				Console.WriteLine("Forneça um nome de arquivo como argumento");
				return 1;
			}
			try
			{
				int lines = CountSLOC(new StreamReader(args[0]));
				Console.WriteLine("Foram lidas " + lines + " linhas de código");
			}
			catch(IOException ioe)
			{
				Console.WriteLine("Arquivo não pode ser aberto");
				Console.WriteLine(ioe.Message);
			}
			return 0;
		}
	}
}
