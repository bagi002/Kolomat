﻿using Microsoft.VisualBasic;
using System.Dynamic;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;


// smer struje u programu definise se preko klase grana
// Smjer ide od ulaznog cvora u ranu uC ka izlaznom cvoru iz grane izC.

namespace Kolomat
{
    internal class Program
    {
        static void Main(string[] args)
        {

            komponenta[] element = null;
            int n = 0;
            int error = 0;

            tacka[] tacke = null;
            int nT = 0;

            grana[] grane = null;
            int nG = 0; 




            error = ucitavanjePodataka(ref element, ref n);

            Console.WriteLine(error);
            Console.WriteLine(n);
         
            if (error == 0) testiranje1(n, element);
         

            if(error == 0) error = proveraPon(n, element);
            if(error == 0) error = dodavanjeTacaka(ref nT,ref tacke,n,element);
            if(error == 0) ispisTacakaTest(tacke, nT);
            if(error == 0) uklanjanjeViskaCvorova(ref element,n);
            if (error == 0) ispisTacakaTest(tacke, nT);
            if (error == 0) setovanjeCvorova(tacke, nT);
            if (error == 0) error = proveraKonekcija(tacke, nT);
            if (error == 0) error = proveraElemenata2(element,n);
            if (error == 0) kreiranjeGrana(tacke, nT, ref grane, ref nG,element);
            if (error == 0) ispisGrana(grane, nG);
            if (error == 0) RefPozC(grane, nG,tacke,nT);
            if (error == 0) ispisFormulaMPC(tacke, nT, grane, nG);
            if (error == 0) error = proveraKola1(grane, tacke);
            if (error == 0) odredjivanjeStrujeKola(grane , element);


            if (error == 1) Console.WriteLine("Prekid izvrsavanja usled gore navedene greske !!!!! \n\n\n");
            Console.ReadLine();
        }

        static int proveraPon(int n,komponenta[] element)
        {
            int error = 0;

            for(int i = 0; i < n-1;i++)
            {
                for(int j = i+1; j < n; j++)
                {
                    if (element[i].uporedi(element[j]) == 1)
                    {
                        error = 1;
                    }
                }
            }
            if (error == 0)
            {
                Console.WriteLine("1) Ne postoje dve indenticne komponente u kolu. \n" +
                    " Svaki element se pojavljuje samo jednom ...");
            }
            else
            {
                
                Console.WriteLine("Postoji greska u zadvanju kola , dve komponente sa \n" +
                    "identicnim opisom su zadate vise puta , proverite zadato kolo !!!!");
            }
            return error;
        } // proveri da li postoje dve identicne komponente , vrati poruku i kod greske
        static int ucitavanjePodataka(ref komponenta[] element, ref int n)
        {
            int error = 0;
            if (!File.Exists("Kolo.txt"))
            {
                Console.WriteLine(" Fatalna greska !!!! \n" +
                    " Doslo je do greske prilikom otvaranja fajla \n" +
                    " sa specifikacijama kola \"Kolo.txt\" \n\n");
                error = 1;

            }

            if (error == 0) {


                StreamReader fajl = new StreamReader("Kolo.txt");

                List<string> redovi = new List<string>();
                string priv;

                while(!fajl.EndOfStream) 
                {
                    priv = fajl.ReadLine();
                    redovi.Add(priv);
                }
                n = redovi.Count();
                fajl.Close();

                element = new komponenta[n];

                for (int i = 0; i < n;i++)
                {
                    element[i] = new komponenta(redovi[i],i);
                }

            }

            return error;
        } // Povlaci podatke iz txt fajla i smjesta ih u objekte tipa komponeta
        static void testiranje1(int n, komponenta[] element)
        {
            for (int i = 0;i < n; i++) 
            {
                element[i].test1();
                Console.WriteLine("Ispisan");
            }
        } // testiranje provera da li je sve ucitano
        static int dodavanjeTacaka(ref int nT , ref tacka[] tacke,int n , komponenta[] elementi) // kreira listu tacaka 
        {
            int error = 0;

            

            for(int i = 0;i < n; i++)
            {
                elementi[i].uvzivanje(ref tacke, ref nT);
            }

            Console.WriteLine("Kreiranje liste tacaka je uspjesno obavljeno !!!");

            return error;
        }
        static void ispisTacakaTest(tacka[] tacke, int nT)
        {
            for (int i = 0; i < nT; i++) tacke[i].testIspis();
        }//za testiranje

