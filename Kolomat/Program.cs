﻿using System.Dynamic;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

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
            Console.ReadLine();

            if(error == 0) error = proveraPon(n, element);
            if(error == 0) error = dodavanjeTacaka(ref nT,ref tacke,n,element);
            if(error == 0) ispisTacakaTest(tacke, nT);
            if(error == 0) uklanjanjeViskaCvorova(ref element,n);
            if (error == 0) ispisTacakaTest(tacke, nT);
            if (error == 0) setovanjeCvorova(tacke, nT);
            if (error == 0) error = proveraKonekcija(tacke, nT);
            if (error == 0) error = proveraElemenata2(element,n);
            if (error == 0) kreiranjeGrana(tacke, nT, ref grane, ref nG);
            if (error == 0) ispisGrana(grane, nG);


            if (error == 1) Console.WriteLine("Prekid izvrsavanja usled gore navedene greske !!!!! \n\n\n");
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
                n = int.Parse(fajl.ReadLine());

                element = new komponenta[n];


                for (int i = 0; i < n; i++)
                {
                    element[i] = new komponenta(fajl.ReadLine(), i);
                    Console.WriteLine("Ucitan");
                }

              

                fajl.Close();

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

        static void kreiranjeGrana(tacka[] tacke, int nT,ref grana[] grane,ref int nG) 
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
                    privremeni[i].proveraDuplikata(privremeni[j]);
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

    }


    class komponenta
    {
        int id;
        String tip = "";
        int n;
        int a;
        tacka ak = null;
        int b;
        tacka bk = null;
        double I;
        double V;
        double P;

        double vrednost = -1;

        public komponenta(string x, int br)
        {
            id = br;
            string[] par = x.Split(' ');
            string priv = "";
            int i = 1;

            if (Char.IsLetter(par[0][0])) tip = tip + par[0][0];
            if (Char.IsLetter(par[0][1]))
            {
                tip = tip + par[0][1];
                i++;
            }
            if (Char.IsNumber(par[0][i])) priv = priv + par[0][i];
            i++;
            if (par[0].Length == i + 2)
            {
                if (Char.IsNumber(par[0][i])) priv = priv + par[0][i];
            }
            n = int.Parse(priv);

            a = int.Parse(par[1]);
            b = int.Parse(par[2]);

            if (tip.CompareTo("W") != 0) vrednost = double.Parse(par[3]);


        } //konstruktor metoda prima string ucitan iz txt fajla i rasclanjuje ga vadeci revantne podatke
        public void test1()
        {
            Console.WriteLine("Komponenta {0} je tipa {1} ({4}) \n" +
                " Spojena na krajeve {2} i {3} ", id, tip, a, b, n);
            if (vrednost > -1) Console.WriteLine("Vrednost komponente je : {0}\n\n", vrednost);
        }//metoda za svrhu testiranja , ispis sadrzaja objekta
       
        public int uporedi (komponenta druga )
            { 
                int prov = 0;

               if( tip.CompareTo(druga.tip) == 0 ) 
            {
                if (n == druga.n )
                {
                    if ((a == druga.a) && (b == druga.b) && (vrednost == druga.vrednost) )
                    {
                        prov = 1;
                    }
                }
            }

                return prov;
            } // vraca 1 ako su dve komponente iste / 0 ako su razlicite

        public void uvzivanje(ref tacka[] tacke,ref int nT)
        {
            int test = 0;
            int adr = 0;
            tacka[] priv = new tacka[nT+2];

            for(int i = 0;i< nT ;i++) priv[i] = tacke[i];

            for(int i = 0;i < nT ;i++)
            {
                if (priv[i].provera(a) == 1)
                {
                    test = 1;
                    adr = i;
                }
            }
            if (test == 0)
            {
                priv[nT] = new tacka(a,this);
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

       public void promenaVeze(tacka nova,tacka stara) // azurira veze date komponente sve stare veze zamjeni novim
        {
            if (ak == stara) {
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

            if((bk == pristupna && ak.CvorProvera() == 1) || (ak == pristupna && bk.CvorProvera() == 1))
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
       
       
    } 

    class tacka
    {
        public int id;
        public bool cvor = false;
        bool poznat = false;
        komponenta[] konekcije = null;
        int nkonekcija = 0;
        double I;
        double V;

        public void generatorGranaZaCvor(ref grana[] privremene,ref int brojac)
        {
            if (cvor)
            {
                for (int i = 0; i < nkonekcija; i++)
                {
                    privremene[brojac] = new grana(this, konekcije[i], brojac);
                    brojac++;

                }
            }
        }// za dati cvor generise sve grane koje polaze iz njega
        public int brVezaCvor()
        {
            int x = 0;
            if (cvor) x = nkonekcija;
                return x;
        }//vrati br konekcija ako je tacka cvor inace 0
        public komponenta vratiKomponentu(komponenta priv)
        {
            komponenta nova = null;
            int x = 0;

            if (konekcije[0] == priv && x == 0)
            {
                x = 1;
                nova = konekcije[1];
            }

            if (konekcije[1] == priv && x == 0)
            {
                x = 1;
                nova = konekcije[0];
            }

            return nova;
        }//vraca narednu komponentu konektovanu na ovaj cvor // ide kroz granu

        public int CvorProvera()
        {
            int x = 0;
            if (cvor) x = 1;
            return x;
        }//vraca 1 ako je data tacka cvor , 0 ako ta tacka nije cvor
        public void vrednosti(int a,int b)
        {
            I = a;
            V = b;  
        }
        public int povezanost()
        {
            int x = 0;

            if (nkonekcija == 1) x = 1;

            return x;
        }//vraca 0 ako je tacka uredu , ako tacka visi detektovati prekid grane vratiti 1

        public void jeCvor()
        {
            if (nkonekcija > 2) cvor = true;
        }//proverava da li je data tacka cvor >2 konekcije menja logicku postavku za cvor
        public void spojiSa(tacka druga) //spaja datu tacku sa tackom datom kao argument ,
                                         //na nivou izmene konekcija za novu tacku i broja konekcija date tacke
        {
            int nnovo = nkonekcija + druga.nkonekcija - 2;
            komponenta[] priv = new komponenta[nnovo];

            int j = 0;
            for (int i = 0; i < nkonekcija; i++)
            {
                if (konekcije[i].proveraVeza(this, druga) == 0)
                {
                    priv[j] = konekcije[i];
                    j++;
                }

            }
            
            for (int i = 0; i < druga.nkonekcija; i++)
            {
                if (druga.konekcije[i].proveraVeza(this, druga) == 0)
                {
                    priv[j] = druga.konekcije[i];
                    j++;
                }

            }
            konekcije = priv;
            nkonekcija = nnovo;

        }

        public void testIspis()
        {
            if (nkonekcija != 0)
            {
                Console.WriteLine("tacka {0} ima {1} konekcija \n", id, nkonekcija);
            }
        } // ispis tacke u svrhu testiranja programa

        public tacka(int kon, komponenta trenutna)
        {
            id = kon;
            konekcije = new komponenta[1];
            konekcije[0] = trenutna;
            nkonekcija++;
            V = 0;
            I = 0;
        } // Kreira Tacku

        public void dodajKomponentu(komponenta trenutna)
        {
            nkonekcija++;
            komponenta[] privremeni = new komponenta[nkonekcija];

            for (int i = 0; i < nkonekcija - 1; i++)
            {
                privremeni[i] = konekcije[i];
            }

            privremeni[nkonekcija - 1] = trenutna;
            konekcije = privremeni;
        }// Dodaje komponentu na tacku;

        public int provera(int x)//vraca 1 ako se poklapa inace 0
            {
            int u = 0;
            if (x == id) u = 1;

                return u;
            }

        public void ukloniVezu(tacka nova) // uklonice veze svim elementima vezanim na tu tacku i
                                           // prebaciti ih na novu tacku datu argumentom
        {
            for(int i = 0; i < nkonekcija; i++)
            {
                konekcije[i].promenaVeze(nova, this);
            }
            nkonekcija = 0;
        }

    }


    class grana
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
        int I = 0;

        public grana(tacka ulaz,komponenta prva,int x)
        {
            id = x;
            uC = ulaz;
            int provera = 0;
            komponenta priv = prva;
            tacka pristupna = uC;


            while (provera >= 0)
            {
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
                    case 1-5:
                        brv++;
                        break;
                    case 2-5:
                        bri++;
                        break;
                    case 3-5:
                        brr++;
                        break;
                    case 4-5:
                        brc++;
                        break;

                } // broji komponente u zavisnosti od povratne vrednosti

                if (provera >= 0)
                {
                    priv = pristupna.vratiKomponentu(priv);
                }
            }

            if (brc == 0 && bri == 0 && brr == 0 && brv > 0) idealanF = true;
            if (brc == 0 && bri == 0) zaFormulu=true;


        }//generise granu
        
        public void proveraDuplikata(grana druga)
        {
            if (!duplikat)
            {
                if(this.uC == druga.izC && this.izC == druga.uC && !druga.duplikat && bri == druga.bri && brv == druga.brv 
                    && brr == druga.brr && brc == druga.brc)
                {
                    duplikat = true;
                }
            }
        }//ako je poredjena grana ista kao i posmatran posmatrana se proglasava duplikatom

        public void ispisiGranu()
        {
            Console.WriteLine(" Grana {0}. Iz cvora {1} u cvor {2} \n" +
                "U grani se nalzi : {3} Naponskih generatora\n" +
                "                   {4} Strujnih generatora\n" +
                "                   {5} Otpornika\n" +
                "                   {6} Kondezatora",id,uC.id,izC.id,brv,bri,brr,brc);
            if (zaFormulu) Console.WriteLine("Za datu granu je moguce pisati formulu");
            if (idealanF) Console.WriteLine("Radi se o idealnoj grani sa samo naponskim generatorima");
            Console.WriteLine("----------------------------------------------------------------------");
        }// test funkcija za ispis grane sa osnovnim informacijama o istoj

        public void refakturisiID(int x)
        {
            id = x;
        }//menja id komponente nakon sto je doslo do sredjivanja liste elemenata
    }
}