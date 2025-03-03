using System;
using System.Collections.Generic;
using System.Linq;

namespace SekizTasBulmaca
{
    class Dugum
    {
        public int[] Durum { get; set; }
        public Dugum UstDugum { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int F => G + H;

        public Dugum(int[] durum, Dugum ustDugum, int g, int h)
        {
            Durum = durum;
            UstDugum = ustDugum;
            G = g;
            H = h;
        }
    }

    class Program
    {
        private static int[] Hedef;

        static void Main()
        {
            Console.WriteLine("Hedef durumunu girin (Boşluk için 0 kullanın):");
            Hedef = DurumAl();

            int[] baslangic = BaslangicDurumuAl();


            bool cozuldu = CozulebilirMi(baslangic);

            if (!cozuldu)
            {
                Console.WriteLine("Bu sıralama çözülemez! Lütfen farklı bir giriş yapın.");
                baslangic = BaslangicDurumuAl();
                cozuldu = CozulebilirMi(baslangic);
            }

            if (cozuldu)
            {
                Coz(baslangic);
            }
        }

        private static int[] DurumAl()
        {
            int[] durum = new int[9];
            for (int i = 0; i < 9; i++)
            {
                Console.Write((i + 1) + ". rakam: ");
                durum[i] = int.Parse(Console.ReadLine());
            }
            return durum;
        }

        private static int[] BaslangicDurumuAl()
        {
            Console.WriteLine("Başlangıç durumunu girin (Boşluk için 0 kullanın):");
            return DurumAl();
        }

        private static bool CozulebilirMi(int[] durum)
        {
            int inversiyon = durum.Where(x => x != 0)
                                  .SelectMany((x, i) => durum.Skip(i + 1)
                                  .Where(y => y != 0 && y < x)).Count();
            return inversiyon % 2 == 0;
        }

        private static void Coz(int[] baslangic)
        {
            var acikListe = new List<Dugum> { new Dugum(baslangic, null, 0, SezgiselHesapla(baslangic)) };
            var kapaliListe = new HashSet<string>();

            while (acikListe.Count > 0)
            {
                var mevcut = acikListe.OrderBy(d => d.F).First();
                acikListe.Remove(mevcut);

                if (mevcut.Durum.SequenceEqual(Hedef))
                {
                    Console.WriteLine("Çözüm bulundu!");
                    CozumuGoster(mevcut);
                    return;
                }

                kapaliListe.Add(string.Join(",", mevcut.Durum));
                var cocuklar = CocuklariBul(mevcut).ToList();

                for (int i = 0; i < cocuklar.Count; i++)
                {
                    var cocuk = cocuklar[i];
                    if (!kapaliListe.Contains(string.Join(",", cocuk.Durum)))
                    {
                        acikListe.Add(cocuk);
                    }
                }
            }
            Console.WriteLine("Çözüm bulunamadı.");
        }

        private static int SezgiselHesapla(int[] durum)
        {
            return durum.Where(x => x != 0)
                        .Sum(x => Math.Abs(Array.IndexOf(Hedef, x) / 3 - Array.IndexOf(durum, x) / 3) +
                                  Math.Abs(Array.IndexOf(Hedef, x) % 3 - Array.IndexOf(durum, x) % 3));
        }

        private static IEnumerable<Dugum> CocuklariBul(Dugum ebeveyn)
        {
            var cocuklar = new List<Dugum>();
            int bosIndex = Array.IndexOf(ebeveyn.Durum, 0);
            int[] hareketler = { -3, 3, -1, 1 };

            for (int i = 0; i < hareketler.Length; i++)
            {
                int hareket = hareketler[i];
                int yeniIndex = bosIndex + hareket;

                if (yeniIndex >= 0 && yeniIndex < 9 && !(bosIndex % 3 == 2 && hareket == 1) && !(bosIndex % 3 == 0 && hareket == -1))
                {
                    var yeniDurum = (int[])ebeveyn.Durum.Clone();
                    yeniDurum[bosIndex] = yeniDurum[yeniIndex];
                    yeniDurum[yeniIndex] = 0;

                    cocuklar.Add(new Dugum(yeniDurum, ebeveyn, ebeveyn.G + 1, SezgiselHesapla(yeniDurum)));
                }
            }

            return cocuklar;
        }

        private static void CozumuGoster(Dugum dugum)
        {
            var yol = new Stack<Dugum>();

            if (dugum != null)
            {

                if (dugum != null)
                {
                    yol.Push(dugum);

                    Dugum currentDugum = dugum.UstDugum;
                    while (currentDugum != null)
                    {
                        yol.Push(currentDugum);
                        currentDugum = currentDugum.UstDugum;
                    }
                }

            }

            if (yol.Count > 0)
            {
                Dugum current = yol.Pop();
                DurumuYazdir(current.Durum);
                Console.WriteLine();

                while (yol.Count > 0)
                {
                    current = yol.Pop();
                    DurumuYazdir(current.Durum);
                    Console.WriteLine();
                }
            }
        }

        private static void DurumuYazdir(int[] durum)
        {
            for (int i = 0; i < 3; i++)
                Console.WriteLine($"{durum[i * 3]} {durum[i * 3 + 1]} {durum[i * 3 + 2]}");
        }
    }
}
