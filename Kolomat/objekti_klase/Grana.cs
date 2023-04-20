using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kolomat
{

    public class grana
    {
        public bool duplikat = false;
        int id;
        tacka uC;
        tacka izC;
        int bri = 0;
        int brv = 0;
        int brr = 0;
        int brc = 0;
        bool zaFormulu = false;
        bool idealanF = false;
        double I = 0;

        public double Struja()
        {
            return I;
        }//Vraca I
        public void strujaPGrane()
        {
            if (bri > 0)
            {
                I = uC.krozGranuStruja(this, uC, bri);
            }
        }//struja u grani sa idealnim strujnim generatorima
        public int proveraKraja(tacka kontrolna)
        {
            int x = 0;

            if (kontrolna == izC) x = 1;

            return x;
        }//vraca 1 za kraj grane

        public grana(tacka ulaz, komponenta prva, int x)
        {
            id = x;
            uC = ulaz;
            int provera = 0;
            komponenta priv = prva;
            tacka pristupna = uC;


            while (provera >= 0)
            {
                priv.dodjelaGrane(this);

                provera = priv.ProlazKrozGranu1(ref izC, ref pristupna);

                switch (provera)
                {
                    case 1:
                        brv++;
                        break;
                    case 2:
                        bri++;
                        break;
                    case 3:
                        brr++;
                        break;
                    case 4:
                        brc++;
                        break;
                    case 1 - 5:
                        brv++;
                        break;
                    case 2 - 5:
                        bri++;
                        break;
                    case 3 - 5:
                        brr++;
                        break;
                    case 4 - 5:
                        brc++;
                        break;

                } // broji komponente u zavisnosti od povratne vrednosti

                if (provera >= 0)
                {
                    priv = pristupna.vratiKomponentu(priv);
                }

            }

            if (brc == 0 && bri == 0 && brr == 0 && brv > 0) idealanF = true;
            if (brc == 0 && bri == 0 && brr > 0) zaFormulu = true;


        }//generise granu

        public void proveraDuplikata(grana druga, komponenta[] komponente)
        {
            if (!duplikat)
            {
                if (uC == druga.izC && izC == druga.uC && !druga.duplikat && bri == druga.bri && brv == druga.brv
                    && brr == druga.brr && brc == druga.brc)
                {
                    duplikat = true;
                    for (int i = 0; i < komponente.Length; i++)
                    {
                        komponente[i].proveraPromene(this, druga);
                    }
                }
            }
        }//ako je poredjena grana ista kao i posmatran posmatrana se proglasava duplikatom

        public void ispisiGranu()
        {
            Console.WriteLine(" Grana {0}. Iz cvora {1} u cvor {2} \n" +
                "U grani se nalzi : {3} Naponskih generatora\n" +
                "                   {4} Strujnih generatora\n" +
                "                   {5} Otpornika\n" +
                "                   {6} Kondezatora", id, uC.id, izC.id, brv, bri, brr, brc);
            if (zaFormulu) Console.WriteLine("Za datu granu je moguce pisati formulu");
            if (idealanF) Console.WriteLine("Radi se o idealnoj grani sa samo naponskim generatorima");
            Console.WriteLine("----------------------------------------------------------------------");
        }// test funkcija za ispis grane sa osnovnim informacijama o istoj

        public void refakturisiID(int x)
        {
            id = x;
        }//menja id komponente nakon sto je doslo do sredjivanja liste elemenata

        public int GranaRefC(ref int id, int m)
        {
            int x = 0;

            if (idealanF)
            {
                x = 1;
                uC.SetReferent();
                id = m;

              //  int Napon = 0;
              //  tacka pristupna = uC;

               // for (int i = 0; i < brv; i++)
             //   {
                    izC.SetPoznati(uC, this, 0);
             //   }

            }

            return x;
        }//proverava da li je grana idealna naponska
         //ako jeste setuje odgovarajuci referentni i poznati cvor

        public int akoIdealna()
        {
            int x = 0;

            if (idealanF) x = 1;

            return x;
        }//vraca keca ako je idealna grana

        public string formula1(tacka kontrolna, ref int x)
        {
            string a = "";

            if (uC == kontrolna)
            {
                if (x != 0) a = a + " + ";
                x++;
                a = a + "I" + id;
            }
            if (izC == kontrolna)
            {
                a = a + " - ";
                x++;
                a = a + "I" + id;
            }

            return a;
        }// u formulu tipa 1 dodaje oznaku za datu granu  na formulu koja se pise

        public string formula2(tacka kontrolna, ref int x) // za formulu po MPC za odredjenicvor svakoj njegovoj grani
                                                           // generise izraz za datu formulu
        {
            string a = "";

            if (uC == kontrolna)
            {
                if (x != 0) a = a + " + ";
                x++;
                if (zaFormulu)
                {
                    a = a + "( V" + uC.id + " - V" + izC.id + " ";
                    a = a + uC.krozGranuF2(this, uC, brv);
                }
                else
                {
                    if (brc > 0)
                    {
                        a = a + "0";
                    }
                    else
                    {
                        a = a + "I" + id;
                    }
                }

            }
            if (izC == kontrolna)
            {
                a = a + " - ";
                x++;
                if (zaFormulu)
                {
                    a = a + "( V" + uC.id + " - V" + izC.id + " ";
                    a = a + uC.krozGranuF2(this, uC, brv);
                }
                else
                {
                    if (brc > 0)
                    {
                        a = a + "0";
                    }
                    else
                    {
                        a = a + "I" + id;
                    }
                }

            }

            return a;
        }
        public string formula3(tacka kontrolna, ref int x)
        {
            string a = "";

            if (uC == kontrolna)
            {

                x++;
                if (zaFormulu)
                {
                    if (x != 0) a = a + " + ";
                    a = a + "(";

                    if (uC.jePoznat() == 1)
                    {
                        a = a + " " + uC.vratiPodatak(1) + " - ";
                    }
                    else
                    {
                        a = a + " V" + uC.id;
                        if (izC.vratiPodatak(1) >= 0)
                        {
                            a = a + " - ";
                        }
                        else
                        {
                            a = a + " + ";
                        }
                    }

                    if (izC.jePoznat() == 1)
                    {
                        a = a + Math.Abs(izC.vratiPodatak(1));

                    }
                    else
                    {
                        a = a + "V" + izC.id;
                    }

                    a += " ";

                    a = a + uC.krozGranuF3(this, uC, brv);
                }
                else
                {
                    if (brc > 0)
                    {
                        a = a + " + 0";
                    }
                    else
                    {
                        if (I > 0) a = a + " + " + I + "";
                        if (I < 0) a = a + " - " + Math.Abs(I) + "";
                    }
                }

            }
            if (izC == kontrolna)
            {

                x++;

                if (zaFormulu)
                {
                    a = a + " - ";
                    a = a + "(";

                    if (uC.jePoznat() == 1)
                    {
                        a = a + " " + uC.vratiPodatak(1) + " - ";
                    }
                    else
                    {
                        a = a + " V" + uC.id;
                        if (izC.vratiPodatak(1) >= 0)
                        {
                            a = a + " - ";
                        }
                        else
                        {
                            a = a + " + ";
                        }
                    }

                    if (izC.jePoznat() == 1)
                    {
                        a = a + " " + Math.Abs(izC.vratiPodatak(1));
                    }
                    else
                    {
                        a = a + "V" + izC.id;
                    }

                    a += " ";

                    a = a + uC.krozGranuF3(this, uC, brv);
                }
                else
                {
                    if (brc > 0)
                    {
                        a = a + " - 0";
                    }
                    else
                    {
                        if (I > 0) a = a + " - " + I + "";
                        if (I < 0) a = a + " + " + Math.Abs(I) + "";
                    }
                }
            }

            return a;
        } //u formulama se nalaze brojevne vrednosti

        public void IdealnaPolu()// setuje potencijal za odgovarajuci cvor , ako je jedan poznat a drugi ne a grana je idealna naponska
        {

            double V = 0;

            if ((uC.jePoznat() == 1 && izC.jePoznat() != 1) || (uC.jePoznat() != 1 && izC.jePoznat() == 1))
            {
                if (this.akoIdealna() == 1)
                {
                    if (uC.jePoznat() == 1)
                    {
                        V = uC.vratiPodatak(1);
                        izC.SetPoznati(uC, this, V);

                    }
                    else
                    {
                        V = izC.vratiPodatak(1);
                        uC.SetPoznati(uC, this, V);
                    }
                }
            }

        }

        public void StrujaGrane()
        {
            I = 0;

            if (bri > 0)
            {

            }
            else
            {
                if (brc > 0)
                {
                    I = 0;
                }
                else
                {
                    double U = 0;
                    double R = 0;

                    U = uC.vratiPodatak(1) - izC.vratiPodatak(1);

                    U += uC.krozGranuNapon(this, uC);
                    R += uC.krozGranuOtpor(this, uC);

                    I = U / R;


                }

            }
            if (brc == 0 && bri == 0 && brr == 0 && brv > 0)
            {
                I = 0;
            }
        }//odredjuje struju u grani za sve grane osim za onu sa naponskim generatorima

        public void StrujaNaponskeGrane()
        {

            if (brc == 0 && bri == 0 && brr == 0 && brv > 0)
            {

                I = 0 - uC.StrujeTacka();

            }

        }// odredjuje jacinu struje u grani sa samo naponskim generatorima

        public int CvorTest(tacka provera)
        {
            int x = 0;

            if (provera == uC) x = 1;
            if (provera == izC) x = -1;

            return x;
        } // vraca 1 ako je provera == uC a -1 == izC , vraca 0 ako nema poklapanja

        public void IspisStruje() // nakonzoli ispisuje struju u grani uz osnovne podatke o grani
        {
            double a = I;
            if (I > 0.05)
            {
                Console.WriteLine("Jacina struje u grani {0} iznosi {1:0.000} A \n" +
                                  " Grana iz cvora {2} u cvor {3} \n", id, a, uC.id, izC.id);
            }
            else
            {
                a = a * 1000;
                Console.WriteLine("Jacina struje u grani {0} iznosi {1:0.000} mA \n" +
                                 " Grana iz cvora {2} u cvor {3} \n", id, a, uC.id, izC.id);
            }

        }

        public void naponKrozGranu()//setuje potencijale tacaka izmedju dva cvora dok prolazi kroz granu
        {
            if (bri == 0 && brc == 0)
            {
                double V = 0;
                tacka privremena = uC;
                komponenta trenutna = null;
                privremena.predjiPrekoKomponente(ref privremena, ref trenutna, this,ref V);
                while (privremena != izC)
                {

                    privremena.setovanjeTacaka(V, trenutna.odrediNapon(privremena),I);

                    privremena.predjiPrekoKomponente(ref privremena, ref trenutna, this, ref V);
                }
            }
            if (brc > 0)
            {
                double V = 0;
                tacka privremena = uC;
                komponenta trenutna = null;
                privremena.predjiPrekoKomponente(ref privremena, ref trenutna, this, ref V);
                while (privremena == izC)
                {
                    if (trenutna.proveraTipa("C") == 1) 
                    { 
                        V = izC.vratiPodatak(1); 
                    }

                    privremena.setovanjeTacaka(V, 0,0);

                    privremena.predjiPrekoKomponente(ref privremena, ref trenutna, this, ref V);
                }
            }
            if (bri > 0)
            {
                
                double V = 0;
                tacka privremena = uC;
                komponenta trenutna = null;
                privremena.predjiPrekoKomponente(ref privremena, ref trenutna, this, ref V);
                while (true)
                {
                    
                    if (trenutna.proveraTipa("IS") == 1)
                    {
                        break;
                    }
                    privremena.setovanjeTacaka(V, trenutna.odrediNapon(privremena), I);

                    


                    privremena.predjiPrekoKomponente(ref privremena, ref trenutna, this, ref V);
                }



                V = 0;
                privremena = izC;
                trenutna = null;
                privremena.predjiPrekoKomponente(ref privremena, ref trenutna, this, ref V);
                while (true)
                {
                    if (trenutna.proveraTipa("IS") == 1)
                    {
                        break;
                    }
                    privremena.setovanjeTacaka(V, -trenutna.odrediNapon(privremena), I);

                    privremena.predjiPrekoKomponente(ref privremena, ref trenutna, this, ref V);
                }
            }
        }


    }
}