        static void uklanjanjeViskaCvorova(ref komponenta[] clanovi,int n) // uklanja visak tacaka , tacke vezane zicom
        {
            for(int i = 0; i < n;i++)
            {
                clanovi[i].izbacivanjeVisaka(ref clanovi);

            }
            Console.WriteLine("Svi potencijalno nepotrebni cvorevi su eliminisani iz kola \n" +
                " na nacin da su dva cvora spojena kratkospojnikom povezana u jedan");
        }

        static void setovanjeCvorova(tacka[] tacke,int nT)
        {
            for(int i = 0;i < nT ; i++)
            {
                tacke[i].jeCvor();
            }
        }// tacke sa preko 2 konekcije postaju cvorovi
        static int proveraKonekcija(tacka[] tacke,int nT)
        {

            int error = 0;

            for (int i = 0; i < nT; i++)
            {
               error += tacke[i].povezanost();
            }

            if(error != 0)
            {
                Console.WriteLine("Pronadjena je greska u kolu , postoji ne zatvoren strujni krug !!! \n" +
                    " Dio kola nije pravilno povezan provjerite to");
            }
            else
            {
                Console.WriteLine("Strujno kolo je zatvoreno ne postoje prekidi u kolu");
            }

            return error;

        }//provera da li ima visecih grana , da li je zatvoren strujni krug

        static int proveraElemenata2(komponenta[] element ,int n)
        {
            int error = 0;

            for(int i = 0; i < n;i++)
            {
               error += element[i].proKratko();
              
            }

            if(error != 0)
            {
                Console.WriteLine("Doslo je do greske !!\n" +
                    " Postoji komponenta u kratkom spoju sa obje\n" +
                    " strane spojena na istu tacku.");
            }
            else
            {
                Console.WriteLine("Ne postoje komponente sa neispravnim ukljucenjem u kolo!!");
            }

            return error;
        }//provera komponenata u kratkom spoju vraca 0 ako je sve uredu

        static void kreiranjeGrana(tacka[] tacke, int nT,ref grana[] grane,ref int nG, komponenta[] komponente) 
        {
            nG = 0;
            for(int i = 0; i < nT; i++)
            {
                nG += tacke[i].brVezaCvor();
            }
            nG = nG / 2;
            grana[] privremeni = new grana[nG*2];
            int j = 0;
            for(int i = 0;i < nT; i++)
            {
                tacke[i].generatorGranaZaCvor(ref privremeni, ref j);
            }

            for(int i = 0; i < nG*2-1; i++)
            {
                for(j = i+1; j < nG*2; j++)
                {
                    privremeni[i].proveraDuplikata(privremeni[j],komponente);
                }
            }

            j = 0;

            grane = new grana[nG];

            for(int i = 0;i < nG * 2; i++)
            {
                if (!privremeni[i].duplikat) {
                    grane[j] = privremeni[i];
                    grane[j].refakturisiID(j+1);
                    j++;
                        }
            }
        }//kreira objekte za sve grane u kolu

        static void ispisGrana(grana[] grane,int nG)
        {
            Console.WriteLine("------------- Spisak grana u kolu ----------------------\n" +
                "----------------------------------------------------------");
            for(int i = 0; i < nG; i++)
            {
                grane[i].ispisiGranu();
            }
        }//test fija ispis grana

        static void RefPozC(grana[] grane, int nG, tacka[] tacke, int nT)
        {
            int refID = -1;

           for(int i = 0; i < nG; i++)
            {
                grane[i].GranaRefC(ref  refID,  i );
                
            }

           if(refID < 0)
            {
                for(int i = 0; i< nT ; i++)
                {
                    if (tacke[i].CvorProvera() == 1)
                    {
                        tacke[i].SetReferent();
                        break;
                    }
                }
            }
        }//nalazi referentni cvor ako ima idealna naponska grana setuje
                                                                            //setuje i odgovarajuci idealni naponski cvor

