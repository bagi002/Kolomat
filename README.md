# Kolomat

Program za resavanje elektricnih kola Vremenski konstantnih struja

Opis projekta nalazi se u prilozenom .PDF fajlu

(Prilikom pokretanja i testiranja programa fajl kolo.txt kopirati na mjesto izvrsne datoteke i 
 po pottrebi izmjeniti u skladu sa specifikacijama , program u suprotnom nece raditi
 Adresa fajla nakon komplairanja u VS "\Kolomat\Kolomat\bin\Debug\net6.0"
 U slucaju greske pisati na mejl blagojemilosevic123@gmail.com
 )

Trenutno inplementirano

-klasa tipa komponenta
-klasa tipa tacka 
-klasa tipa grana

trenutno inplementirane funkcionalnosti

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
   --------------------------------- 08/04/2023   16:48 -----------------------------------------------
   