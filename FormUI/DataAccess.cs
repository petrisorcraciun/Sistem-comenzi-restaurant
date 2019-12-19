using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace FormUI
{
    public class DataAccess
    {

        public List<Mese> getListaMese()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.Conectare()))
            {
                var output = connection.Query<Mese>($"select * from Mese ").ToList();
                return output;
            }
        }

        public List<Gestiuni> getListaGestiuni()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.Conectare()))
            {
                var output = connection.Query<Gestiuni>($"select * from Gestiuni ").ToList();
                return output;
            }
        }

        public List<Produse> listaProduseGestiune(int id)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.Conectare()))
            {
                var output = connection.Query<Produse>($"select * from Produse where id_gestiune = '{id}' ").ToList();
                return output;
            }
        }

        public List<Produse> detaliiProdus(int id)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.Conectare()))
            {
                var output = connection.Query<Produse>($"select * from Produse where id = '{id}' ").ToList();
                return output;
            }
        }


        public List<ProduseComanda> listaProduseComanda(int id)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.Conectare()))
            {
                var output = connection.Query<ProduseComanda>($"select * from ProduseComanda " +
                    $"INNER JOIN Comenzi ON ProduseComanda.idComanda = Comenzi.id " +
                    $"INNER JOIN Produse ON ProduseComanda.idProdus = Produse.id " +
                    $"where Comenzi.idMasa = '{id}' ").ToList();
                return output;
            }
        }


        public void adaugaProdusComanda(int idProdus,int idComanda,int cantitate, decimal pret)
        {
            using (SqlConnection connection = new SqlConnection(Helper.Conectare()))
            {
                String query = "INSERT INTO dbo.ProduseComanda (idComanda,idProdus,cantitate,pret,valoare) VALUES (@idComanda,@idProdus, @cantitate,@pret, @valoare)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@idComanda", idComanda);
                    command.Parameters.AddWithValue("@idProdus", idProdus);
                    command.Parameters.AddWithValue("@cantitate", cantitate);
                    command.Parameters.AddWithValue("@pret", pret);
                    command.Parameters.AddWithValue("@valoare", cantitate * pret);

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                }
            }
        }
        public void stergeProdus(int idProdus)
        {
            using (SqlConnection connection = new SqlConnection(Helper.Conectare()))
            {
                String query = "DELETE FROM dbo.ProduseComanda WHERE idProdus =  @idProdus";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@idProdus", idProdus);

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                }
            }
        }

        public bool stareMasa(int idMasa)
        {
            using (SqlConnection connection = new SqlConnection(Helper.Conectare()))
            {
                String query = "Select Stare From dbo.Mese WHERE NrMasa =  @NrMasa";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@NrMasa", idMasa);

                    connection.Open();
                    bool result = (bool) command.ExecuteScalar();
                

                    return result;
                }
            }
        }

        public void adaugaComandaNoua(int idMasa)
        {
            using (SqlConnection connection = new SqlConnection(Helper.Conectare()))
            {
                String query = "INSERT INTO dbo.Comenzi (idMasa,Data,ora,stare) VALUES (@idMasa,@Data, @ora,@stare)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@idMasa", idMasa);
                    command.Parameters.AddWithValue("@Data", DateTime.Now);
                    command.Parameters.AddWithValue("@ora", DateTime.Now);
                    command.Parameters.AddWithValue("@stare", 0);
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                }
            }
        }

        public void schimbaStareMasa(int idMasa,int stare)
        {
            using (SqlConnection connection = new SqlConnection(Helper.Conectare()))
            {
                String query = "UPDATE dbo.Mese SET Stare = @Stare WHERE NrMasa = @idMasa";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Stare", stare);
                    command.Parameters.AddWithValue("@idMasa", idMasa);
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                }
            }
        }

        public int cautaIdComanda(int idMasa)
        {
            using (SqlConnection connection = new SqlConnection(Helper.Conectare()))
            {
                String query = "SELECT TOP 1 id From dbo.Comenzi WHERE idMasa =  @NrMasa ORDER BY ID DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@NrMasa", idMasa);

                    connection.Open();
                    int result = (Int32)command.ExecuteScalar();


                    return result;
                }
            }
        }

        public void stergeProduseComanda(int idComanda)
        {
            using (SqlConnection connection = new SqlConnection(Helper.Conectare()))
            {
                String query = "DELETE FROM dbo.ProduseComanda WHERE idComanda =  @idComanda";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@idComanda", idComanda);
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                }
            }
        }

        public void schimbaStareComanda(int idMasa, int stare)
        {
            using (SqlConnection connection = new SqlConnection(Helper.Conectare()))
            {
                String query = "UPDATE dbo.Comenzi SET stare = @stare WHERE idMasa = @idMasa";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@stare", stare);
                    command.Parameters.AddWithValue("@idMasa", idMasa);
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                }
            }
        }

        public int cautaIdProdus(String numeProdus)
        {
            using (SqlConnection connection = new SqlConnection(Helper.Conectare()))
            {
                String query = "SELECT id From dbo.Produse WHERE denumire like @numeProdus ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@numeProdus", numeProdus);

                    connection.Open();
                    int result = (Int32)command.ExecuteScalar();


                    return result;
                }
            }
        }

        public decimal pretProdus(int idProdus)
        {
            using (SqlConnection connection = new SqlConnection(Helper.Conectare()))
            {
                String query = "SELECT pret From dbo.Produse WHERE id like @idProdus ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@idProdus", idProdus);

                    connection.Open();
                    decimal result = (decimal)command.ExecuteScalar();

                    return result;
                }
            }
        }

        public void stergeProdusComanda(int idComanda, int idProdus)
        {
            using (SqlConnection connection = new SqlConnection(Helper.Conectare()))
            {
                String query = "DELETE FROM dbo.ProduseComanda WHERE idComanda =  @idComanda AND idProdus = @idProdus";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@idComanda", idComanda);
                    command.Parameters.AddWithValue("@idProdus", idProdus);
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                }
            }
        }

        public void modificaCantitate(int idComanda, int idProdus, int cantitate, decimal pretProdus)
        {
            using (SqlConnection connection = new SqlConnection(Helper.Conectare()))
            {
                String query = "UPDATE dbo.ProduseComanda SET cantitate = @cantitate, valoare = @valoare  WHERE idComanda = @idComanda AND idProdus = @idProdus ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cantitate", cantitate);
                    command.Parameters.AddWithValue("@valoare", cantitate * pretProdus);
                    command.Parameters.AddWithValue("@idComanda", idComanda);
                    command.Parameters.AddWithValue("@idProdus", idProdus);
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                }
            }
        }




    }
}
