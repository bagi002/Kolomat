# Kolomat

Program za resavanje elektricnih kola Vremenski konstantnih struja

Opis projekta nalazi se u prilozenom .PDF fajlu

(Prilikom pokretanja i testiranja programa fajl kolo.txt kopirati na mjesto izvrsne datoteke i 
 po pottrebi izmjeniti u skladu sa specifikacijama , program u suprotnom nece raditi
 Adresa fajla nakon komplairanja u VS "\Kolomat\Kolomat\bin\Debug\net6.0"
 U slucaju greske pisati na mejl blagojemilosevic123@gmail.com)

 Sve funkcionalno zavrsene cjeline u vidu verzija nalaze se odvojeno zipovane
 Tu se ne nalaze kodovi vec sam program izvrsna verzija 
 U datom folderu samo izmjenite .txt fajl u skladu sa specifikacijama i pokrenete .exe fajl koji se tu nalazi

  PROGRAM TRENUTNO IMA ODREDJENI BROJ ISPISA KOJI NISU PREDVIDJENI SPECIFIKACIJOM TU SE 
  NALAZE RADI TESTIRANJA FUNKCIONALNOSTI FUNKCIJA KOJE RADE U POZADINI !!!!

 ------------------------------------------------------------------------------------------------

Trenutno inplementirano :

      -klasa tipa komponenta
 
      -klasa tipa tacka 

      -klasa tipa grana

trenutno inplementirane funkcionalnosti

---------------------------- ISTORIJA RADA NA KODU (VERSION HISTORY) -----------------------------

-  Ucitavanje sirovih podataka o komponentama iz fajla kolo.txt i sredjivanje tih podataka
   tako da se dobije niz podataka tipa komponenta
-  (test funkcija) ispis svih komponenti sa informacijama o njima
-  Provera da li postoje greske u zadavanju kola odnosno dali postoji greskom dva puta zadata
   ista komponenta u kolu ( dve identicne komponente su komponente istog naziva sa istim konekcijama
   i osobinama i to ce se smatrati da je komponentat greskom dva puta unjeta)
-  Kreiranje liste objekata tipa tacka koja sadrzi sve tacke koje se nalaze u kolu ,
   u ovoj fazi program na logickom nivou uvezuje tacke sa komponentama , ali sada
   se i komponente funkcionalno povezu sa tackama
-  Funkcija koja ispsiuje sve tacke u kolu i informacije o njima (Funkcija nastala u svrhu testiranja)
-  Funkcija za uklanjanje viska cvorova iz kola , postoje dve tacke ili cvora koji su povezani
   kratko spojnikom i u tom slucaju smatraju se istim cvorom / funkcija uklanja suvisne cvorove
   i vodi evidenciju da azurira cvorove , komponente i konekcije u odnosu na dati slucaj.
-  Funkcija koja proverava da li je neka tacka ustvari i cvor ,  te definise koje tacke ce biti cvor
-  Funkcija koja provjerava da li su sve komponente dpbro povezane , tj provjerava da li 
   postoje potecionalni prekidi u kolu tj ne zatvorene strujne grane , ako strujno kolo nije zatvoreno
   program prekida izvrsavanje
-  Funkcija koja provjerava da li postoje komponente u kratkom spoju , pogresno zadate ako ih 
   nadje vraca gresku u progrmu i prekida izvrsavanje

   ----------------------------- 08/04/2023   16:48 -------------------------------------------

-  Implementirana je klasas koja sadrzi sve grane u kolu , vodeci racuna o polaznom i krajnjem 
   Cvoru , broju komponenata osbinama grane , jacine struje u grani
-  funkcija koja iz niza objekata tacke i objekata komponente generise niz objekata tipa grana
   vodeci racuna da se broj grana ispravno odredi i sprjeci dupliciranje grana kao i da se za svaku granu
   odrede svi parametri sem jacine struje 
-  Test funkcija za ispis svih grana u kolu  sa osnovnim osobinama 0

------------------------------ 09/04/2023 13:00 v.beta.5 ---------------------------------------

-  Implementirana funkcija koja nalazi referntni cvor u kolu setuje njegovu vrednost
    Fija vodi racuna o granama koje imaju samo idealne naponske generatore u sebi i 
    bira najpovoljniji cvor u kolu a zatim potencijalno odredjuje i sve druge cvorove
    cije potencijale moze da odredi
-  Implementirana Funkcija za ispis formula po MPC i to do odredjene faze 
        + Ispis formula sa strujama tipa I1 - I2 + I3 = 0 za svaki cvor
        + Formule napisane preko MPC gdje se I1 npr mjenja sa (V1 - V2 + E3)/ (R1 + R2)
        + Formule po MPC sa uvrstenim brojnim vrjednostima koje su poznate

------------------------------ 10/04/2023 16:00 BETA V.7 ----------------------------------------

-  Implementirane 4 nove funkcije koje obradjuju izgled formula po cvorovima u 4 kruga obrade
   tako da se korisniku ispisuje postepeno razradjivanje i resavanje sistema jednacina
   pa se od polaznih jednacina npr "I1 - I2 + I3 = 0" dodje do " 2 * V1 - 4 * V4 = 10"
-  Ispravljen niz gresaka na vec napisanim funkcijama 
-  Potpuno kreiran ispis formula ( Isti je trenutno malo netacan samo iz razloga jer nije implementirana
                                   funkcija koja granama sa Idealnim strujnim generatorom odredjuje I
                                    ono je za potrebe testa trenutno 0)

------------------------------ 11/04/2023 12:05 BETA V.8 -----------------------------------------

- Implementirana funkcija za odredjivanje struje u granama u kojima se nalaze strujni generatori
- Ispravljen niz gresaka u pisanju formula i bagova u istime (potrebno jos testiranja)
- Ispravljeni bagovi prikom unosa podataka omogucen unos komponenata bey broja samo IS ,W ,R
- Ispravljena nekolicina bagova u samoj logici programa (konstruktor grana)
- !!! IMPLEMENTIRANA FUNKCIJA KOJA RJESAVA SISTEM JEDNACINA i ispisuje resenja istih

------------------------------ 11/04/2023 21:30 ALFA V.1 ------------------------------------

- Popravljeni bagovi prilikom ucitavanja datoteka (ispravljene potencionalne greske)
- Redizajnirana funkcija za ucitavanje u skladu sa specifikacijama
- Otkolonjeni manji bagovi na funkciji za ispis formula
- Inplementirana funkcija koja odredjuje struju u svim djelovima kola 
- Popravljene greske kod funkcije za trazenje jacine struje u granama sa idealnim naponskim generatorom
- Kod podeljen po datotekama radi vece preglednosti
- Svaka klasa izdvojena u posebnu datoteku

------------------------------ 14/04/2023 00:10 ALFA V.2 ------------------------------------------