        static void ispisFormulaMPC(tacka[] tacke ,int nT, grana[] grane,int nG) // ispisuje formule po MPC , podesava dodatne
                                                                                 // parametre poznatih grna nakraju i rjesi
                                                                                 // sistem jednacina simbolicki i logicki
        {

            Console.WriteLine("------------------------------------------------------------------\n" +
                              "                Formule resenja kola po metodi \n" +
                              "                   Potencijala cvorova (MPC)               \n" +
                              "----------------------------------------------------------------");
            int brojac = 0;

            int brC = 0;
            int brF = 0;
            for(int i = 0; i < nT ; i++)
            {
                if (tacke[i].CvorProvera() == 1) brC++;
            }

            brF = brC - 1;
            for(int i =0; i< nG ; i++)
            {
                brF = brF - grane[i].akoIdealna();
                grane[i].strujaPGrane();
            }

            string[] formule = new string[brF];

            for(int i = 0; i < nT; i++)
            {
                tacke[i].FormuleDatiOblik1(nG, grane , ref brojac);
                if(brojac == brF)
                {
                    break;
                }
            }
            brojac = 0;
            Console.WriteLine("-------------------------------------------------------------------");
            for (int i = 0; i < nT; i++)
            {
                tacke[i].FormuleDatiOblik2(nG, grane, ref brojac);
                if (brojac == brF)
                {
                    break;
                }
            }

            Console.WriteLine("-------------------------------------------------------------------");

            brojac = 0;
            for (int i = 0; i < nT; i++)
            {
                tacke[i].FormuleDatiOblik3(nG, grane, ref brojac , ref formule[brojac]);
                if (brojac == brF)
                {
                    break;      
                }
            }
            Console.WriteLine("-------------------------------------------------------------------");

            for(int i = 0;i < brF; i++)
            {
                formulaMod(ref formule[i]);
                Console.WriteLine(formule[i]);
            }

            Console.WriteLine("-------------------------------------------------------------------");

            for (int i = 0; i < brF; i++)
            {
                formulaMod2(ref formule[i]);
                Console.WriteLine(formule[i]);
            }

            Console.WriteLine("-------------------------------------------------------------------");

            for (int i = 0; i < brF; i++)
            {
                formulaMod3(ref formule[i]);
                Console.WriteLine(formule[i]);
            }

            Console.WriteLine("-------------------------------------------------------------------");

            for (int i = 0; i < brF; i++)
            {
                formulaMod4(ref formule[i],brF);
                Console.WriteLine(formule[i]);
            }

            Console.WriteLine("-------------------------------------------------------------------");
            resavanjeJednacina(ref  formule ,tacke);

        }

