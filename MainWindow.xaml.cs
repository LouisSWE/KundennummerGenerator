﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using KundenummerGenerator.DataBaseService;
using KundenummerGenerator.Model;

namespace KundenummerGenerator
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    #region Constructors

    public MainWindow()
    {
      InitializeComponent();
      kundenummerStore = new KundenummerStore();
      LoadKundennummerUI();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Funktion zum erzeugen der Kundennummer und dem ausführen des hinzufügens der Nummer in die Datenbank.
    /// </summary>
    /// <param name="value"></param>
    private void CreateandAddKundennummer(String value)

    {
      //Ist der String für die Kundennummer
      String nummer = String.Empty;

      //Ist eine Variable um zufällige zahlen in ein Array zu befüllen
      Int32 w;

      //Behinhaltet die zufälligen Zahlen von den Stellen 3-8 in vorm eines Strings
      String stellen3Bis8 = String.Empty;

      var quersumme = 0;

      //Ist das Array was die zufälligen Zahlen beinhaltet
      var stellen3Bis8Rnd = new Int32[7];

      //Fügt 7 zufällig generierte Zahlen in ein Array hinzu und erstellt die Quersumme dieser zahlen.
      for (var i = 0; i < 7; i++)
      {
        var rnd = new Random();
        w = rnd.Next(1, 9);
        stellen3Bis8Rnd[i] = w;

        quersumme += w;
      }

      //Nimmt jedes Element aus dem Array und fügt dieses zu einem String zur weiteren Verwendung
      foreach (var item in stellen3Bis8Rnd)
      {
        stellen3Bis8 += item.ToString();
      }

      if (quersumme < 10)
      {
        //quersumme zum string
        var s = quersumme.ToString();

        //quersumme als string um eine 0 vor die eigentliche quersumme zu machen wenn die quersumme nicht 2 stellig ist
        String quersummes = s.Insert(0,"0");

        //String der die Kundennummer darstellt
        nummer = $"KD{stellen3Bis8}{quersummes}";
      }
      else
      {
        //String der die Kundennummer darstellt
        nummer = $"KD{stellen3Bis8}{quersumme}";
      }

      Kundennummer kundennummer = kundenummerStore.AddKundennummer(new Kundennummer(Guid.NewGuid(), nummer));
      dataGrid.Items.Refresh();
    }

    /// <summary>
    /// Aktion nach dem CLick des GenerateButton's, welches die Kundennummer hinzufügen Funktion aufruft.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GenerateButton_OnClick(Object sender, RoutedEventArgs e)
    {
      var nummer = String.Empty;
      CreateandAddKundennummer(nummer);
    }

    /// <summary>
    /// Function zum anzeigen der Kundennummern im Data Grid.
    /// </summary>
    private void LoadKundennummerUI()
    {
      listItems = kundenummerStore.KundenNummer;
      dataGrid.ItemsSource = listItems;
    }

    #endregion

    #region Private Fields

    private KundenummerStore kundenummerStore;

    /// <summary>
    /// Oberservablecollection, used to update and show Data in the UI. Used instead of a List!
    /// </summary>
    private ObservableCollection<Kundennummer> listItems = new ObservableCollection<Kundennummer>();

    #endregion
  }
}