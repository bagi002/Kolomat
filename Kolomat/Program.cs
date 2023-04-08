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

        static void ispisJednacina(tacka[] tacke, int nT) 
        {
            int x = 0;
            for(int i = 0;i < nT; i++)
            {

                if (tacke[i].cvor && x == 0)
                {
                    x++;
                    tacke[i].vrednosti(0, 0);
                }
                if (tacke[i].cvor)
                {

                    x++;


                }

            }
        }

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
        int I;
        int V;
        int P;

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


    } 

    class tacka
    {
        public int id;
        public bool cvor = false;
        komponenta[] konekcije = null;
        int nkonekcija = 0;
        int I;
        int V;

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
        int id;
        tacka uC;
        tacka izC;
        int bri;
        int brv;
        int brr;
        int brc;
        bool zaFormulu = false;
        bool idealanF = false;

        public grana(tacka ulaz)
        {

        }
    }
}