        static void formulaMod(ref string formula)
        {
            string[] razbijenaF = formula.Split(' ');

            bool A;
            bool B;
            double a=0;
            double b=0;
            double c=0;
         

            int konZ = 0;


            for (int i = 0; i < razbijenaF.Length; i++)
            {
              
                if (i+2 < razbijenaF.Length)
                {
                    A = double.TryParse(razbijenaF[i],out a);
                    B = double.TryParse(razbijenaF[i + 2],out b);

                    
                    if (A && B && (razbijenaF[i+1].CompareTo("+") == 0 || razbijenaF[i + 1].CompareTo("-") == 0))
                    {
                        if(razbijenaF[i - 1].CompareTo("-") != 0)
                        {
                            if (razbijenaF[i + 1].CompareTo("+") == 0) c = a + b;
                            if (razbijenaF[i + 1].CompareTo("-") == 0) c = a - b;
                        }
                        else
                        {
                            if (razbijenaF[i + 1].CompareTo("+") == 0) c = -a + b;
                            if (razbijenaF[i + 1].CompareTo("-") == 0) c = -a - b;
                            razbijenaF[i - 1] = "";
                        }
                        
                        razbijenaF[i] = "";
                        razbijenaF[i+1] = "";
                        razbijenaF[i+2] = "";
                        if (c < 0) 
                        {
                            razbijenaF[i - 1] = "-";
                            c = Math.Abs(c);
                        }
                        razbijenaF[i] = razbijenaF[i] + c.ToString();

                    }
                }
            }
            formula = "";
            formula = string.Join(" ", razbijenaF.Where(x => !string.IsNullOrEmpty(x)));//funkcija za izostavljanje praznih stringova izguglana

            razbijenaF = formula.Split(' ');
            c = double.Parse(razbijenaF[razbijenaF.Length - 1]);

            for(int i = 1;i < razbijenaF.Length-1;i++)
            {
                A = double.TryParse(razbijenaF[i], out a);
                if (A && (razbijenaF[i-1].CompareTo("+") == 0 || razbijenaF[i - 1].CompareTo("-") == 0) && (razbijenaF[i + 1].CompareTo("+") == 0 || razbijenaF[i + 1].CompareTo("-") == 0))
                {
                    if (razbijenaF[i - 1].CompareTo("+") == 0)
                    {
                        c = c - a;
                    }
                    if (razbijenaF[i - 1].CompareTo("-") == 0)
                    {
                        c = c + a;
                    }
                    razbijenaF[i] = "";
                    razbijenaF[i - 1] = "";
                } 
                if ( A && (razbijenaF[i - 1].CompareTo("+") == 0 || razbijenaF[i - 1].CompareTo("-") == 0) && razbijenaF[i + 1].CompareTo("=") == 0)
                {
                    if (razbijenaF[i - 1].CompareTo("+") == 0)
                    {
                        c = c - a;
                    }
                    if (razbijenaF[i - 1].CompareTo("-") == 0)
                    {
                        c = c + a;
                    }
                    razbijenaF[i] = "";
                    razbijenaF[i - 1] = "";
                }
            }

            razbijenaF[razbijenaF.Length - 1] = c.ToString();

            formula = "";
            formula = string.Join(" ", razbijenaF.Where(x => !string.IsNullOrEmpty(x)));

            // eliminacija zagraada nepotrebnih

            razbijenaF = formula.Split(' ');
            c = double.Parse(razbijenaF[razbijenaF.Length - 1]);

            for (int i = 1; i < razbijenaF.Length - 1; i++)
            {

                A = double.TryParse(razbijenaF[i],out a);

                if(A && razbijenaF[i-1].CompareTo("(") == 0 && razbijenaF[i + 1].CompareTo(")") == 0)
                {
                    razbijenaF[i - 1] = "";
                    razbijenaF[i + 1] = "";
                }
                
            }

            formula = "";
            formula = string.Join(" ", razbijenaF.Where(x => !string.IsNullOrEmpty(x)));
        }//prvi krug modifikacije formule racunske operacije koje je moguce
                                                     // momentalno odraditi tipa const +- const , eliminacija nepotrebnih zagrad
         

        static void formulaMod2(ref string formula)
        {
            string[] razbijenaF = formula.Split(' ');

            bool A;
            bool B;
            double a = 0;
            double b = 0;
            double c = 0;
            int konZ = 0;

            c = 1;
            a = 1;
            b = 1;
            for (int i = 0; i < razbijenaF.Length; i++)
            {

                if (razbijenaF[i].CompareTo("/") == 0) 
                {
                    a = double.Parse(razbijenaF[i+1]);
                }

                c = (a > b) ? a : b;
                while (true)
                {
                    if(c % a == 0 && c % b == 0)
                    {
                        break;
                    }
                    c++;
                }
                b = c;

            }

            for (int i = 0; i < razbijenaF.Length; i++)
            {

                if (razbijenaF[i].CompareTo("/") == 0)
                {
                    a = double.Parse(razbijenaF[i + 1]);

                    b = c / a;
                    razbijenaF[i] = "*";
                    razbijenaF[i + 1] = b.ToString();
                }

            }

            a = double.Parse(razbijenaF[razbijenaF.Length - 1]);
            b = a * c;
            razbijenaF[razbijenaF.Length - 1] = b.ToString();



            for (int i = 0; i < razbijenaF.Length; i++)
            {

                if (razbijenaF[i].CompareTo("*") == 0)
                {
                    a = double.Parse(razbijenaF[i + 1]);

                    if (a == 1)
                    {
                        razbijenaF[i] = "";
                        razbijenaF[i + 1] = "";
                    }
                   
                }

            }


            formula = "";
            formula = string.Join(" ", razbijenaF.Where(x => !string.IsNullOrEmpty(x)));

            int ida = 0;
            int idb = 0;

            razbijenaF = formula.Split(' ');

            for (int i = 0; i < razbijenaF.Length; i++)
            {
                if (razbijenaF[i].CompareTo("(") == 0)
                {
                    ida = i;
                }
                if (razbijenaF[i].CompareTo(")") == 0)
                {
                    idb = i;
                    if (razbijenaF[i + 1].CompareTo("*") == 0)
                    {
                        a = double.Parse(razbijenaF[i + 2]);
                        for (int j = ida+1;j < idb; j++)
                        {
                            B = double.TryParse(razbijenaF[j], out b);
                            if (!(razbijenaF[j].CompareTo("+") == 0 || razbijenaF[j].CompareTo("-") == 0))
                            {
                                if (B)
                                {
                                    c = a * b;
                                    razbijenaF[j] = c.ToString();
                                }
                                else
                                {
                                    razbijenaF[j] = a.ToString() + " * " + razbijenaF[j];
                                }
                            }
                        }

                        razbijenaF[i + 1] = "";
                        razbijenaF[i + 2] = "";

                    }
                }
               

            }


            formula = "";
            formula = string.Join(" ", razbijenaF.Where(x => !string.IsNullOrEmpty(x)));

        }// mnozenje formule sa NZS da bi se eliminisali razlomci

