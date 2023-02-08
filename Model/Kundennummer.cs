using System;
using System.Diagnostics.CodeAnalysis;

namespace KundenummerGenerator.Model;

/// <summary>
/// This is a class representing a Kundennummer.
/// </summary>
public class Kundennummer
{
  #region Constructors

  /// <summary>
  /// Creates a new instance of Kundennummer.
  /// </summary>
  /// <param name="id">Unique Identifier of the entity</param>
  /// <param name="kundennummer">The Kundennummer</param>
  public Kundennummer([NotNull] Guid id, [NotNull] String kundennummer)
  {
    KundenNummer = kundennummer;
    Id = id;
  }

  #endregion

  #region Public Properties

  public Guid Id { get; set; }

  public String KundenNummer { get; set; }



  #endregion
}