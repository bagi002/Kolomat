using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Kolomat
{

    public class tacka
    {
        public int id;
        bool cvor = false;
        bool vestak = false;
        bool poznat = false;
        komponenta[] konekcije = null;
        int nkonekcija = 0;
        double I;
        double V;
        tacka zamjena = null;

        public string krozGranuF3(grana trenutna, tacka uC, int brv)
        {
            string a = "";

            tacka privremena = uC;

            komponenta prateca = null;

            for (int i = 0; i < nkonekcija; i++)
            {
                if (konekcije[i].proveraPripadnostiGrani(trenutna) == 1)
                {
                    prateca = konekcije[i];
                    break;
                }
            }
            komponenta prva = prateca;

            if (brv > 0)
            {

                while (trenutna.proveraKraja(privremena) != 1)
                {


                    a = a + prateca.dodavanjeNaponaF3(ref privremena);

                    if (privremena.konekcije[0] == prateca)
                    {
                        prateca = privremena.konekcije[1];

                    }
                    else
                    {
                        prateca = privremena.konekcije[0];
                    }

                }
            }
            a = a + ") / (";

            privremena = uC;
            prateca = prva;

            int pr1 = 0;

            while (trenutna.proveraKraja(privremena) != 1)

            {

                a = a + prateca.dodavanjeOtporaF3(ref privremena, ref pr1);

                if (privremena.konekcije[0] == prateca)
                {
                    prateca = privremena.konekcije[1];

                }
                else
                {
                    prateca = privremena.konekcije[0];
                }
            }

            a = a + ") ";

            return a;
        } // iz date tacke prolazi kroz granu i ispisuje formule sa brojevnim vrednostima

        public string krozGranuF2(grana trenutna, tacka uC, int brv)
        {
            string a = "";

            tacka privremena = uC;

            komponenta prateca = null;

            for (int i = 0; i < nkonekcija; i++)
            {
                if (konekcije[i].proveraPripadnostiGrani(trenutna) == 1)
                {
                    prateca = konekcije[i];
                    break;
                }
            }
            komponenta prva = prateca;

            if (brv > 0)
            {

                while (trenutna.proveraKraja(privremena) != 1)
                {


                    a = a + prateca.dodavanjeNaponaF2(ref privremena);

                    if (privremena.konekcije[0] == prateca)
                    {
                        prateca = privremena.konekcije[1];

                    }
                    else
                    {
                        prateca = privremena.konekcije[0];
                    }

                }
            }
            a = a + ") / (";

            privremena = uC;
            prateca = prva;

            int pr1 = 0;

            while (trenutna.proveraKraja(privremena) != 1)

            {

                a = a + prateca.dodavanjeOtporaF2(ref privremena, ref pr1);

                if (privremena.konekcije[0] == prateca)
                {
                    prateca = privremena.konekcije[1];

                }
                else
                {
                    prateca = privremena.konekcije[0];
                }
            }

            a = a + ") ";

            return a;
        }//prolazi iz date tacke kroz granu i ispisuje formulu po MPC
        public double krozGranuStruja(grana trenutna, tacka uC, int bri)
        {
            double a = 0;

            tacka privremena = uC;

            komponenta prateca = null;

            for (int i = 0; i < nkonekcija; i++)
            {
                if (konekcije[i].proveraPripadnostiGrani(trenutna) == 1)
                {
                    prateca = konekcije[i];
                    break;
                }
            }

            while (trenutna.proveraKraja(privremena) != 1)
            {

                a = a + prateca.vratiStrujuIS(ref privremena);

                if (privremena.konekcije[0] == prateca)
                {
                    prateca = privremena.konekcije[1];

                }
                else
                {
                    prateca = privremena.konekcije[0];
                }

            }




            return a;
        }//prolazi iz date tacke kroz granu i odredjuje jacinu struje u grani


        public void SetPoznati(tacka uC, grana ta,double a)
        {
            V = a;
            tacka privremena = this;
            poznat = true;
            komponenta prethodna;

            for (int i = 0; i < nkonekcija; i++)
            {
                if (konekcije[i].proveraPripadnostiGrani(ta) == 1)
                {
                    V += konekcije[i].NaponIdealGrana(ref privremena, ta);
                    prethodna = konekcije[i];

                    while (ta.proveraKraja(privremena) == 0 && privremena != uC)
                    {
                        if (privremena.konekcije[0] == prethodna)
                        {
                            prethodna = privremena.konekcije[1];
                            V += privremena.konekcije[1].NaponIdealGrana(ref privremena, ta);

                        }
                        if (privremena.konekcije[1] == prethodna)
                        {
                            prethodna = privremena.konekcije[0];
                            V += privremena.konekcije[0].NaponIdealGrana(ref privremena, ta);

                        }
                    }

                }
            }


        } //setuje cvor koji se nalazi na suprotnom kraju od referentnog kod poznate grane
        public void SetReferent()
        {
            V = 0;
            poznat = true;
        }//postavlja referentni cvor
        public void generatorGranaZaCvor(ref grana[] privremene, ref int brojac)
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
        public void vrednosti(int a, int b)
        {
            I = a;
            V = b;
        }//prima vrednosti napona i struje i stavlja ih u cvor
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
            druga.zamjena = this;

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
            for (int i = 0; i < nkonekcija; i++)
            {
                konekcije[i].promenaVeze(nova, this);
            }
            nkonekcija = 0;
        }

        public double vratiPodatak(int tst) // vraca Napon za 1arg , vraca stuju za 2 kao arg
        {
            double x = 0;
            if (nkonekcija != 0)
            {
                if (tst == 1) x = V;
                if (tst == 2) x = I;
            }
            else
            {
               x = zamjena.vratiPodatak(tst);
            }

            return x;
        }
        public int jePoznat()
        {
            int x = 0;

            if (poznat) x = 1;

            return x;
        }//vraca 1 ako je dati cvor poznat

        public string FormuleDatiOblik1(int nG, grana[] grane, ref int brojac)
        {

            string formula = " ";
            int ni = 0;

            if (cvor && !poznat)
            {
                brojac++;
                for (int i = 0; i < nG; i++)
                {
                    formula = formula + grane[i].formula1(this, ref ni);
                }

                formula = formula + " = 0";

                Console.WriteLine("  Cvor {0} :", id);
                Console.WriteLine("              " + formula);

                
            }

            return formula;

        }// za dati cvor ako zadovoljava uslove ispisuje formulu prvog nivoa odnosno prvi kirkofov zakon za cvorove

        public string FormuleDatiOblik2(int nG, grana[] grane, ref int brojac)
        {

            string formula = "";
            int ni = 0;

            if (cvor && !poznat)
            {
                brojac++;
                for (int i = 0; i < nG; i++)
                {
                    formula = formula + grane[i].formula2(this, ref ni);
                }

                formula = formula + " = 0";

                Console.WriteLine("  Cvor {0} :", id);
                Console.WriteLine("              " + formula);
            }

            return formula;

        }// za dati cvor ispise odgovarajucu formulu po MPC


        public string FormuleDatiOblik3(int nG, grana[] grane, ref int brojac, ref string formula)
        {

            formula = "";
            int ni = 0;

            if (cvor && !poznat)
            {
                brojac++;
                for (int i = 0; i < nG; i++)
                {
                    formula = formula + grane[i].formula3(this, ref ni);
                }

                formula = formula + " = 0";

                Console.WriteLine("  Cvor {0} :", id);
                Console.WriteLine("              " + formula);
            }

            return formula;

        }// za dati cvor ispise odgovarajucu formulu po MPC

        public void setovanjeNapona(string x, double vr)
        {
            if (cvor && !poznat)
            {
                string xa = "";
                int a = 0;


                foreach (char c in x)
                {
                    if (a != 0) xa = xa + c;
                    a++;
                }

                a = int.Parse(xa);

                if (a == id)
                {
                    poznat = true;
                    V = vr;
                    I = 0;
                }
            }
        }// za date ulazne podatke ako ulazni podatak ima poklapanje za
         // dati cvor setovati mu vrijednost na onu koja se prosljedi parametrom

        public double krozGranuNapon(grana trenutna, tacka uC)
        {
            double a = 0;

            tacka privremena = uC;

            komponenta prateca = null;

            for (int i = 0; i < nkonekcija; i++)
            {
                if (konekcije[i].proveraPripadnostiGrani(trenutna) == 1)
                {
                    prateca = konekcije[i];
                    break;
                }
            }
             a = a + prateca.vratiNaponE(ref privremena);
            if(privremena.konekcije[0] == prateca)
                {
                prateca = privremena.konekcije[1];

            }
                else
            {
                prateca = privremena.konekcije[0];
            }

            while (trenutna.proveraKraja(privremena) != 1)
            {

                a = a + prateca.vratiNaponE(ref privremena);

                if (privremena.konekcije[0] == prateca)
                {
                    prateca = privremena.konekcije[1];

                }
                else
                {
                    prateca = privremena.konekcije[0];
                }

            }




            return a;
        }//prolazi iz date tacke kroz granu i odredjuje Napon generatora u grani za potrebe racunanja struje

        public double krozGranuOtpor(grana trenutna, tacka uC)
        {
            double a = 0;
            tacka privremena = uC;

            komponenta prateca = null;

            for (int i = 0; i < nkonekcija; i++)
            {
                if (konekcije[i].proveraPripadnostiGrani(trenutna) == 1)
                {
                    prateca = konekcije[i];
                    break;
                }
            }

            a = a + prateca.vratiOtporR(ref privremena);

            if (privremena.konekcije[0] == prateca)
            {
                prateca = privremena.konekcije[1];

            }
            else
            {
                prateca = privremena.konekcije[0];
            }

            while (trenutna.proveraKraja(privremena) != 1)
            {

                a = a + prateca.vratiOtporR(ref privremena);

                if (privremena.konekcije[0] == prateca)
                {
                    prateca = privremena.konekcije[1];

                }
                else
                {
                    prateca = privremena.konekcije[0];
                }

            }




            return a;
        }

        public double StrujeTacka()//vrati zbir struja u cvor sluzi prilikom odredjivanja privremeno, nije realna situacija
        {
            double a = 0;
            double b = 0;

            for(int i = 0; i < konekcije.Length; i++)
            {
                b = konekcije[i].vratPodatak(2);
                a += konekcije[i].vratPodatak(2) * konekcije[i].proveriSmer(this);
            }

            return a;
        }

        public void predjiPrekoKomponente(ref tacka sledeca,ref komponenta predjena,grana pregledana,ref double stariPotecijal)
        {
            if (nkonekcija > 2 || vestak)
            {
                for (int i = 0; i < nkonekcija; i++)
                {
                    if (konekcije[i].proveraPripadnostiGrani(pregledana) == 1)
                    {

                        predjena = konekcije[i];
                        sledeca = predjena.SuprotniCvor(this);
                    }
                }
            }
            else
            {
                if (predjena == konekcije[0])
                {
                    predjena = konekcije[1];

                }
                else if (predjena == konekcije[1])
                {
                    predjena = konekcije[0];
                }

                sledeca = predjena.SuprotniCvor(this);
            }
            stariPotecijal = V;
        }//predji preko komponente koja je vezana na dati cvor vrati sledeci cvor i javi vezu na komponentu

        public void setovanjeTacaka (double x, double y,double z)
        {
            if (!poznat)
            {
                V = x + y;
                poznat = true;
                I = z;
            }
        }//setuje tacku na poznate dobavljene vrjednosti

        public void PotencijalKrajaZice(tacka druga)
        {
            if(this.poznat)
            {
                druga.poznat = true;
                druga.V = this.V;
            }
            else
            {
                this.poznat = true;
                this.V = druga.V;
            }
        }// fija za odredjivanje tacke suprotne poznatoj na zici

        public void VestackiCvor()
        {
            cvor = true;
            vestak = true;
            
        } // vestacki setuje tacku na cvor , tretira se kao vestacki cvor ubuduce

        public void PotencijalCvoraIspis()
        {
            if(cvor && poznat)
            {
                Console.WriteLine("V{0} = {1:0.000}V",id,V);
            }
        }

    }

}