        static void formulaMod3(ref string formula)
        {
            string[] razbijenaF = formula.Split(' ');

            bool A;
            bool B;
            bool M = false;
            double a = 0;
            double b = 0;
            double c = 0;
            int konZ = 0;

            int ida = 0;
            int idb = 0;

            c = 1;
            a = 1;
            b = 1;
            for (int i = 0; i < razbijenaF.Length; i++)
            { 
                if (razbijenaF[i].CompareTo("(") == 0)
                {
                    ida = i;
                    if (i != 0)
                    {
                        M = razbijenaF[i-1].CompareTo("-") == 0;
                     
                    }
                }
                if (razbijenaF[i].CompareTo(")") == 0)
                {
                    idb = i;
                    if (M)
                    {
                        for (int j = ida;j < idb; j++)
                        {
                            if (razbijenaF[j].CompareTo("+") == 0)
                            {
                                razbijenaF[j] = "-";
                            }else if (razbijenaF[j].CompareTo("-") == 0)
                            {
                                razbijenaF[j] = "+";
                            }
                            

                        }
                    }
                }
            }

            for(int i = 0; i < razbijenaF.Length; i++)
            {
                if (razbijenaF[i].CompareTo("(") == 0 || razbijenaF[i].CompareTo(")") == 0)
                {
                    razbijenaF[i] = "";
                }
            }


            formula = "";
            formula = string.Join(" ", razbijenaF.Where(x => !string.IsNullOrEmpty(x)));


        }//oslobadjanje zagrada

