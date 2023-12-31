﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace w15_1.Models
{
    public class Clienti
    {
        public int IdCliente { get; set; }
        [Required(ErrorMessage = "Il campo nome non è valido")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Il campo indirizzo non è valido")]
        public string Indirizzo { get; set; }
        [Required(ErrorMessage = "Il campo città non è valido")]
        public string Città { get; set; }
       
        [Display(Name = "Codice fiscale")]
        [StringLength(16,MinimumLength =16, ErrorMessage ="Il campo richiede 16 caratteri")]
        public string CodiceFiscale { get; set; }
        [Display(Name = " Partita iva")]
        public string PartitaIva { get; set; }

        [Display(Name ="Sei un azienda?")]
        [Required(ErrorMessage = "Il campo non è valido")]
        public bool IsAzienda { get; set; }

        public static List<Clienti> ListClienti = new List<Clienti>();
        public static List<SelectListItem> DropdownClienti = new List<SelectListItem>();

        //select
        public static void Select()
        {

            string connection = ConfigurationManager.ConnectionStrings["ConnectionDB"]
             .ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connection);
            SqlCommand cmd1 = new SqlCommand("SELECT * From Clienti", conn);
            SqlDataReader sqlreader1;
            conn.Open();
            sqlreader1 = cmd1.ExecuteReader();

            while (sqlreader1.Read())
            {
                Clienti c = new Clienti();
                c.Nome = sqlreader1["Nome"].ToString();
                c.Indirizzo =sqlreader1["Indirizzo"].ToString();
                c.Città = sqlreader1["Città"].ToString();
                c.CodiceFiscale = sqlreader1["CodiceFiscale"].ToString().ToUpper();
                c.PartitaIva = sqlreader1["PartitaIva"].ToString();
                c.IsAzienda = Convert.ToBoolean(sqlreader1["IsAzienda"]);
                c.IdCliente = Convert.ToInt16(sqlreader1["IdCliente"]);
                ListClienti.Add(c);

            }
            conn.Close();
           

        }
        public static void SelectWhereId(int id)
        {

            string connection = ConfigurationManager.ConnectionStrings["ConnectionDB"]
             .ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connection);
            SqlCommand cmd1 = new SqlCommand("SELECT *  FROM Clienti WHERE IdCliente=@id", conn);
            SqlDataReader sqlreader1;
            conn.Open();

            cmd1.Parameters.AddWithValue("id", HttpContext.Current.Request.QueryString["Id"]);
            sqlreader1 = cmd1.ExecuteReader();

            while (sqlreader1.Read())
            {
                Clienti c = new Clienti();
                c.Nome = sqlreader1["Nome"].ToString();
                c.Indirizzo = sqlreader1["Indirizzo"].ToString();
                c.Città = sqlreader1["Città"].ToString();
                c.CodiceFiscale = sqlreader1["CodiceFiscale"].ToString();
                c.PartitaIva = sqlreader1["PartitaIva"].ToString();
                c.IsAzienda = Convert.ToBoolean(sqlreader1["IsAzienda"]);
                c.IdCliente = Convert.ToInt16(id);

                ListClienti.Add(c);

            }
            conn.Close();
        }

        public static List<Clienti> SelectNome()
        {

            string connection = ConfigurationManager.ConnectionStrings["ConnectionDB"]
             .ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connection);
            SqlCommand cmd1 = new SqlCommand("SELECT Nome From Clienti", conn);
            SqlDataReader sqlreader1;
            conn.Open();
            sqlreader1 = cmd1.ExecuteReader();

            while (sqlreader1.Read())
            {
                Clienti c = new Clienti();
                c.Nome = sqlreader1["Nome"].ToString();
                ListClienti.Add(c);

            }
            conn.Close();
            return ListClienti;

        }
        public static void Dropdown()
        {
            List<Clienti> sped = new List<Clienti>();
            sped = SelectNome();
            foreach (Clienti item in sped)
            {
                SelectListItem l = new SelectListItem { Text = item.Nome, Value = item.IdCliente.ToString() };
                DropdownClienti.Add(l);
            }


        }

        //Insert modifica elimina
        public static void Insert(Clienti c,string messaggio)
    {
            string connection = ConfigurationManager.ConnectionStrings["ConnectionDB"]
           .ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connection);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO Clienti VALUES(@Nome,@Indirizzo,@CodiceFiscale,@PartitaIva,@Città,@TipoCliente)";
                cmd.Parameters.AddWithValue("Nome", c.Nome);
                cmd.Parameters.AddWithValue("Indirizzo", c.Indirizzo);
              
                if (c.PartitaIva.IsEmpty())
                {
                cmd.Parameters.AddWithValue("PartitaIva", "NULL");
                }
                else
                {
                 cmd.Parameters.AddWithValue("PartitaIva", c.PartitaIva);
                }
                if (c.CodiceFiscale.IsEmpty())
                {
                  cmd.Parameters.AddWithValue("CodiceFiscale", "NULL");
                }
                else
                {
                    cmd.Parameters.AddWithValue("CodiceFiscale", c.CodiceFiscale);
                }

                cmd.Parameters.AddWithValue("Città", c.Città);
                cmd.Parameters.AddWithValue("TipoCliente", c.IsAzienda);


                int inserimentoEffettuato = cmd.ExecuteNonQuery();

                if (inserimentoEffettuato > 0)
                {
                    messaggio = "Inserimento effetuato con successo";
                }

            }
            catch (Exception ex) {
                // messaggio = $"{ex}";
                }
            finally { conn.Close(); }
        }
        public static void Elimina()
        {
            string connection = ConfigurationManager.ConnectionStrings["ConnectionDB"]
            .ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connection);
            SqlCommand cmd1 = new SqlCommand("SELECT * , Nome FROM Spedizione INNER JOIN Clienti ON Spedizione.Cliente=Clienti.IdCliente inner join Aggiornamenti on Spedizione.IdSpedizione=Aggiornamenti.IdSpedizioni inner join StatoSpedizione on Aggiornamenti.StatoSpedizione=StatoSpedizione.IdStato where Cliente=@id ", conn);
            SqlDataReader sqlreader1;
            conn.Open();
            cmd1.Parameters.AddWithValue("id", HttpContext.Current.Request.QueryString["Id"]);
            sqlreader1 = cmd1.ExecuteReader();
            Spedizioni p = new Spedizioni();
            while (sqlreader1.Read())
            {
               
                p.NomeCliente = sqlreader1["Nome"].ToString();
                p.Città = sqlreader1["Città"].ToString();
                p.Indirizzo = sqlreader1["Indirizzo"].ToString();
                p.NominativoDestinatario = sqlreader1["NominativoDestinatario"].ToString();
                p.CostoSpedizione = Convert.ToDouble(sqlreader1["CostoSpedizione"]);
                p.DataSpedizione = Convert.ToDateTime(sqlreader1["DataSpedizione"]);
                p.DataC = Convert.ToDateTime(sqlreader1["DataConsegna"]).ToShortDateString();
                p.DataS = Convert.ToDateTime(sqlreader1["DataSpedizione"]).ToShortDateString();
                p.DataConsegna = Convert.ToDateTime(sqlreader1["DataConsegna"]);
                p.Peso = Convert.ToInt16(sqlreader1["Peso"]);
                p.IdSpedizione = Convert.ToInt16(sqlreader1["IdSpedizione"]);
                p.Nomestato = sqlreader1["NomeStato"].ToString();
                p.Idstato = Convert.ToInt16(sqlreader1["IdStato"]);
                p.StatoSpedizione = Convert.ToInt16(sqlreader1["StatoSpedizione"]);
                p.IdAggiornamento = Convert.ToInt16(sqlreader1["IdAggiornamento"]);
                p.Luogo = sqlreader1["Luogo"].ToString();
                p.Descrizione = sqlreader1["Descrizione"].ToString();
                p.DataOraAggiornamento = Convert.ToDateTime(sqlreader1["DataOraAggiornamento"]);
                p.IdSpedizioni = Convert.ToInt16(sqlreader1["IdSpedizioni"]);
               

            }
            conn.Close();

            SqlCommand cmd4 = new SqlCommand();
            cmd4.Connection = conn;

            //cmd4.CommandText = "DELETE FROM StatoSpedizione where Idstato =@id";
            //cmd4.Parameters.AddWithValue("id", p.Idstato);

            //conn.Open();

            //cmd4.ExecuteNonQuery();

            //conn.Close();


            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = conn;

            cmd2.CommandText = "DELETE FROM Aggiornamenti where IdSpedizioni =@id";
            cmd2.Parameters.AddWithValue("id", p.IdSpedizioni);

            conn.Open();

            cmd2.ExecuteNonQuery();

            conn.Close();

            conn.Open();
            SqlCommand cmd3 = new SqlCommand();
            cmd3.Connection = conn;
            cmd3.CommandText = "DELETE FROM Spedizione where Cliente =@id";
            cmd3.Parameters.AddWithValue("id", HttpContext.Current.Request.QueryString["Id"]);



            cmd3.ExecuteNonQuery();

            conn.Close();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM Clienti where IdCliente = @id";
            cmd.Parameters.AddWithValue("id", HttpContext.Current.Request.QueryString["Id"]);

            conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();


        }
        public static void Modifica(Clienti s)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionDB"].ConnectionString.ToString();
            SqlConnection conn2 = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn2;
            cmd.CommandText = "UPDATE Clienti SET Nome=@Nome,Indirizzo=@Indirizzo,CodiceFiscale=@CodiceFiscale,PartitaIva=@PartitaIva,Città=@Città,IsAzienda=@IsAzienda  where IdCliente=@id";

            cmd.Parameters.AddWithValue("id", HttpContext.Current.Request.QueryString["Id"]);
            cmd.Parameters.AddWithValue("Nome", s.Nome);
            cmd.Parameters.AddWithValue("Indirizzo", s.Indirizzo);
            cmd.Parameters.AddWithValue("CodiceFiscale", s.CodiceFiscale);
            cmd.Parameters.AddWithValue("PartitaIva", s.PartitaIva);
            cmd.Parameters.AddWithValue("Città", s.Città);
            cmd.Parameters.AddWithValue("IsAzienda", s.IsAzienda);


            conn2.Open();

            cmd.ExecuteNonQuery();

            conn2.Close();
        }
    }
}