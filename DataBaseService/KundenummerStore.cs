using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using KundenummerGenerator.Model;

namespace KundenummerGenerator.DataBaseService;

/// <summary>
/// Klasse für alle Datenbank Interaktionen mit Kundennummer.
/// </summary>
public class KundenummerStore
{
  #region Constructors

  /// <summary>
  /// Initialisiert eine neue Instanz vom KundennummerStore.
  /// </summary>
  public KundenummerStore()
  {
    LoadKundennummern();
  }

  #endregion

  #region Public Properties

  /// <summary>
  /// Liste die die Kundennummern beinhaltet.
  /// </summary>
  public ObservableCollection<Kundennummer> KundenNummer { get; private set; } = new ObservableCollection<Kundennummer>();

  #endregion

  #region Public Methods

  /// <summary>
  /// Funktion zum hinzufügen einer Kundennummer in die Datenbank.
  /// </summary>
  /// <param name="kundennummer"></param>
  /// <returns></returns>
  public Kundennummer AddKundennummer( Kundennummer kundennummer)
  {
    //Kontrollfunktion um zu überprüfen ob es die Kundennummer bereits gibt.
    var kundennummer2 = KundenNummer.FirstOrDefault(k => k.KundenNummer.Equals(kundennummer.KundenNummer, StringComparison.InvariantCultureIgnoreCase));
    if (kundennummer2 is not null)
      return kundennummer2;

    using (var conn = new SqlConnection(connectionString))
    {
      var sql =
        "INSERT INTO Kundennummer (Id, Nummer)" +
        "VALUES(@Id, @Nummer)";


      using var command = new SqlCommand(sql, conn);
      command.Parameters.AddWithValue("@Id", kundennummer.Id);
      command.Parameters.AddWithValue("@Nummer", kundennummer.KundenNummer);


      command.Connection.Open();
      using var reader = command.ExecuteReader();
    }

    KundenNummer.Add(kundennummer);
    return kundennummer;
  }

  #endregion

  #region Private Methods

  /// <summary>
  /// Funtkion zum Laden aller Kundennummern in der Datenbank und fügt dieser der Liste hinzu. 
  /// </summary>
  /// <param name="reinitialize"></param>
  private void LoadKundennummern(Boolean reinitialize = false)
  {
    if (KundenNummer.Count > 0 && !reinitialize)
      return;
    if (reinitialize)
      KundenNummer.Clear();

    using (var conn = new SqlConnection(connectionString))
    {
      var query = "SELECT * " +
                  "FROM Kundennummer";
      var cmd = new SqlCommand(query, conn);
      cmd.Connection.Open();
      var reader = cmd.ExecuteReader();
      while (reader.Read())
      {
        KundenNummer.Add(new Kundennummer(reader.GetGuid("Id"), reader.GetString("Nummer")));
      }
    }
  }

  #endregion

  #region Private Fields

  /// <summary>
  /// String der die Verbindung zur SQL Datenbank ermöglicht. 
  /// </summary>
  private readonly String connectionString = @"Data Source=(localdb)\MSSQLLocalDB ;Initial Catalog=KundenummerDb; Integrated Security=true ";

  #endregion
}