        static void formulaMod4(ref string formula,int brF)
        {
            
            string[] razbijenaF = formula.Split(' ');

            bool A;
            bool B;
            bool M = false;
            double a = 0;
            double b = 0;
            double c = 0;
            int konZ = 0;

            int ida = 0;
            int idb = 0;

            c = 1;
            a = 1;
            b = 1;

            c = 0;

            for (int i = 1; i < razbijenaF.Length; i++)
            {
                if (i != razbijenaF.Length - 1)
                {
                    A = double.TryParse(razbijenaF[i], out a);
                    if (A && (razbijenaF[i - 1].CompareTo("+") == 0 || razbijenaF[i - 1].CompareTo("-") == 0)
                        && (razbijenaF[i + 1].CompareTo("+") == 0 || razbijenaF[i + 1].CompareTo("-") == 0))
                    {
                        razbijenaF[i] = "";
                        if (razbijenaF[i - 1].CompareTo("-") == 0)
                        {
                            c = c + a;
                        }
                        else
                        {
                            c = c - a;
                        }
                        razbijenaF[i - 1] = "";
                    }
                    if (A && (razbijenaF[i - 1].CompareTo("+") == 0 || razbijenaF[i - 1].CompareTo("-") == 0)
                        && (razbijenaF[i + 1].CompareTo("=") == 0))
                    {
                        razbijenaF[i] = "";
                        if (razbijenaF[i - 1].CompareTo("-") == 0)
                        {
                            c = c + a;
                        }
                        else
                        {
                            c = c - a;
                        }
                        razbijenaF[i - 1] = "";
                    }

                }
                else
                {
                    A = double.TryParse(razbijenaF[i], out a);
                    if (A && (razbijenaF[i - 1].CompareTo("+") == 0 || razbijenaF[i - 1].CompareTo("-") == 0))
                    {
                        razbijenaF[i] = "";
                        if (razbijenaF[i - 1].CompareTo("-") == 0)
                        {
                            c = c + a;
                        }
                        else
                        {
                            c = c - a;
                        }
                        razbijenaF[i - 1] = "";
                    }
                }
            }

            c = c + double.Parse(razbijenaF[razbijenaF.Length - 1]);   
            razbijenaF[razbijenaF.Length - 1] = c.ToString();

            formula = "";
            formula = string.Join(" ", razbijenaF.Where(x => !string.IsNullOrEmpty(x)));


           razbijenaF = formula.Split(' ');


            string privremeni;
            string[] promenjive = formula.Split(' ');
            promenjive[promenjive.Length - 1] = "";
            promenjive[promenjive.Length - 2] = "";

            for (int i = 0; i < promenjive.Length-2; i++)
            {
                A = double.TryParse(promenjive[i], out a);
                if (A || promenjive[i].CompareTo("+") == 0 || promenjive[i].CompareTo("-") == 0 || promenjive[i].CompareTo("*") == 0)
                {
                    promenjive[i] = "";
                }
            }

            int polozaj = 0;

            List<string> lsStringova = promenjive.Distinct().ToList();
            promenjive = lsStringova.ToArray();


            Array.Sort(promenjive, (x, y) => String.Compare(x, y, StringComparison.Ordinal)); //izguglana fija da ne radim rucno sortira niz


            formula = "0 + " + formula;
            razbijenaF = formula.Split(' ');

            privremeni = "";
            privremeni = string.Join(" ", promenjive.Where(x => !string.IsNullOrEmpty(x)));

            promenjive = privremeni.Split(' ');

            double[] koeficijenti = new double[promenjive.Length];

            for (int j = 0; j < promenjive.Length; j++)
            {
                koeficijenti[j] = 0;
            }

            for (int i = 0;i < razbijenaF.Length-2; i++)
            {
                A = double.TryParse(razbijenaF[i], out a);
                if (A || razbijenaF[i].CompareTo("+") == 0 || razbijenaF[i].CompareTo("-") == 0 || razbijenaF[i].CompareTo("*") == 0)
                {

                }
                else
                {
                    for(int j = 0; j < promenjive.Length; j++)
                    {
                        
                        if (razbijenaF[i].CompareTo(promenjive[j]) == 0) polozaj = j;
                    }

                    if (razbijenaF[i-1].CompareTo("*") == 0)
                    {
                        b = double.Parse(razbijenaF[i - 2]);

                        if (razbijenaF[i - 3].CompareTo("+") == 0)
                        {
                            koeficijenti[polozaj] += b;
                        }
                        if (razbijenaF[i - 3].CompareTo("-") == 0)
                        {
                            koeficijenti[polozaj] -= b;
                        }
                    }
                    else
                    {
                        if (razbijenaF[i - 1].CompareTo("+") == 0)
                        {
                            koeficijenti[polozaj]++;
                        }
                        if (razbijenaF[i - 1].CompareTo("-") == 0)
                        {
                            koeficijenti[polozaj]--;
                        }
                    }
                }
            }
            formula = "";
            for (int i = 0; i < koeficijenti.Length; i++)
            {
                if (koeficijenti[i] >= 0)
                {
                    if (formula.CompareTo("") != 0)
                    {
                        formula = formula + "+ ";
                    }
                    else
                    {
                        formula = formula + "+ ";
                    }
                    formula = formula + koeficijenti[i].ToString() + " * " + promenjive[i] + " ";
                }
                else
                {
                    formula = formula +"- " + Math.Abs(koeficijenti[i]).ToString() + " * " + promenjive[i] + " ";
                }
            }

            formula = formula + "= " + razbijenaF[razbijenaF.Length-1];

        }//zavrsno sredjivanje jednacina u oblik pogodan za rjesavanje

