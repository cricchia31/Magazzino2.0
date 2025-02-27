﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCF_Server
{
    class DB
    {
        public DB()
        {
            this.port= "3306;";
            this.name = "magazzino";
            this.user = "root;";
            this.passw= ";";
            this.address = "127.0.0.1;";
        }

        string port { get; set; }
        string name { get; set; }
        string user { get; set; }
        string passw { get; set; }
        string address { get; set; }


        //stringa di connessione al DB
        public string connectstring()
        {
            string con = "datasource=" + address + "port=" + port + "username=" + user + "password=" + passw + "database=" + name;

            return con;
        }

        //connessione a MysqlConnection
        public MySqlConnection getsqlconnect(string c)
        {
            MySqlConnection dbconnect = new MySqlConnection(c);
            return dbconnect;
        }

        //metodo per login
        public DipendenteServer accessoutenti(MySqlConnection x, int n, string p)
        {
            try
            {
                DipendenteServer ds1 = new DipendenteServer();

                //apertura connessione al DB
                x.Open(); 

                using (MySqlCommand command1 = x.CreateCommand())
                {
                    //query login
                    command1.CommandText = "SELECT * FROM `dipendente` WHERE dipendente.Password='" + p + "' AND dipendente.IDDipendente=" + n + ";";

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //legge i risultati ottenuti dalla query, in questo caso ritorna solo l id
                            //di chi ha fatto l accesso 
                            ds1.id = reader.GetInt32(0);
                            ds1.nome = reader.GetString(1);
                            ds1.cognome = reader.GetString(2);
                            ds1.telefono = reader.GetString(3);
                            ds1.password = reader.GetString(4);
                            var ceo = reader.GetInt32(5);
                            if (ceo == 1)
                                ds1.amministratore = true;
                            else
                                ds1.amministratore = false;

                            x.Close(); //chiudiamo la connessione al DB
                            return ds1;
                        }
                        x.Close();
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                x.Close();
                return null;
            }
        }

        //metodo che ritorna la lista di tutti i dipendenti
        public ListaDipendentiServer getListaDipendenti(MySqlConnection x)
        {
            try
            {
                ListaDipendentiServer lps = new ListaDipendentiServer();
                x.Open();
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    //query che ritorna tutti i dipendenti
                    command1.CommandText = "SELECT * " +
                    "FROM Dipendente ;";

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //legge i risultati ottenuti dalla query, in questo caso ritorna i dipendendti
                            var id = reader.GetInt32(0);
                            var nome = reader.GetString(1);
                            var cognome = reader.GetString(2);
                            var telefono = reader.GetString(3);
                            var psw = reader.GetString(4);
                            var amministrator = reader.GetInt32(5);
                            var ceo = false;
                            if (amministrator == 1)
                            {
                                ceo = true;
                            }

                            DipendenteServer ps = new DipendenteServer(id, nome, cognome, telefono, psw, ceo);
                            lps.listaDipendServer.Add(ps);
                        }
                        x.Close();
                        return lps;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                return null;
            }
        }

        //metodo che ritorna la categoria che ha un certo ID passatogli come argomento
        public ListaProdottiServer getListaProdottiByCategory(MySqlConnection x, int idcat)
        {
            try
            {
                ListaProdottiServer lps = new ListaProdottiServer();
                x.Open();
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    //query che ritorna la categoria con quell'ID
                    command1.CommandText = "SELECT * "+
                                            "FROM prodotto "+
                                            "WHERE prodotto.IDCategoria = "+idcat+";";

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //legge i risultati ottenuti dalla query, in questo caso ritorna i prodotti che hanno una determinata categoria
                            var id = reader.GetInt32(0);
                            var nome = reader.GetString(1);
                            var idProduttore = reader.GetInt32(2);
                            var idCat = reader.GetInt32(3);
                            var prezzo = reader.GetFloat(4);
                            var quantita = reader.GetInt32(5);
                            var posizione = reader.GetString(6);

                            ProdottoServer ps = new ProdottoServer(id, nome, idProduttore, prezzo, idCat, quantita, posizione);
                            lps.listaProducts.Add(ps);
                        }
                        x.Close();
                        return lps;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                return null;
            }
        }

        //metodo che ritorna tutta la lista dei prodotti del DB
        public ListaProdottiServer getListaProdotti(MySqlConnection x)
        {
            try
            {
                ListaProdottiServer lps = new ListaProdottiServer();
                x.Open();              
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    //query che ritorna tutti i prodotti del DB
                    command1.CommandText = "SELECT * " +
                    "FROM PRODOTTO ;";

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //legge i risultati ottenuti dalla query, in questo caso ritorna i prodotti
                            var id = reader.GetInt32(0);
                            var nome = reader.GetString(1);
                            var idProduttore = reader.GetInt32(2);
                            var idCat = reader.GetInt32(3);
                            var prezzo = reader.GetFloat(4);
                            var quantita = reader.GetInt32(5);
                            var posizione = reader.GetString(6);

                            ProdottoServer ps = new ProdottoServer(id, nome, idProduttore,prezzo, idCat, quantita, posizione);
                            lps.listaProducts.Add(ps);
                        }
                        x.Close();
                        return lps;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                return null;
            }
        }

        //metodo che ritorna un prodotto che ha un certo ID passatogli come argomento
        public ProdottoServer getProdottoById(MySqlConnection x, int n)
        {
            try
            {
                x.Open();
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    //query che ritorna il prodotto con quell'ID
                    command1.CommandText = "SELECT * " +
                    "FROM PRODOTTO " +
                    "WHERE PRODOTTO.IDPRODOTTO =" + n + ";";

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //legge i risultati ottenuti dalla query, in questo caso ritorna il prodotto cercato con id N
                            var id = reader.GetInt32(0);
                            var nome = reader.GetString(1);
                            var idProduttore = reader.GetInt32(2);
                            var idCat = reader.GetInt32(3);
                            var prezzo = reader.GetFloat(4);
                            var quantita = reader.GetInt32(5);
                            var posizione = reader.GetString(6);

                            ProdottoServer ps = new ProdottoServer(id, nome, idProduttore, prezzo, idCat, quantita, posizione);
                            return ps;
                        }
                        x.Close();
                    }
                    x.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                return null;
            }
        }

        //metodo che ritorna un dipendente che ha un certo ID passatogli come argomento
        public DipendenteServer getUtenteById(MySqlConnection x, int n)
        {
            try
            {
                x.Open();
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    //query che ritorna il dipendente con quell'ID
                    command1.CommandText = "SELECT * " +
                    "FROM Dipendente " +
                    "WHERE Dipendente.IDDipendente =" + n + ";";

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //legge i risultati ottenuti dalla query, in questo caso ritorna l user cercato con id N
                            var id = reader.GetInt32(0);
                            var nome = reader.GetString(1);
                            var cognome = reader.GetString(2);
                            var telefono = reader.GetString(3);
                            var psw = reader.GetString(4);
                            var amministrator = reader.GetInt32(5);
                            var ceo = false;
                            if (amministrator == 1)
                            {
                                ceo = true;
                            }

                            DipendenteServer ps = new DipendenteServer(id, nome, cognome, telefono, psw, ceo);
                            return ps;
                        }
                        x.Close();
                    }
                    x.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                return null;
            }
        }

        //metodo che ritorna tutte le posizioni libere
        public List<String> getFreePosition(MySqlConnection x)
        {
            List<String> posti = new List<string>();
            try
            {
                x.Open();
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    //query che ritorna tutte le posizioni con disponibilità = 1 (cioè libere)
                    command1.CommandText = "SELECT * " +
                    "FROM POSIZIONE " +
                    "WHERE POSIZIONE.DISPONIBILE = 1;";

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //legge i risultati ottenuti dalla query, in questo caso ritorna i posti disponibili
                            posti.Add(reader.GetString(0));
                        }
                        x.Close();
                        return posti;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                return null;
            }
        }

        //metodo che modifica il prodotto, aggiorna i posti e crea l'operazione
        public bool ProductUpdate(MySqlConnection x,ProdottoServer p, int idDip, string desc, string date)
        {
            //dichiariamo la transazione e la facciamo partire
            MySqlTransaction transaction;
            x.Open();
            transaction = x.BeginTransaction();

            try
            {
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    // Must assign both transaction object and connection
                    // to Command object for a pending local transaction
                    command1.Connection = x;
                    command1.Transaction = transaction;

                    //Mettiamo la vecchia posizione del prodotto come disponibile
                    command1.CommandText = "UPDATE posizione SET Disponibile=1 " +
                              "WHERE posizione.IDPosizione = (SELECT posizione.IDPosizione " +
                              "FROM posizione, prodotto " +
                              "WHERE posizione.IDPosizione = prodotto.IDPosizione AND prodotto.IDProdotto =" + p.id + ");";
                    command1.ExecuteNonQuery();

                    //aggiorniamo la posizione e la quantità
                    command1.CommandText = "UPDATE prodotto SET Quantita=@Quantita,IDPosizione=@Posizione WHERE IDProdotto=@IDProdotto;";
                    command1.Parameters.AddWithValue("@Quantita", p.quantita);
                    command1.Parameters.AddWithValue("@Posizione", p.posizione);
                    command1.Parameters.AddWithValue("@IDProdotto", p.id);
                    command1.ExecuteNonQuery();

                    //mettiamo la nuova posizione del prodotto non disponibile
                    command1.CommandText = "UPDATE posizione SET Disponibile=0 " +
                       "WHERE posizione.IDPosizione = (SELECT posizione.IDPosizione " +
                                                        "FROM posizione, prodotto " +
                                                        "WHERE posizione.IDPosizione = prodotto.IDPosizione AND prodotto.IDProdotto =" + p.id + ");";
                    command1.ExecuteNonQuery();
                    command1.CommandText = "INSERT INTO operazione(IDOperazione, IDDipendente, Data, Descrizione, IDProdotto)VALUES(null," + idDip + ", '" + date + "', '" + desc + "'," + p.id + ");";
                    command1.ExecuteNonQuery();

                    transaction.Commit();

                    x.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                // In caso di errore chiamiamo la Rollback
                try
                {
                    //vengono annulate le modifiche in caso di errore e si ripristina il db a prima che si effettuasse la query
                    transaction.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    return false;
                }
            }
        }

        //metodo che crea un nuovo utente (passatogli come argomento)
        public bool CreaUtente(MySqlConnection x, DipendenteServer ds)
        {
            //dichiariamo la transazione e la facciamo partire
            x.Open();
            var transaction = x.BeginTransaction();

            try
            {
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    var ceo = 0;
                    if (ds.amministratore == true)
                        ceo = 1;

                    // Must assign both transaction object and connection
                    // to Command object for a pending local transaction
                    command1.Connection = x;
                    command1.Transaction = transaction;
                    //query che crea il nuovo dipendente
                    command1.CommandText = "INSERT INTO Dipendente(IDDipendente, Nome, Cognome, Telefono, Password, Amministratore)VALUES(null,'" + ds.nome + "', '" + ds.cognome + "', '" + ds.telefono + "','" + ds.password + "'," + ceo + ");";
                    command1.ExecuteNonQuery();
                    transaction.Commit();

                    x.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                // In caso di errore chiamiamo la Rollback
                 try
                 {
                     //vengono annulate le modifiche in caso di errore e si ripristina il db a prima che si effettuasse la query
                     transaction.Rollback();
                     return false;
                 }
                 catch (Exception ex2)
                 {
                     Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                     Console.WriteLine("  Message: {0}", ex2.Message);
                     return false;
                 }
            }
        }

        //metodo che elimina il dipendente passatogli come argomento e le sue operazioni eseguite
        public bool EliminaDipendente(MySqlConnection x, DipendenteServer ds)
        {
            //dichiariamo la transazione e la facciamo partire
            x.Open();
            var transaction = x.BeginTransaction();

            try
            {
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    // Must assign both transaction object and connection
                    // to Command object for a pending local transaction
                    command1.Connection = x;
                    command1.Transaction = transaction;

                    //elimino le operazioni con quel dipendente
                    command1.CommandText = "DELETE operazione FROM operazione WHERE operazione.IDDipendente = " + ds.id + ";";
                    command1.ExecuteNonQuery();

                    //elimino il dipendente tramite il suo id
                    command1.CommandText = "DELETE dipendente FROM dipendente WHERE dipendente.IDDipendente = " + ds.id + ";";
                    command1.ExecuteNonQuery();

                    transaction.Commit();

                    x.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                // In caso di errore chiamiamo la Rollback
                try
                {
                    //vengono annulate le modifiche in caso di errore e si ripristina il db a prima che si effettuasse la query
                    transaction.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    return false;
                }
            }
        }

        //metodo che crea un prodotto (passatogli come parametro) e aggiorna i posti disponibili
        public bool CreaProdotto(MySqlConnection x, ProdottoServer ps)
        {
            //dichiariamo la transazione e la facciamo partire
            x.Open();
            var transaction = x.BeginTransaction();

            try
            {
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    // Must assign both transaction object and connection
                    // to Command object for a pending local transaction
                    command1.Connection = x;
                    command1.Transaction = transaction;

                    //query che crea il prodotto
                    command1.CommandText = "INSERT INTO Prodotto(IDProdotto, Nome, IDProduttore, IDCategoria, PrezzoVendita, Quantita, IDPosizione)VALUES(null,'" + ps.nome + "', " + ps.produttore + ", " + ps.categoria + "," + ps.prezzo + "," + ps.quantita + ",'" + ps.posizione + "');";
                    command1.ExecuteNonQuery();

                    //aggiorniamo la posizione, che diventerà occupata
                    command1.CommandText = "UPDATE posizione SET Disponibile=0 " +
                                            "WHERE posizione.IDPosizione = '" + ps.posizione + "' ;";
                    command1.ExecuteNonQuery();

                    transaction.Commit();

                    x.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                // In caso di errore chiamiamo la Rollback
                try
                {
                    //vengono annulate le modifiche in caso di errore e si ripristina il db a prima che si effettuasse la query
                    transaction.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    return false;
                }
            }
        }

        //metodo che elimina il prodotto che gli viene passato come argomento, tutte le sue operazioni e aggiorna i posti
        public bool EliminaProdotto(MySqlConnection x, ProdottoServer ps)
        {
            //dichiariamo la transazione e la facciamo partire
            x.Open();
            var transaction = x.BeginTransaction();

            try
            {
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    // Must assign both transaction object and connection
                    // to Command object for a pending local transaction
                    command1.Connection = x;
                    command1.Transaction = transaction;

                    //elimino le operazioni con quel prodotto
                    command1.CommandText = "DELETE operazione FROM operazione WHERE operazione.IDProdotto = " + ps.id + ";";
                    command1.ExecuteNonQuery();

                    //elimino il prodotto tramite il suo id
                    command1.CommandText = "DELETE prodotto FROM prodotto WHERE prodotto.IDProdotto = " + ps.id + ";";
                    command1.ExecuteNonQuery();

                    //aggiorniamo la posizione, che diventerà disponibie
                    command1.CommandText = "UPDATE posizione SET Disponibile=1 " +
                                            "WHERE posizione.IDPosizione = '" + ps.posizione + "' ;";
                    command1.ExecuteNonQuery();

                    transaction.Commit();

                    x.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                // In caso di errore chiamiamo la Rollback
                try
                {
                    //vengono annulate le modifiche in caso di errore e si ripristina il db a prima che si effettuasse la query
                    transaction.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    return false;
                }
            }
        }

        //metodo che ritorna la lista di nomi di tutte le categorie
        public List<String> getListaCategorie(MySqlConnection x)
        {
            try
            {
                List<String> categorie = new List<string>();
                x.Open();
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    //ritorno tutti i nomi delle categorie
                    command1.CommandText = "SELECT CATEGORIA.NOME " +
                                           "FROM CATEGORIA ;";

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //legge i risultati ottenuti dalla query, in questo caso ritorna i nomi delle categorie
                            var nome = reader.GetString(0);

                            categorie.Add(nome);
                        }
                        x.Close();
                        return categorie;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                return null;
            }
        }

        //metodo che ritorna la lista di nomi di tutti i produttori
        public List<String> getListaProduttori(MySqlConnection x)
        {
            try
            {
                List<String> produttori = new List<string>();
                x.Open();
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    //ritorno tutti i nomi dei produttori
                    command1.CommandText = "SELECT PRODUTTORE.NOME " +
                                           "FROM PRODUTTORE ;";

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //legge i risultati ottenuti dalla query, in questo caso ritorna i nomi dei produttori
                            var nome = reader.GetString(0);
                            produttori.Add(nome);
                        }
                        x.Close();
                        return produttori;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                return null;
            }
        }

        //metodo del CEO che modifica il prodotto, aggiorna i posti e crea l'operazione
        //come il precedente, ma se volessimo fare qualche altra modifica per il ceo modificheremo solo questo
        public bool ProductUpdateCeo(MySqlConnection x, ProdottoServer p1, int idUser, string date, string desc)
        {
            //dichiariamo la transazione e la facciamo partire
            MySqlTransaction transaction;
            x.Open();
            transaction = x.BeginTransaction();

            try
            {
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    // Must assign both transaction object and connection
                    // to Command object for a pending local transaction
                    command1.Connection = x;
                    command1.Transaction = transaction;

                    //Mettiamo la vecchia posizione del prodotto come disponibile
                    command1.CommandText = "UPDATE posizione SET Disponibile=1 " +
                              "WHERE posizione.IDPosizione = (SELECT posizione.IDPosizione " +
                              "FROM posizione, prodotto " +
                              "WHERE posizione.IDPosizione = prodotto.IDPosizione AND prodotto.IDProdotto =" + p1.id + ");";
                    command1.ExecuteNonQuery();

                    //aggiorniamo la posizione e la quantità
                    command1.CommandText = "UPDATE prodotto SET Nome=@Nome, IDProduttore=@IDProduttore, IDCategoria=@IDCategoria, PrezzoVendita=@PrezzoVendita, Quantita=@Quantita,IDPosizione=@Posizione WHERE IDProdotto=@IDProdotto;";
                    command1.Parameters.AddWithValue("@Nome", p1.nome);
                    command1.Parameters.AddWithValue("@IDProduttore", p1.produttore);
                    command1.Parameters.AddWithValue("@IDCategoria", p1.categoria);
                    command1.Parameters.AddWithValue("@PrezzoVendita", p1.prezzo);
                    command1.Parameters.AddWithValue("@Quantita", p1.quantita);
                    command1.Parameters.AddWithValue("@Posizione", p1.posizione);
                    command1.Parameters.AddWithValue("@IDProdotto", p1.id);
                    command1.ExecuteNonQuery();

                    //mettiamo la nuova posizione del prodotto non disponibile
                    command1.CommandText = "UPDATE posizione SET Disponibile=0 " +
                       "WHERE posizione.IDPosizione = (SELECT posizione.IDPosizione " +
                                                        "FROM posizione, prodotto " +
                                                        "WHERE posizione.IDPosizione = prodotto.IDPosizione AND prodotto.IDProdotto =" + p1.id + ");";
                    command1.ExecuteNonQuery();
                    command1.CommandText = "INSERT INTO operazione(IDOperazione, IDDipendente, Data, Descrizione, IDProdotto)VALUES(null," + idUser + ", '" + date + "', '" + desc + "'," + p1.id + ");";
                    command1.ExecuteNonQuery();

                    transaction.Commit();

                    x.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                // In caso di errore chiamiamo la Rollback
                try
                {
                    //vengono annulate le modifiche in caso di errore e si ripristina il db a prima che si effettuasse la query
                    transaction.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    return false;
                }
            }
        }

        //metodo del CEO che modifica il dipendente
        public bool UserUpdateCeo(MySqlConnection x, DipendenteServer d1)
        {
            //dichiariamo la transazione e la facciamo partire
            MySqlTransaction transaction;
            x.Open();
            transaction = x.BeginTransaction();

            try
            {
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    var ceo = 0;
                    if (d1.amministratore == true)
                    {
                        ceo = 1;
                    }

                    // Must assign both transaction object and connection
                    // to Command object for a pending local transaction
                    command1.Connection = x;
                    command1.Transaction = transaction;

                    //aggiorniamo i dati del dipendente
                    command1.CommandText = "UPDATE dipendente SET Nome=@Nome, Cognome=@Cognome, Telefono=@Telefono, Password=@Password, Amministratore=@Amministratore WHERE IDDipendente=@IDDipendente;";
                    command1.Parameters.AddWithValue("@Nome", d1.nome);
                    command1.Parameters.AddWithValue("@Cognome", d1.cognome);
                    command1.Parameters.AddWithValue("@Telefono", d1.telefono);
                    command1.Parameters.AddWithValue("@Password", d1.password);
                    command1.Parameters.AddWithValue("@Amministratore", ceo);
                    command1.Parameters.AddWithValue("@IDDipendente", d1.id);
                    command1.ExecuteNonQuery();

                    transaction.Commit();

                    x.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                // In caso di errore chiamiamo la Rollback
                try
                {
                    //vengono annulate le modifiche in caso di errore e si ripristina il db a prima che si effettuasse la query
                    transaction.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    return false;
                }
            }
        }



        //ritorno il nome dei prodotti in esaurimento
        public List<string> getProductInExhaustion(MySqlConnection x)
        {
            try
            {
                List<string> prodotti = new List<string>();
                x.Open();
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    //ritorno tutti i nomi dei prodotti con quantità <=3
                    command1.CommandText = "SELECT PRODOTTO.NOME " +
                    "FROM PRODOTTO " +
                    "WHERE PRODOTTO.QUANTITA<=3;";

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //legge i risultati ottenuti dalla query, in questo caso ritorna i nomi dei prodotti
                            var nome = reader.GetString(0);

                            prodotti.Add(nome);
                        }
                        x.Close();
                        return prodotti;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                return null;
            }
        }

        //ritorna tutte le operazioni del DB
        public ListaOperazioniServer getOperazioni(MySqlConnection x)
        {
            try
            {
                ListaOperazioniServer operazioni = new ListaOperazioniServer();
                x.Open();
                using (MySqlCommand command1 = x.CreateCommand())
                {
                    //ritorno tutte le operazioni
                    command1.CommandText = "SELECT * " +
                    "FROM OPERAZIONE;";

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //legge i risultati ottenuti dalla query, in questo caso ritorna tutte le operazioni
                            var idOpe = reader.GetInt32(0);
                            var idDip = reader.GetInt32(1);
                            var data = reader.GetDateTime(2);
                            var descr = reader.GetString(3);
                            var idProd = reader.GetInt32(4);

                            OperazioneServer ops = new OperazioneServer(idOpe, idDip, data, descr, idProd);
                            operazioni.listaOpe.Add(ops);
                        }
                        x.Close();
                        return operazioni;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE: " + e.ToString());
                return null;
            }
        }
    }
}
