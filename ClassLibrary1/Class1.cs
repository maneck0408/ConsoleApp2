using System;

namespace ClassLibrary1;

public class YamsGame
{
    Random r = new();

    string[] cat =
    {
        "1", "2", "3", "4", "5", "6",
        "Brelan", "Carré", "Full", "Petite suite",
        "Grande suite", "Yams", "Chance"
    };

    int[] scores = new int[13];
    bool[] used = new bool[13];
    readonly int[] des = new int[5];

    public void Jouer()
    {
        Console.WriteLine("=== Yams (librairie) ===");

        for (int tour = 0; tour < 13; tour++)
        {
            Console.WriteLine($"\n--- Tour {tour + 1}/13 ---");

            // jusqu'à 3 lancers
            for (int l = 1; l <= 3; l++)
            {
                if (l == 1)
                    LancerTous();
                else
                {
                    Console.Write("Relancer ? (o/n) : ");
                    var rep = (Console.ReadLine() ?? "").ToLower();
                    if (rep != "o" && rep != "oui") break;
                    RelancerChoisis();
                }

                Console.WriteLine($"Lancer {l} :");
                AfficherDes();
            }

            Console.WriteLine("\nCatégories :");
            for (int i = 0; i < cat.Length; i++)
                Console.WriteLine($"{i + 1}. {cat[i]} {(used[i] ? "(utilisée)" : "")}");

            int c = DemanderCategorie();
            int s = CalculerScore(c);
            scores[c] = s;
            used[c] = true;
            Console.WriteLine($"→ {s} points dans {cat[c]}");
        }

        AfficherResultat();
    }
    void LancerTous()
    {
        for (int i = 0; i < 5; i++)
            des[i] = r.Next(1, 7);
    }

    void RelancerChoisis()
    {
        Console.WriteLine("Numéros des dés à relancer (1..5) séparés par des espaces :");
        string[] s = (Console.ReadLine() ?? "")
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        bool[] rel = new bool[5];
        foreach (var x in s)
            if (int.TryParse(x, out int n) && n >= 1 && n <= 5)
                rel[n - 1] = true;

        for (int i = 0; i < 5; i++)
            if (rel[i]) des[i] = r.Next(1, 7);
    }

    void AfficherDes()
    {
        for (int i = 0; i < 5; i++)
            Console.WriteLine($"Dé {i + 1} : {des[i]}");
    }

    int DemanderCategorie()
    {
        while (true)
        {
            Console.Write("Catégorie (numéro) : ");
            if (int.TryParse(Console.ReadLine(), out int n))
            {
                n--;
                if (n >= 0 && n < used.Length && !used[n])
                    return n;
            }
            Console.WriteLine("Choix invalide.");
        }
    }

    int CalculerScore(int c)
    {
        int[] cnt = new int[7];
        int sum = 0;
        foreach (int x in des)
        {
            cnt[x]++;
            sum += x;
        }

        // 0..5 : total des 1..6
        if (c is >= 0 and <= 5)
            return cnt[c + 1] * (c + 1);

        if (c == 6) // Brelan
        {
            for (int i = 1; i <= 6; i++)
                if (cnt[i] >= 3) return sum;
            return 0;
        }

        if (c == 7) // Carré
        {
            for (int i = 1; i <= 6; i++)
                if (cnt[i] >= 4) return sum;
            return 0;
        }

        if (c == 8) // Full
        {
            bool a3 = false, a2 = false;
            for (int i = 1; i <= 6; i++)
            {
                if (cnt[i] == 3) a3 = true;
                if (cnt[i] == 2) a2 = true;
            }
            return a3 && a2 ? 25 : 0;
        }

        if (c == 9) // Petite suite
        {
            bool s1 = cnt[1] > 0 && cnt[2] > 0 && cnt[3] > 0 && cnt[4] > 0;
            bool s2 = cnt[2] > 0 && cnt[3] > 0 && cnt[4] > 0 && cnt[5] > 0;
            bool s3 = cnt[3] > 0 && cnt[4] > 0 && cnt[5] > 0 && cnt[6] > 0;
            return s1 || s2 || s3 ? 30 : 0;
        }

        if (c == 10) // Grande suite
        {
            bool s1 = cnt[1] > 0 && cnt[2] > 0 && cnt[3] > 0 && cnt[4] > 0 && cnt[5] > 0;
            bool s2 = cnt[2] > 0 && cnt[3] > 0 && cnt[4] > 0 && cnt[5] > 0 && cnt[6] > 0;
            return s1 || s2 ? 40 : 0;
        }

        if (c == 11) // Yams
        {
            for (int i = 1; i <= 6; i++)
                if (cnt[i] == 5) return 50;
            return 0;
        }

        if (c == 12) // Chance
            return sum;

        return 0;
    }

    void AfficherResultat()
    {
        int total = 0;
        Console.WriteLine("\n--- Résumé ---");
        for (int i = 0; i < cat.Length; i++)
        {
            Console.WriteLine($"{cat[i]} : {scores[i]}");
            total += scores[i];
        }

        Console.WriteLine($"\nSCORE FINAL : {total}");
    }
}