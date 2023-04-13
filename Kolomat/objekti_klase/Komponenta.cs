using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kolomat
{

    public class komponenta
    {
        int id;
        String tip = "";
        int n;
        int a;
        tacka ak = null;
        int b;
        tacka bk = null;
        double I;
        double U;
        double P;
        grana pripadajuca = null;

        double vrednost = -1;

        public double vratPodatak(int tst) 
        {
            double x = 0;

            switch (tst)
            {
                case 1: x = U; break;
                    case 3: x = P; break;
                    case 2: x = I; break;
            }

            return x;
        }// 1 - U , 2 - I , 3 - P
        public double vratiOtporR(ref tacka privremena)
        {
            double x = 0;

            if (tip.CompareTo("R") == 0)
            {
                if (bk == privremena) x = vrednost;
                if (ak == privremena) x = vrednost;
            }

            if (privremena == ak)
            {
                privremena = bk;
            }
            else
            {
                privremena = ak;
            }



            return x;
        }//vraca otpor za otpornik i azurira prolaz kroz granu
        public double vratiNaponE(ref tacka privremena) 
        {
            double x = 0;

            if (tip.CompareTo("E") == 0)
            {
                if (bk == privremena) x = vrednost;
                if (ak == privremena) x = -vrednost;
            }

            if (privremena == ak)
            {
                privremena = bk;
            }
            else
            {
                privremena = ak;
            }



            return x;
        }//vraca napon za elektricni generator i azurira prolaz kroz granu
        public double vratiStrujuIS(ref tacka privremena)
        {
            double x = 0;

            if (tip.CompareTo("IS") == 0)
            {
                if (bk == privremena) x = vrednost;
                if (ak == privremena) x = -vrednost;
            }

            if (privremena == ak)
            {
                privremena = bk;
            }
            else
            {
                privremena = ak;
            }



            return x;
        }//vraca struju za strujni generator i azurira prolaz kroz granu
        public void proveraPromene(grana stara, grana nova)
        {
            if (pripadajuca == stara) pripadajuca = nova;
        }//prilikom brisanja viska grana azurira vezu grana sa komponentama
        public void dodjelaGrane(grana x)
        {
            pripadajuca = x;
        }// dodjeljuje komponenti granu kojoj pripada
        public komponenta(string x, int br)
        {
            id = br;
            string[] par = x.Split(' ');
            string priv = "";
            int i = 1;

            foreach (char s in par[0]) 
            {

                if (Char.IsLetter(s))
                {
                    tip += s;
                }
                if (Char.IsDigit(s))
                {
                    priv += s;
                }

            }

            if (!(int.TryParse(priv,out n)))
            {
                n = 0;
            }




            a = int.Parse(par[1]);
            b = int.Parse(par[2]);

            if (tip.CompareTo("W") != 0) vrednost = double.Parse(par[3]);
            if (tip.CompareTo("E") == 0) U = vrednost;


        } //konstruktor metoda prima string ucitan iz txt fajla i rasclanjuje ga vadeci revantne podatke
        public void test1()
        {
            Console.WriteLine("Komponenta {0} je tipa {1} ({4}) \n" +
                " Spojena na krajeve {2} i {3} ", id, tip, a, b, n);
            if (vrednost > -1) Console.WriteLine("Vrednost komponente je : {0}\n\n", vrednost);
        }//metoda za svrhu testiranja , ispis sadrzaja objekta

        public int uporedi(komponenta druga)
        {
            int prov = 0;

            if (tip.CompareTo(druga.tip) == 0)
            {
                if (n == druga.n)
                {
                    if ((a == druga.a) && (b == druga.b) && (vrednost == druga.vrednost))
                    {
                        prov = 1;
                    }
                }
            }

            return prov;
        } // vraca 1 ako su dve komponente iste / 0 ako su razlicite

        public void uvzivanje(ref tacka[] tacke, ref int nT)
        {
            int test = 0;
            int adr = 0;
            tacka[] priv = new tacka[nT + 2];

            for (int i = 0; i < nT; i++) priv[i] = tacke[i];

            for (int i = 0; i < nT; i++)
            {
                if (priv[i].provera(a) == 1)
                {
                    test = 1;
                    adr = i;
                }
            }
            if (test == 0)
            {
                priv[nT] = new tacka(a, this);
                nT++;
                ak = priv[nT - 1];
            }
            else
            {
                priv[adr].dodajKomponentu(this);
                ak = priv[adr];
            }
            test = 0;
            adr = 0;

            for (int i = 0; i < nT; i++)
            {
                if (priv[i].provera(b) == 1)
                {
                    test = 1;
                    adr = i;
                }
            }
            if (test == 0)
            {
                priv[nT] = new tacka(b, this);
                nT++;
                bk = priv[nT - 1];
            }
            else
            {
                priv[adr].dodajKomponentu(this);
                bk = priv[adr];
            }

            tacke = new tacka[nT];
            for (int i = 0; i < nT; i++) tacke[i] = priv[i];

        } // dati cvor uveze sa tackama ako iste ne postoje kreira ih

        public void izbacivanjeVisaka(ref komponenta[] clanovi) // ako je data komponenta tipa zica (W) spaoaja ta dva cvora
        {
            if (tip.CompareTo("W") == 0)
            {
                ak.spojiSa(bk);
                bk.ukloniVezu(ak);
            }
        }

        public void promenaVeze(tacka nova, tacka stara) // azurira veze date komponente sve stare veze zamjeni novim
        {
            if (ak == stara)
            {
                ak = nova;
                a = nova.id;
            }
            if (bk == stara)
            {
                bk = nova;
                b = nova.id;
            }

        }

        public int proveraVeza(tacka a, tacka b)
        {
            int x = 0;

            if ((a == ak || a == bk) && (b == bk || b == ak)) x = 1;

            return x;
        } //vraca 1 ako data komponenta ima te veze

        public int proKratko()
        {
            int x = 0;

            if (ak == bk && tip.CompareTo("W") != 0) x = 1;
            return x;
        } //vraca 1 ako je u kratkom spoju ,neispravno unjeta komponenta , inace 0

        public int ProlazKrozGranu1(ref tacka izC, ref tacka pristupna)//podesava informaciju o izlaznom cvoru u slucaju da je izlaz iz
                                                                       // komponente cvor u suprotnom vraca null toj komponent
                                                                       // vraca pristupnu tacku za sledeci poziv. Direktno vraca 
                                                                       // numericku vrednost u zavisnosti od toga na sta je naisla
                                                                       // 1 eg , 2 is , 3 r ,4 c a ako je to kraj grane datu vrednost umanji za
                                                                       // 5 pa je u tom slucaju broj manji od 0 
        {
            int x = 0;
            izC = null;
            int us1 = 0;

            if ((bk == pristupna && ak.CvorProvera() == 1) || (ak == pristupna && bk.CvorProvera() == 1))
            {
                x = -5;
                if (ak == pristupna) izC = bk;
                if (bk == pristupna) izC = ak;

            }


            if (tip.CompareTo("E") == 0) x += 1;
            if (tip.CompareTo("IS") == 0) x += 2;
            if (tip.CompareTo("R") == 0) x += 3;
            if (tip.CompareTo("C") == 0) x += 4;




            if (ak == pristupna && us1 == 0)
            {
                pristupna = bk;
                us1 = 1;
            }
            if (bk == pristupna && us1 == 0)
            {
                pristupna = ak;
                us1 = 1;
            }

            return x;
        }

        public double NaponIdealGrana(ref tacka pristupna, grana ispitivan) // dodaje napon datog generatora u jednacinu za
                                                                            // racunaje potencijala cvora poznatog unapred
                                                                            // napon na idealnoj naponskoj grani
        {
            double x = 0;

            if (ispitivan == pripadajuca)
            {

                if (pristupna == ak && x == 0)
                {
                    x = -U;
                    pristupna = bk;
                }
                if (pristupna == bk && x == 0)
                {
                    x = U;
                    pristupna = ak;
                }

            }

            return x;
        }

        public int proveraPripadnostiGrani(grana test)//vraca 1 ako je komponenta u datoj grani
        {
            int x = 0;

            if (test == pripadajuca) x = 1;

            return x;
        }

        public string dodavanjeNaponaF2(ref tacka privremena)
        {
            string a = "";

            if (tip.CompareTo("E") == 0)
            {

                if (ak == privremena)
                {

                    a = a + "- E" + n + " ";
                }
                else if (bk == privremena)
                {

                    a = a + "+ E" + n + " ";
                }
            }
            if (ak == privremena)
            {
                privremena = bk;

            }
            else if (bk == privremena)
            {
                privremena = ak;

            }

            return a;
        }// vraca simbolicki zapis datog generatora za granu i azurira seledecu tacku an  koju se nailazi

        public string dodavanjeOtporaF2(ref tacka privremena, ref int pr)
        {
            string a = "";

            if (tip.CompareTo("R") == 0)
            {
                if (pr != 0)
                {
                    a = a + "+";
                }
                else
                {
                    pr++;
                }



                a = a + " R" + n + " ";
            }
            if (ak == privremena)
            {
                privremena = bk;

            }
            else if (bk == privremena)
            {
                privremena = ak;

            }

            return a;
        }// vraca simbolicki zapis datog otpornika za granu i azurira seledecu tacku an  koju se nailazi


        public string dodavanjeNaponaF3(ref tacka privremena)
        {
            string a = "";

            if (tip.CompareTo("E") == 0)
            {

                if (ak == privremena)
                {

                    a = a + "- " + vrednost + " ";
                }
                else if (bk == privremena)
                {

                    a = a + "+ " + vrednost + " ";
                }
            }
            if (ak == privremena)
            {
                privremena = bk;

            }
            else if (bk == privremena)
            {
                privremena = ak;

            }

            return a;
        }// vraca simbolicki zapis datog generatora za granu i azurira seledecu tacku an  koju se nailazi

        public string dodavanjeOtporaF3(ref tacka privremena, ref int pr)
        {
            string a = "";

            if (tip.CompareTo("R") == 0)
            {
                if (pr != 0)
                {
                    a = a + "+";
                }
                else
                {
                    pr++;
                }



                a = a + " " + vrednost + " ";
            }
            if (ak == privremena)
            {
                privremena = bk;

            }
            else if (bk == privremena)
            {
                privremena = ak;

            }

            return a;
        }// vraca simbolicki zapis datog otpornika za granu i azurira seledecu tacku an  koju se nailazi

        public void azurirajStruju()
        {
            if (tip.CompareTo("W") != 0)
            {
                I = pripadajuca.Struja();
            }
        }//azurira struju na komponenti

        public int proveriSmer(tacka cvor)
        {
            int x = 0;

            x = pripadajuca.CvorTest(cvor);

            return x;
        }//vraca 1 ako je smer grane date komponente iz te tacke ako je u tu tacku onda vrati -1
    }

}
