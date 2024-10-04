﻿using edutico.Data;
using MySql.Data.MySqlClient;

namespace edutico.Repositorio
{
    public class CartaoRepositorio : ICartaoRepositorio
    {
        public String CadastrarCartao(decimal NumCartao, string NomeTitular, int DataVali, int Bandeira, int CodLogin)
        {
            // Conexão com o banco de Dados
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();

            // Variável que armazena o comando SQL
            string sql = "Call spInsertTbCartaoCredito(@NumCartao, @NomeTitular,@DataVali, @Bandeira, @CodLogin);";

            // Variável que armazena o comando SQL e a conexão com o banco de dados 
            MySqlCommand cmd = new MySqlCommand(sql, conexao);

            // Parametros do comando sql 
            cmd.Parameters.AddWithValue("@NumCartao", NumCartao);
            cmd.Parameters.AddWithValue("@NomeTitular", NomeTitular);
            cmd.Parameters.AddWithValue("@DataVali", DataVali);
            cmd.Parameters.AddWithValue("@Bandeira", Bandeira);
            cmd.Parameters.AddWithValue("@CodLogin", CodLogin);

            // Lê os dados retornados pela procedure do BD
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            // Armazena os dados retornados do Banco de Dados
            MySqlDataReader dr;

            // Executando os comandos do mysql e passsando paa a variavel dr
            dr = cmd.ExecuteReader();

            // Para não dar erro na mensagem 
            string mensagem = null;

            if (dr.Read())
            {
                mensagem = dr.GetString(0); // Captura a primeira coluna (que é a mensagem retornada)
            }
            else
            {
                mensagem = "Erro Desconhecido";
            }
            // Fechar o DataReader antes de executar outro comando
            dr.Close();

            return mensagem;

            // deconecta do banco de dados
            con.DesconectarBD();
        }
    }
}