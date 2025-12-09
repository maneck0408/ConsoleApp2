using System;
using ClassLibrary1;   // ← la librairie

var game = new YamsGame();
game.Jouer();

Console.WriteLine("\nAppuie sur Entrée pour quitter.");
Console.ReadLine();