        static void resavanjeJednacina(ref string[] formule, tacka[] tacke) 
        {
            string privremeni = " ";
            for(int i = 0;i < formule.Length;i++)
            {
                privremeni = " " + formule[i];
            }

            string[] promenjive = privremeni.Split(" ");
            double a = 0;
            bool A = false;
            for (int i = 0; i < promenjive.Length - 2; i++)
            {
                A = double.TryParse(promenjive[i], out a);
                if (A || promenjive[i].CompareTo("+") == 0 || promenjive[i].CompareTo("-") == 0 || promenjive[i].CompareTo("*") == 0)
                {
                    promenjive[i] = "";
                }
            }
            promenjive[promenjive.Length - 1] = "";
            promenjive[promenjive.Length - 2] = "";

            List<string> lsStringova = promenjive.Distinct().ToList();
            promenjive = lsStringova.ToArray();


            Array.Sort(promenjive, (x, y) => String.Compare(x, y, StringComparison.Ordinal)); //izguglana fija da ne radim rucno sortira niz

            double[,] koeifcijenti = new double[formule.Length, promenjive.Length-1]; 
            double[] jednako = new double[formule.Length];

            string[] formuleR;

            for(int i = 0; i < formule.Length; i++)
            {
                formuleR = formule[i].Split(" ");

                for(int j = 3;j <  formuleR.Length;j+=4)
                {
                    for(int k = 1; k < promenjive.Length; k++)
                    {
                        if (promenjive[k].CompareTo(formuleR[j]) == 0)
                        {

                            if (formuleR[j - 3].CompareTo("+") == 0)
                            {
                                koeifcijenti[i,k-1] = double.Parse(formuleR[j-2]);
                            }
                            if (formuleR[j - 3].CompareTo("-") == 0)
                            {
                                koeifcijenti[i, k - 1] = - double.Parse(formuleR[j - 2]);
                            }

                        }
                    }
                }

                jednako[i] = double.Parse(formuleR[formuleR.Length-1]);
            }

           
            for (int i = 0; i < formule.Length - 1; i++)
            {
                for (int j = i + 1; j < formule.Length; j++)
                {
                    double factor = koeifcijenti[j, i] / koeifcijenti[i, i];
                    for (int k = i + 1; k < formule.Length; k++)
                    {
                        koeifcijenti[j, k] -= factor * koeifcijenti[i, k];
                    }
                    jednako[j] -= factor * jednako[i];
                }
            }

            double[] x = new double[formule.Length];
            for (int i = formule.Length - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < formule.Length; j++)
                {
                    sum += koeifcijenti[i, j] * x[j];
                }
                x[i] = (jednako[i] - sum) / koeifcijenti[i, i];
            }

            for(int i = 0; i < formule.Length;i++)
            {
                Console.WriteLine(promenjive[i+1] + " = " + x[i].ToString() + "V");   
            }

            setovanjeNaponaCvorovima(x, promenjive, formule.Length, tacke);

        } // resava jednacine ispise resenja i logicki ih vrati u sistem

        static void setovanjeNaponaCvorovima(double[] vre, string[] cvorovi, int n, tacka[] tacke)
        {
            
            for (int i = 0; i < n; i++) 
            { 
            
                for(int j = 0;j < tacke.Length; j++)
                {

                    tacke[j].setovanjeNapona(cvorovi[i + 1], vre[i]);

                }

            }

        }// setuje napone na cvorovima 

        static int proveraKola1(grana[] grane, tacka[] tacke)
        {
            int error = 0;

            for (int i = 0;i < grane.Length; i++)//setuje napon na eventualno ne setovane cvorove
            {
                grane[i].IdealnaPolu();
            }

            for(int i = 0; i < tacke.Length; i++) 
            {
                if (tacke[i].CvorProvera() == 1)
                {
                    if (tacke[i].jePoznat() == 1)
                    {

                    }
                    else
                    {
                        error = 101;
                        Console.WriteLine("Doslo je do prekida izvrsavanja programa !!!! \n" +
                                          "Postoji nepoznata greska u resavanju kola \n" +
                                          "Postoje cvorovi na kojima nije odredjen potencijal !!!");
                    }
                }
            }


            return error;
        }//proverava da li su svi potencijali odredjeni ,
                                                                //prethodno odredi potencionalno nepoznate potencijale
        static void odredjivanjeStrujeKola(grana[] grane, komponenta[] elementi)
        {
            for(int i = 0;i<grane.Length;i++)
            {
                grane[i].StrujaGrane();
                grane[i].strujaPGrane();
            }

            for (int i = 0; i < elementi.Length; i++)
            {

                elementi[i].azurirajStruju();

            }

            for (int i = 0; i < grane.Length; i++)
            {
                grane[i].StrujaNaponskeGrane();
                
            }

            for (int i = 0; i < elementi.Length; i++)
            {

                elementi[i].azurirajStruju();

            }
        }//odredjuje sve struje u kolu
    }
}